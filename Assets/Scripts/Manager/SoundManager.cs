using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    //sound
    static public string click_Sound = "pickUpItem";
    static public string hoe_Sound = "hoeHit";
    static public string water_Sound = "water_lap2";
    static public string harvest_Sound = "harvest";
    static public string doorOpen_Sound = "doorOpen";
    static public string get_Sound = "reward";
    static public string openChest_Sound = "openChest";
    static public string feed_Sound = "leafrustle";
    


    //bgm
    static public string farm_BGM = "Farm";
    static public string town_BGM = "Town";

    private static SoundManager Instance;
    public static SoundManager Ins
    {
        get
        {
            return Instance;
        }
    }

   
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            if (Instance != this)
            {
                Destroy(gameObject);
            }
        }
    }

    [SerializeField] AudioSource sound;
    [SerializeField] AudioSource bgm;

    [SerializeField] List<AudioClip> sound_Clips;
    [SerializeField] List<AudioClip> bgm_Clips;

    private bool isOnFade = false;
    float fadeTime = 0.5f;

    public void PlaySound(string _soundStr)
    {
        AudioClip clip = sound_Clips.SingleOrDefault(r => r.name == _soundStr);

        if(clip != null)
        {
            sound.clip = clip;
            sound.Play();
        }
    }

    public void PlayBGM(string _bgmStr)
    {
        AudioClip clip = bgm_Clips.SingleOrDefault(r => r.name == _bgmStr);

        if (clip != null && bgm.clip != clip)
        {
            if (isOnFade)
            {
                StopCoroutine(CoStopBGM());
                StopCoroutine("CoPlayBGM");
            }
                
            StartCoroutine(CoPlayBGM(clip));
        }
 
    }

    IEnumerator CoPlayBGM(AudioClip _clip)
    {
        isOnFade = true;

        while (bgm.volume > 0)
        {
            bgm.volume -= Time.deltaTime * 0.5f;
            yield return new WaitForEndOfFrame();
        }

        bgm.volume = 0.25f;
        bgm.clip = _clip;
        bgm.Play();

        isOnFade = false;
    }

    public void StopBGM()
    {
        if (isOnFade)
        {
            StopCoroutine(CoStopBGM());
            StopCoroutine("CoPlayBGM");
        }

        StartCoroutine(CoStopBGM());
    }

    IEnumerator CoStopBGM()
    {
       isOnFade = true;
       while (bgm.volume > 0)
       {
           bgm.volume -= Time.deltaTime * 0.5f;
           yield return new WaitForEndOfFrame();
       }

        bgm.clip = null;
        isOnFade = false;
    }
}
