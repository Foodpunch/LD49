using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunScript : MonoBehaviour
{
    [SerializeField]
    float fireRate;                 //calculated by 1 second / fire rate (higher number means faster)
    float spreadAngle =10f;         //determines bullet spread
    [SerializeField]
    int pelletCount = 5;
    [SerializeField]
    bool isAuto = true;                    //determines if is auto or semi  //edit fuck it! so pointless make the player keep clicking. Just full auto it

    float gunTime;
    float nextTimeToFire;
    [SerializeField]
    List<GameObject> BulletList = new List<GameObject>();

    float stabilityMeter = 100f;                //each time you shoot, this value decreases. When the value hits 0, gun random rolls
    float gunCoolDown = 3f;
    bool isGunRolling;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(stabilityMeter <=0f)
        {
            isGunRolling = true;
            RollGunStats();
        }

        if(isAuto)
        {
            if(Input.GetMouseButton(0))
            {
                FireGun();
            }
        }
        else
        {
            if(Input.GetMouseButtonDown(0))
            {
                FireGun();
            }
        }

        //testing stuff
        if(Input.GetMouseButtonDown(1))
        {
            RollGunStats();
        }
    }
    void FireGun()
    {
        gunTime += Time.deltaTime;
        if (Time.time >= nextTimeToFire)
        {
            SpawnBullet();
            nextTimeToFire = Time.time + (1f / fireRate);
        }
    }
    void RollGunStats()
    {
        stabilityMeter = 100f;
        //int x = Random.Range(0, 101);     //determines firerate
        //int y = Random.Range(0, 2);       //determines semi or auto
        //int z = Random.Range(0, 2);       //determines multishot or not

        //Debug.Log(x);

        //if(x <=20)
        //{
        //    fireRate = 5f;
        //}
        //else if(x <= 60)
        //{
        //    fireRate = 1f;
        //}
        //else
        //{
        //    fireRate = 3f;
        //}
        //isAuto = (y == 1);
        //pelletCount = (z == 1) ? 1 : 5;

        fireRate = FiftyPercent() ? 1f : 5f;
        //isAuto = FiftyPercent();
        pelletCount = FiftyPercent() ? 1 : 5;



    }
    GameObject PickRandomBullet()
    {
        //Fisher Yates shuffle here then maybe reset once done
        return BulletList[0];       //for now return first bullet in that list
    }
    void SpawnBullet()
    {
        for(int i =0; i< pelletCount; i++)
        {
            float spreadRange = Random.Range(-(spreadAngle*pelletCount), spreadAngle*pelletCount);
            Quaternion randomArc = Quaternion.Euler(0, 0, spreadRange);
            GameObject bulletClone = Instantiate(PickRandomBullet(), transform.position, transform.rotation * randomArc);
        }
    }
    bool FiftyPercent()
    {
        int i =Random.Range(0, 2);
        return (i == 0);
    }
}
