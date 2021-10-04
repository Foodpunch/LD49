using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using NaughtyAttributes;

public class RoomManager : MonoBehaviour
{
    //Actually the game manager lul
    public static RoomManager instance;

    public List<Animator> GlitchedObjAnims = new List<Animator>();
    public List<Enemy> EnemyList = new List<Enemy>();

    public bool isBossDead;

    public CanvasGroup gameOver;
    public CanvasGroup victory;
    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.H))
        {
            for(int i=0; i < GlitchedObjAnims.Count; i++)
            {
                GlitchedObjAnims[i].SetTrigger("glitch");
            }
        }
        if(PlayerMovement.instance.isDead)
        {
            gameOver.alpha += Time.deltaTime / 3f;
            gameOver.interactable = true;
        }
        if (isBossDead)
        {
            victory.alpha += Time.deltaTime / 3f;
            victory.interactable = true;
        }
    }
    public Enemy GetRandomEnemy()
    {
        return EnemyList[Random.Range(0, EnemyList.Count)];
    }
    [Button]
    public void GetAllGlitchedTiles()
    {
        GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("Glitch");
        for(int i=0; i<gameObjects.Length; i++)
        {
            if(gameObjects[i].GetComponent<Animator>()!=null)
            {
                GlitchedObjAnims.Add(gameObjects[i].GetComponent<Animator>());
            }
        }

    }
    [Button]
    void HurtPlayer()
    {
        PlayerMovement.instance.TakeDamage(5f);
    }
    public void ReloadGame()
    {
        SceneManager.LoadScene(0);
    }
}
