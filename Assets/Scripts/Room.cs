using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Room : MonoBehaviour
{
    [SerializeField]
    bool hasEntered;

    public List<Transform> EnemySpawns;
    public List<Transform> PropSpawns;
    public List<Enemy> RoomEnemyList;

    int enemyCount;

    public List<GameObject> EnvironmentStuff;
    // Start is called before the first frame update
    void Start()
    {
        SpawnAI();

    }

    // Update is called once per frame
    void Update()
    {
        if(hasEntered)
        {
            ActivateAI();
        }
        if(Input.GetKeyDown(KeyCode.M))
        {
            for(int i=0; i< EnvironmentStuff.Count; i++)
            {
                if(EnvironmentStuff[i].GetComponent<Animator>()!=null)
                EnvironmentStuff[i].GetComponent<Animator>().SetTrigger("glitch");
            }
        }
    }
    void SpawnAI()
    {
        for (int i = 0; i < EnemySpawns.Count; i++)
        {
            Enemy newEnemy = Instantiate(RoomManager.instance.GetRandomEnemy(), EnemySpawns[i].position, Quaternion.identity);
            newEnemy.deathEvent += OnEnemyDeath;
            RoomEnemyList.Add(newEnemy);
        }
    }
    void ActivateAI()
    {
        for(int i=0; i< RoomEnemyList.Count; i++)
        {
            RoomEnemyList[i].isActive = true;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            hasEntered = true;
        }
    }

    void OnEnemyDeath()
    {

    }
}
