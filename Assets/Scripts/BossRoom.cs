using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NaughtyAttributes;
public class BossRoom : MonoBehaviour
{
    bool startBossSequence;
    bool bossSpawned;
    public BossEnemy bossObj;
    public CanvasGroup BossHPGroup;
    float fadeIntime=-2.5f;

    public Transform BossSpawnPoint;
    BossEnemy cachedBoss;
    // Start is called before the first frame update
    void Start()
    {

    }
// Update is called once per frame
    void Update()
    {
        if(cachedBoss!=null)
        {
            if (startBossSequence && !cachedBoss.isDead)
            {
                fadeIntime += Time.deltaTime;
                if (fadeIntime >= 1) fadeIntime = 1;
                BossHPGroup.alpha = fadeIntime;
            }
        }
       
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player" && !startBossSequence)
        {
            startBossSequence = true;
            SummonBoss();
        }
    }
    [Button]
    void SummonBoss()
    {
        cachedBoss = Instantiate(bossObj, BossSpawnPoint.position, Quaternion.identity);
        cachedBoss.isActive = true;
        startBossSequence = true;
        AudioManager.instance.isBossTime = true;
    }

}
