using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemCore : MonoBehaviour
{
    public enum CoreType
    {
        FIRERATE,
        MULTISHOT,
        BULLET
    }
    public CoreType itemCore = CoreType.FIRERATE;
    public Door openDoor;
    public GameObject UIIndicator;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            if (openDoor != null) openDoor.OpenSesame();
            switch(itemCore)
            {
                case CoreType.FIRERATE:
                    GunScript.instance.hasFireRateCore = true;
                    GunScript.instance.RollGunStats();
                    break;
                case CoreType.MULTISHOT:
                    GunScript.instance.hasMultiShotCore = true;
                    GunScript.instance.RollGunStats();
                    break;
                case CoreType.BULLET:
                    GunScript.instance.hasBulletCore = true;
                    GunScript.instance.RollGunStats();
                    break;
            }
            gameObject.SetActive(false);
            UIIndicator.SetActive(true);
        }
    }
}
