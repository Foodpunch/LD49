using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    public static RoomManager instance;

    public List<Enemy> EnemyList = new List<Enemy>();
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
        
    }
    public Enemy GetRandomEnemy()
    {
        return EnemyList[Random.Range(0, EnemyList.Count)];
    }
}
