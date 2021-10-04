using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
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

    public bool hasFireRateCore;
    public bool hasMultiShotCore;
    public bool hasBulletCore;

    public static GunScript instance;

    private void Awake()
    {
        instance = this;
    }
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
        //if(Input.GetMouseButtonDown(1))
        //{
        //    RollGunStats();
        //}
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
    [Button]
    public void RollGunStats()
    {
        AudioManager.instance.PlayCachedSound(AudioManager.instance.ReloadSounds, transform.position, 0.3f);
        shotsBeforeChange = Random.Range(minShots, maxShots + 1);
        float x = Random.Range(0f, 1f);
        float y = Random.Range(0f, 1f);
        
        if(hasFireRateCore)
            fireRate = (fireRateVariance.Evaluate(y) * 7)+ cachedFireRate;      //magic number 3 because max firerate I'd possibly want is ~6 or 7
        if(hasMultiShotCore)
        pelletCount = Mathf.RoundToInt(pelletVariance.Evaluate(x) * 5);         //max pellet count shouldddd be 5
        if(pelletCount <1)
        {
            pelletCount = 1;
        }
      
        if(hasBulletCore)
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
            shotsBeforeChange--;
            for (int i = 0; i < pelletCount; i++)
            {
                float spreadRange = Random.Range(-(spreadAngle * pelletCount), spreadAngle * pelletCount);
                Quaternion randomArc = Quaternion.Euler(0, 0, spreadRange);
                GameObject bulletClone = Instantiate(PickRandomBullet(), shootPoint.position, transform.rotation * randomArc);
            }
        }
        else
        {
            if(hasBulletCore) AudioManager.instance.PlayCachedSound(AudioManager.instance.ShootSounds, transform.position, 0.2f);
            else AudioManager.instance.PlaySoundAtLocation(AudioManager.instance.ShootSounds[0], transform.position);
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
