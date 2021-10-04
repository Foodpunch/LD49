using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Enemy : MonoBehaviour,IDamageable
{
    public float maxHP = 10f;
    public float currHP;
    public int moveSpeed;
    public float fireRate;
    public float gunCooldown = 3f;

    public bool isActive;
    public bool isDead;

    public int gunCount = 3;

    public Transform gunHolder;
    public EnemyGun enemyGun;
    public List<EnemyGun> EnemyGunList = new List<EnemyGun>();

    float enemyTimer;
    float nextTimeToShoot;
    float nextTimeToMove;
    float coolDownTime;
    bool canShoot;

    [SerializeField]
    Transform playerTransform;

    public bool isTrackingPlayer;
    public bool isStationary;
    int moveIndex;

    public int minMoveIndex = 1, maxMoveIndex = 5;
    public float minMoveTime = 0.5f, maxMoveTime = 1f;

    public event Action deathEvent;
    public Animator anim;
    Rigidbody2D _rb;
    // Start is called before the first frame update
    void Start()
    {
        //get player transform
        playerTransform = PlayerMovement.instance.gameObject.transform;
        _rb = GetComponent<Rigidbody2D>();
        currHP = maxHP;    
        //init guns
        for(int i=0; i<gunCount; i++)
        {
            float angle = 360 / gunCount;

            EnemyGun newGun = Instantiate(enemyGun, gunHolder.position, Quaternion.Euler(0, 0, angle * i));
            newGun.transform.SetParent(gunHolder);
            EnemyGunList.Add(newGun);
        }
    }

    // Update is called once per frame
    void Update()
    {
        enemyTimer += Time.deltaTime;
        if(isActive && !isDead)
        {

            if (isTrackingPlayer)
            {
                //technically if I had more time I could do leading shots and more AI but fuck it!
                Vector3 playerDir = playerTransform.position - transform.position;
                gunHolder.transform.right = playerDir;
            }
            else
            {
                gunHolder.transform.rotation = Quaternion.Euler(0, 0, enemyTimer * 360f);
            }
            if (enemyTimer >= coolDownTime)
            {
                canShoot = !canShoot;
                coolDownTime = enemyTimer + gunCooldown;
            }
            if (enemyTimer >= nextTimeToShoot && canShoot)
            {
                nextTimeToShoot = enemyTimer + 1f / fireRate;
                Shoot();
            }


            //Check if stationary or moving
            if (!isStationary) Movement();
        }


    }
    void Movement()
    {
        if (enemyTimer >= nextTimeToMove)
        {
            nextTimeToMove = enemyTimer + UnityEngine.Random.Range(minMoveTime, maxMoveTime);

            moveIndex = UnityEngine.Random.Range(minMoveIndex, maxMoveIndex);
            switch (moveIndex)
            {
                case 1:  //north
                    if (CheckIfHitWall(transform.up)) moveIndex = 2;
                    _rb.velocity = transform.up * moveSpeed;
                    break;
                case 2: //south
                    if (CheckIfHitWall(-transform.up)) moveIndex = 1;
                    _rb.velocity = -transform.up * moveSpeed;
                    break;
                case 3: //east
                    if (CheckIfHitWall(transform.right)) moveIndex = 4;
                    _rb.velocity = transform.right * moveSpeed;
                    break;
                case 4: //west
                    if (CheckIfHitWall(-transform.right)) moveIndex = 3;
                    _rb.velocity = -transform.right * moveSpeed;
                    break;

            }

        }
    }

    bool CheckIfHitWall(Vector3 dir)
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, 1.5f);
        if(hit.collider != null)
        {
            if (hit.collider.tag == "Obstacle")
            {
                //Debug.Log(gameObject.name + " has hit a wall");
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
       
    }
    void Shoot()
    {
        for (int i=0; i< EnemyGunList.Count; i++)
        {
            EnemyGunList[i].Fire();
            AudioManager.instance.PlaySoundAtLocation(AudioManager.instance.MiscSounds[4], transform.position);
        }
    }
    public void OnTakeDamage(float damage)
    {
        //take hit anim here
        anim.SetTrigger("hurt");
        AudioManager.instance.PlayCachedSound(AudioManager.instance.EnemyHurtSounds,transform.position,0.4f);
        currHP-=damage;
        if(currHP <=0)
        {
            Die();
        }
    }
    void Die()
    {
        AudioManager.instance.PlaySoundAtLocation(AudioManager.instance.MiscSounds[2], 0.2f, transform.position, true);
        isDead = true;
        if(deathEvent != null)
        {
            deathEvent();
        }
        gameObject.SetActive(false);
    }
}
