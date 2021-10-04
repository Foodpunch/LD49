using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyBullet : MonoBehaviour
{
    public float bulletDamage = 1f;
    Rigidbody2D _rb;
    float timeToDisappear = 3.5f;
    float bulletSpeed = 2f;
    float bulletAirTime;
    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        bulletAirTime += Time.deltaTime;
        if(bulletAirTime>= timeToDisappear)
        {
            gameObject.SetActive(false);
        }
        _rb.velocity = transform.right * bulletSpeed;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision !=null)
        {
            if (collision.gameObject.tag == "Player")
            {
                collision.gameObject.GetComponent<PlayerMovement>().TakeDamage(bulletDamage);
            }
            _rb.Sleep();
            gameObject.SetActive(false);
        }
      
    }
}
