using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public List<AudioSource> ListofAudioSources = new List<AudioSource>();

    [SerializeField]
    AudioSource source;

    public AudioSource BGMSource;
    public AudioSource GameOverSource;
    public AudioSource BossBGMSource;

    public bool isGameOver;
    public bool isBossTime;
    float fadeTime = 0f;

    //hard code babeyyy~
    public AudioClip[] ShootSounds;
    public AudioClip[] ReloadSounds;
    public AudioClip[] ImpactSounds;
    public AudioClip[] EnemyHurtSounds;
    public AudioClip[] ExplosionSounds;
    public AudioClip[] HurtSounds;
    public AudioClip[] MiscSounds;

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
        //if (isGameOver)
        //{
        //    fadeTime += Time.deltaTime;
        //    if (fadeTime < 1f)
        //    {
        //        BGMSource.volume = Mathf.Lerp(0.234f, 0f, fadeTime);
        //        GameOverSource.volume = Mathf.Lerp(0, 0.234f, fadeTime);
        //        GameOverSource.Play();
        //    }
        //}
        if (isBossTime && !isGameOver)
        {
            fadeTime += Time.deltaTime;
            if (fadeTime < 1f)
            {
                BGMSource.volume = Mathf.Lerp(0.234f, 0f, fadeTime);
                BossBGMSource.volume = Mathf.Lerp(0, 0.234f, fadeTime);
                BossBGMSource.Play();
            }
        }
        if(isGameOver)
        {
            BGMSource.volume -= Time.deltaTime/5f;
            BossBGMSource.volume -= Time.deltaTime/5f;
        }
    }

    //default volume is 0.3f
    public void PlaySoundAtLocation(AudioClip clip, Vector2 pos)
    {
        AudioSource temp = Instantiate(source, pos, Quaternion.identity);
        ListofAudioSources.Add(temp);
        temp.clip = clip;
        temp.Play();
        RemoveUnusedAudioSource();
    }
    public void PlaySoundAtLocation(AudioClip clip, float volume, Vector2 pos)
    {
        AudioSource temp = Instantiate(source, pos, Quaternion.identity);
        ListofAudioSources.Add(temp);
        temp.volume = volume;
        temp.clip = clip;
        temp.Play();
        RemoveUnusedAudioSource();
    }
    public void PlaySoundAtLocation(AudioClip clip, float volume, Vector2 pos, bool randPitch = false)
    {
        AudioSource temp = Instantiate(source, pos, Quaternion.identity);
        ListofAudioSources.Add(temp);
        if (randPitch) temp.pitch *= Random.Range(0.8f, 2.5f);   //below 0 can't hear anything, 0.5f sounds slowww 2.5f sounds fast
        temp.volume = volume;
        temp.clip = clip;
        temp.Play();
        RemoveUnusedAudioSource();
    }
    public void PlayCachedSound(AudioClip[] clips, Vector2 pos, float volume, bool randPitch = false)
    {
        AudioSource temp = Instantiate(source, pos, Quaternion.identity);
        ListofAudioSources.Add(temp);
        if (randPitch) temp.pitch *= Random.Range(0.8f, 2.5f);
        temp.volume = volume;
        temp.clip = clips[Random.Range(0, clips.Length)];
        temp.Play();
        RemoveUnusedAudioSource();
    }


    void RemoveUnusedAudioSource()
    {
        if (ListofAudioSources != null)
        {
            for (int i = 0; i < ListofAudioSources.Count; i++)
            {
                if (!ListofAudioSources[i].isPlaying)
                {
                    Destroy(ListofAudioSources[i].gameObject);
                    ListofAudioSources.RemoveAt(i);
                }
            }
        }
    }


}
