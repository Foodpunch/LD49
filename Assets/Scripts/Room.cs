using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Room : MonoBehaviour
{
    [SerializeField]
    bool hasEntered;

    public List<Transform> EnemySpawns;
    public List<Enemy> RoomEnemyList;


    public Door exitDoor;
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
            if (isAllAIDead()) exitDoor.OpenSesame();
        }
        //if(Input.GetKeyDown(KeyCode.M))
        //{
        //    for(int i=0; i< EnvironmentStuff.Count; i++)
        //    {
        //        if(EnvironmentStuff[i].GetComponent<Animator>()!=null)
        //        EnvironmentStuff[i].GetComponent<Animator>().SetTrigger("glitch");
        //    }
        //}
    }
    bool isAllAIDead()
    {
        for(int i=0; i< RoomEnemyList.Count;i++)
        {
            if(!RoomEnemyList[i].isDead)
            {
                return false;
            }
        }
        return true;
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
