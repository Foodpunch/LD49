using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXManager : MonoBehaviour
{
    public static VFXManager instance;

    public GameObject[] VFXObjects;

    // Start is called before the first frame update
    private void Awake()
    {
        instance = this;
    }

    // Update is called once per frame
    public void Poof(Vector3 pos)
    {
        Instantiate(VFXObjects[0], pos, Quaternion.identity);
    }
    public void Spark(Vector3 pos, Vector3 normal)
    {
        GameObject spark = Instantiate(VFXObjects[1], pos, Quaternion.identity);
        spark.transform.right = normal;
    }
  
}
