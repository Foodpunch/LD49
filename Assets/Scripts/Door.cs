using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    Animator _anim;
    bool isOpen;
    // Start is called before the first frame update
    void Start()
    {
        _anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision !=null)
        {
            if(collision.tag == "Player")
            {
                OpenSesame();
            }
        }
    }
    public void OpenSesame()
    {
        if (!isOpen) AudioManager.instance.PlaySoundAtLocation(AudioManager.instance.MiscSounds[1], 0.3f, transform.position);
        isOpen = true;
        _anim.SetTrigger("Open");
     
    }
}
