using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class BulletScript : MonoBehaviour
{
    /* Vague bullet rules
     * -All bullets with bounce will not decay over time
     * -All other bullets to decay in 5 seconds
     */
    [SerializeField]
    float bulletDamage =1f;
    [SerializeField]
    float bulletSpeed = 8f;
    float explosionRadius = 2f;
    [SerializeField]
    float timeToDisappear = 0f;

    float bulletAirTime;
    Rigidbody2D _rb;

    public bool isBouncy;
    public bool isPiercing;
    public bool isExplosive;

    [SerializeField]
    int bounceCount =5;
    [SerializeField]
    int pierceCount = 3;

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        if (isExplosive) AudioManager.instance.PlaySoundAtLocation(AudioManager.instance.MiscSounds[0],0.2f,transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        bulletAirTime += Time.deltaTime;
        if(timeToDisappear!=0)
        {
            if (bulletAirTime >= timeToDisappear)
            {
                Despawn();
            }
        }
        if (bulletAirTime >= 0.5f && isExplosive)
        {
            _rb.velocity = transform.right * bulletSpeed * 2f;
        }
        else
        {
            _rb.velocity = transform.right * bulletSpeed/2f;
        }

    }
    void Despawn()
    {
        if (isExplosive) Explode();
        _rb.Sleep();
        gameObject.SetActive(false);
        VFXManager.instance.Poof(transform.position);
    }



    void Explode()
    {
        //spawn particle here
        VFXManager.instance.Boom(transform.position);
        AudioManager.instance.PlayCachedSound(AudioManager.instance.ExplosionSounds, transform.position,0.2f);
        CamShaker.instance.Trauma += 0.2f;
        Collider2D[] explosionHits = Physics2D.OverlapCircleAll(transform.position, explosionRadius);
        foreach(Collider2D coll in explosionHits)
        {
            if(coll.GetComponent<Rigidbody2D>()!=null)
            {
                Rigidbody2D objRB = coll.GetComponent<Rigidbody2D>();
                objRB.AddForce((objRB.transform.position - transform.position).normalized * 5f, ForceMode2D.Impulse);
                SendDamage(coll);
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision!=null)
        {
            SendDamage(collision);          
            RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.right.normalized,1.5f);
            if (hit.collider != null)
            {
                if (hit.collider.gameObject.tag == "Obstacle" && isBouncy)
                {
                    Vector2 inDir = transform.right;
                    Vector2 inNormal = hit.normal;
                    transform.right = Vector2.Reflect(inDir, inNormal).normalized;
                    bounceCount--;
                }
            }
            if (pierceCount > 0)
                pierceCount--;
            else Despawn();
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision !=null)
        {
            SendDamage(collision.collider);
            if (collision.collider.GetComponent<IDamageable>() == null)
            {
                AudioManager.instance.PlayCachedSound(AudioManager.instance.ImpactSounds, transform.position, 0.2f);
                VFXManager.instance.Spark(transform.position, collision.contacts[0].normal);
            }
            if (isBouncy)
            {
                Vector2 inDir = transform.right;
                Vector2 inNormal = collision.contacts[0].normal;
                transform.right = Vector2.Reflect(inDir, inNormal).normalized;
                bounceCount--;
            }
            if (!isBouncy || bounceCount <=0)
                Despawn();
        }
    }
    void SendDamage(Collider2D col)
    {
        //Debug.Log("trying to send damage to " + col.gameObject.name);
        if (col.GetComponent<IDamageable>() != null)
        {
            col.GetComponent<IDamageable>().OnTakeDamage(bulletDamage);
            AudioManager.instance.PlaySoundAtLocation(AudioManager.instance.MiscSounds[3], 0.2f, transform.position, true);
        }
    }
}

