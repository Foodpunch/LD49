using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Healthpack : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag== ("Player"))
        {
            gameObject.SetActive(false);
            collision.GetComponent<PlayerMovement>().AddHP(5f);
            AudioManager.instance.PlaySoundAtLocation(AudioManager.instance.MiscSounds[5], 0.3f, transform.position);
        }
    }
}
