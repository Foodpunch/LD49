using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunScript : MonoBehaviour
{
    [SerializeField]
    float fireRate;                 //calculated by 1 second / fire rate (higher number means faster)
    float spreadAngle =8f;         //determines bullet spread
    [SerializeField]
    int pelletCount = 5;
    [SerializeField]
    bool isAuto = true;                    //determines if is auto or semi  //edit fuck it! so pointless make the player keep clicking. Just full auto it

    float gunTime;
    float nextTimeToFire;
    [SerializeField]
    List<GameObject> BulletList = new List<GameObject>();

    [SerializeField]
    int shotsBeforeChange;                      //number of shots before the gun rolls
    int minShots = 2, maxShots = 20;            //Should tie it to the highest normal enemy hp

    [SerializeField]
    Transform shootPoint;

    public bool isUnstableMode;
    GameObject cachedBullet;
    float cachedFireRate;
    [SerializeField]
    AnimationCurve fireRateVariance;
    [SerializeField]
    AnimationCurve pelletVariance;
    
    // Start is called before the first frame update
    void Start()
    {
        shotsBeforeChange = Random.Range(minShots, maxShots + 1);
        cachedBullet = BulletList[0];
        cachedFireRate = fireRate;
    }

    // Update is called once per frame
    void Update()
    {
        if(shotsBeforeChange <=0)
        {
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
        if(Input.GetKeyDown(KeyCode.G))
        {
            isUnstableMode = !isUnstableMode;
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
        shotsBeforeChange = Random.Range(minShots, maxShots + 1);
        //fireRate = FiftyPercent() ? 1f : 5f;
        //pelletCount = FiftyPercent() ? 1 : 5;
        float x = Random.Range(0f, 1f);
        float y = Random.Range(0f, 1f);
        
        fireRate = (fireRateVariance.Evaluate(y) * 7)+ cachedFireRate;      //magic number 3 because max firerate I'd possibly want is ~6 or 7
        pelletCount = Mathf.RoundToInt(pelletVariance.Evaluate(x) * 5);         //max pellet count shouldddd be 5
        if(pelletCount <1)
        {
            pelletCount = 1;
        }
        //int a = Random.Range(0, 91);
        //int b = Random.Range(0, 91);

        //if(a < 30)
        //{
        //    fireRate = 1f;
        //}
        //else if (a < 60)
        //{
        //    fireRate = 5f;
        //}
        //else
        //{
        //    fireRate = 8f;
        //}

        //if (b < 30)
        //{
        //    pelletCount = 1;
        //}
        //else if (b < 60)
        //{
        //    pelletCount = 3;
        //}
        //else
        //{
        //    pelletCount = 5;
        //}


        cachedBullet = PickRandomBullet();

    }
    GameObject PickRandomBullet()
    {
        //Fisher Yates shuffle here then maybe reset once done
        return BulletList[Random.Range(0,BulletList.Count)];       //for now return first bullet in that list
    }
    void SpawnBullet()
    {
        if(isUnstableMode)
        {
            for (int i = 0; i < pelletCount; i++)
            {
                float spreadRange = Random.Range(-(spreadAngle * pelletCount), spreadAngle * pelletCount);
                Quaternion randomArc = Quaternion.Euler(0, 0, spreadRange);
                GameObject bulletClone = Instantiate(PickRandomBullet(), shootPoint.position, transform.rotation * randomArc);
            }
        }
        else
        {
            shotsBeforeChange--;
            for (int i = 0; i < pelletCount; i++)
            {
                float spreadRange = Random.Range(-(spreadAngle * pelletCount), spreadAngle * pelletCount);
                Quaternion randomArc = Quaternion.Euler(0, 0, spreadRange);
                GameObject bulletClone = Instantiate(cachedBullet, shootPoint.position, transform.rotation * randomArc);
            }
        }

    }
    bool FiftyPercent()
    {
        int i =Random.Range(0, 2);
        return (i == 0);
    }
}
