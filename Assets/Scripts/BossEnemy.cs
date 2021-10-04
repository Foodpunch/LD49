using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NaughtyAttributes;

[RequireComponent(typeof(Rigidbody2D))]
public class BossEnemy : MonoBehaviour,IDamageable
{
    public float health = 100f;
    float maxHealth;

    public float moveSpeed= 1.5f;

    public Image healthBar;

    public bool isActive;
    public bool isDead;

    float nextTimeToMove;
    float moveTimer;
    float minMoveTime = 0.3f,maxMoveTime = 0.6f;
    int moveIndex;
    Rigidbody2D _rb;

    Vector3 cachedPos;
    Vector3 lastSummonedPos;
    float summonTimer;
    float nextTimeToSummon =5f;
    public List<Enemy> SpawnedEnemyList = new List<Enemy>();
    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        maxHealth = health;
        healthBar = GameObject.FindGameObjectWithTag("BossHP").GetComponent<Image>();
        cachedPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(isActive && !isDead)
        {
            moveTimer += Time.deltaTime;
            if(moveTimer >= nextTimeToMove)
            {
                nextTimeToMove = moveTimer + UnityEngine.Random.Range(minMoveTime, maxMoveTime);
                moveIndex = UnityEngine.Random.Range(1, 5);

                switch (moveIndex)
                {
                    case 1:  //north
                        _rb.velocity = transform.up * moveSpeed;
                        break;
                    case 2: //south
                        _rb.velocity = -transform.up * moveSpeed;
                        break;
                    case 3: //east
                        _rb.velocity = transform.right * moveSpeed;
                        break;
                    case 4: //west
                        _rb.velocity = -transform.right * moveSpeed;
                        break;
                }
            }
            if(moveTimer >= nextTimeToSummon)
            {
                lastSummonedPos = cachedPos;
                cachedPos = transform.position;
                nextTimeToSummon = moveTimer + 5;
                Summon();
            }
        }
        UpdateHP();
        CheckSpawns();
    }
    void Summon()
    {
        if(SpawnedEnemyList.Count <=3)
        {
            Enemy spawnedEnemy = Instantiate(RoomManager.instance.GetRandomEnemy(), lastSummonedPos, Quaternion.identity);
            spawnedEnemy.isActive = true;
            SpawnedEnemyList.Add(spawnedEnemy);
        }
    }
    void CheckSpawns()
    {
        if(SpawnedEnemyList.Count >0)
        {
            for(int i=SpawnedEnemyList.Count-1; i> 0; i--)
            {
                if (SpawnedEnemyList[i].isDead) SpawnedEnemyList.RemoveAt(i);
            }
        }
    }
    void UpdateHP()
    {
        healthBar.fillAmount = (health / maxHealth);
        if(isDead)
        {
            healthBar.transform.parent.GetComponent<CanvasGroup>().alpha -= Time.deltaTime;
        }
    }
    public void OnTakeDamage(float damage)
    {
        health -= damage;
        if(health <=0)
        {
            Die();
           // gameObject.SetActive(false);
        }
    }
    void Die()
    {
        isDead = true;
        KillAllSpawns();
        RoomManager.instance.isBossDead = true;
    }
    [Button]
    void ActivateBoss()
    {
        isActive = true;
    }
    [Button]
    void DamageBoss()
    {
        OnTakeDamage(Random.Range(2, 8));
    }
    [Button]
    void KillAllSpawns()
    {
        for(int i=0; i<SpawnedEnemyList.Count; i++)
        {
            SpawnedEnemyList[i].isDead = true;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision!=null)
        {
            if(collision.contacts[0].collider.tag!="Player")
            {
                if(collision.contacts[0].collider.GetComponent<Rigidbody2D>()!=null)
                {
                    collision.contacts[0].collider.GetComponent<Rigidbody2D>().AddForce((collision.contacts[0].collider.transform.position - transform.position).normalized * 5f, ForceMode2D.Impulse);
                }
            }
        }
    }
}
