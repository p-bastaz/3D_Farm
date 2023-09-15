using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameSceneManager : MonoBehaviour
{
    private static GameSceneManager Instance;

    public static GameSceneManager Ins
    {
        get
        {
            return Instance;
        }
    }




    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            if(Instance != this)
            {
                Destroy(gameObject);
            }
        }
    }

    public static string beforeScene;
    public static string currentScene = "StartScene";
    //[SerializeField] Image progressBar;


    public void LoadScene(string sceneName)
    {
        beforeScene = currentScene;
        currentScene = sceneName;

        if(GameObject.Find("Player") != null)
        {
            GameObject.Find("Player").GetComponent<PlayerCtr>().inputLock = true;
        }

        UIManager.Ins.fadeCtr.FadeIn(() => StartCoroutine(CoLoadScene()));

    }

    void LoadScene()
    {
        StartCoroutine(CoLoadScene());
    }

    IEnumerator CoLoadScene()
    {
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(currentScene);
        while(!asyncOperation.isDone)
        {
            yield return null;
        }

        UIManager.Ins.fadeCtr.FadeOut(() =>
        {
            if (GameObject.Find("Player") != null)
            {
                GameObject.Find("Player").GetComponent<PlayerCtr>().inputLock = false;
            }

            switch(currentScene)
            {
                case "FarmScene":
                case "HouseScene":
                    SoundManager.Ins.PlayBGM(SoundManager.farm_BGM);
                    break;
                case "TownScene":
                    SoundManager.Ins.PlayBGM(SoundManager.town_BGM);
                    break;
            }
        }
        );
    }

}
