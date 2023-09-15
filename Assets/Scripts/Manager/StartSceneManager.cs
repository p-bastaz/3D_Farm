using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartSceneManager : MonoBehaviour
{
    private void Awake()
    {
        StartCoroutine(CoUpdate());
    }

    IEnumerator CoUpdate()
    {
        while(true)
        {
            yield return null;

            if(DataManager.Ins.initDataLoad && DataManager.Ins.playerDataLoad)
            {
                GameSceneManager.Ins.LoadScene("HouseScene");
                yield break;
            }
        }
    }
}
