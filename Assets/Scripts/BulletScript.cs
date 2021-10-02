using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class BulletScript : MonoBehaviour
{
    float bulletDamage;
    float bulletSpeed = 5f;
    float timeToDisappear = 0f;

    float bulletAirTime;
    Rigidbody2D _rb;
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
        _rb.Sleep();
        gameObject.SetActive(false);
    }
}
