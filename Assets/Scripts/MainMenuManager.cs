using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public GameObject gun;
    Vector3 mouseInput;
    Vector2 mouseDirection;
    public AudioSource source;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        mouseInput = Input.mousePosition;
        Vector3 mousePosInWord = Camera.main.ScreenToWorldPoint(mouseInput);
        mousePosInWord.z = 0;
        mouseDirection = mouseInput - gun.transform.position;
        gun.transform.right = mouseDirection;
    }
    public void StartGame()
    {
        source.Play();
        SceneManager.LoadScene(1);
    }
}
