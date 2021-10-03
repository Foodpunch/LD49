using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGun : MonoBehaviour
{
    public GameObject bullet;
    public Transform shootPoint;
    //yes I know you can probably condense this code but fuck it! 
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Fire()
    {
        Instantiate(bullet, shootPoint.position, transform.rotation);
    }
}
