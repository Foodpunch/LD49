using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    Animator _anim;
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
                _anim.SetTrigger("Open");
            }
        }
    }
    public void OpenSesame()
    {
        _anim.SetTrigger("Open");
    }
}
