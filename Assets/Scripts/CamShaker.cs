//Created by Arman Awan - ArmanDoesStuff 2018, edited by sheol
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
public class CamShaker : MonoBehaviour
{

    public bool camShakeAcive = true; //on or off
    [Range(0, 1)]
    [SerializeField] float trauma;
    [SerializeField] float traumaMult = 16; //the power of the shake
    [SerializeField] float traumaMag = 0.8f; //the range of movment
    [SerializeField] float traumaRotMag = 17f; //the rotational power
    [SerializeField] float traumaDecay = 1.3f; //how quickly the shake falls off

    public static CamShaker instance;

    float timeCounter = 0; //counter stored for smooth transition
    public float Trauma //accessor is used to keep trauma within 0 to 1 range
    {
        get
        {
            return trauma;
        }
        set
        {
            trauma = Mathf.Clamp01(value);
        }
    }

    //Get a perlin float between -1 & 1, based off the time counter. edit seems more like -0.5 to 0.5 hmm
    float GetFloat(float seed)
    {
        return (Mathf.PerlinNoise(seed, timeCounter) - 0.5f) * 2f;
    }

    //use the above function to generate a Vector3, different seeds are used to ensure different numbers
    Vector3 GetVec3()
    {
        return new Vector3(GetFloat(1), GetFloat(10)) ;
    }

    private void Awake()
    {
        instance = this;
    }
    private void Update()
    {
        if (camShakeAcive && Trauma > 0)
        {
            //increase the time counter (how fast the position changes) based off the traumaMult and some root of the Trauma
            timeCounter += Time.deltaTime * Mathf.Pow(Trauma, 0.3f) * traumaMult;
            //Bind the movement to the desired range
            Vector3 newPos = GetVec3() * traumaMag * Trauma;
            newPos.z = -10;             //s: remember to set cam depth!! z -10
            transform.localPosition = newPos;
            //rotation modifier applied here
            transform.localRotation = Quaternion.Euler(0,0,newPos.x * traumaRotMag);
            //decay faster at higher values
            Trauma -= Time.deltaTime * traumaDecay * (Trauma + 0.3f);
        }
        else
        {
            //s: don't have to lerp it back because the cam manager is controlling the cam pos
            ////lerp back towards default position and rotation once shake is done
            //Vector3 ogPos = Vector3.Lerp(transform.localPosition, new Vector3(0,1,-10), Time.deltaTime);
            //transform.localPosition = ogPos;
            //transform.localRotation = Quaternion.identity;
        }

    }
    [Button]
    void Shake()
    {
        Trauma += 0.5f;
    }
}