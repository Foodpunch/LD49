using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class BulletScript : MonoBehaviour
{
    float bulletDamage;
    float bulletSpeed = 5f;
    float explosionRadius = 2f;
    float timeToDisappear = 0f;

    float bulletAirTime;
    Rigidbody2D _rb;

    public bool isBouncy;
    public bool isPiercing;
    public bool isExplosive;

    [SerializeField]
    int bounceCount =5;
    int pierceCount = 4;

    public LayerMask layerMask;
    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
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
        _rb.velocity = transform.right * bulletSpeed;

    }
    void Despawn()
    {
        if (isExplosive) Explode();
        _rb.Sleep();
        gameObject.SetActive(false);
    }



    void Explode()
    {
        //spawn particle here
        Collider2D[] explosionHits = Physics2D.OverlapCircleAll(transform.position, explosionRadius);
        foreach(Collider2D coll in explosionHits)
        {
            if(coll.GetComponent<Rigidbody2D>()!=null)
            {
                Rigidbody2D objRB = coll.GetComponent<Rigidbody2D>();
                objRB.AddForce((objRB.transform.position - transform.position).normalized * 5f, ForceMode2D.Impulse);
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision!=null)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.right.normalized,1.5f);
            // Debug.Log(hit.collider.name);
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
            foreach(ContactPoint2D contact in collision.contacts)
            {

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
    private void OnDrawGizmos()
    {
        Debug.DrawRay(transform.position, transform.right.normalized,Color.red);
        Gizmos.DrawSphere(transform.position, explosionRadius);
    }
}
