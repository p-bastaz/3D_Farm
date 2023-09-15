using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseManager : MonoBehaviour
{
    [Header("player")]
    [SerializeField] private PlayerCtr playerCtr;

    private void Start()
    {
        if (GameSceneManager.beforeScene == "FarmScene")
        {
            playerCtr.transform.localEulerAngles = new Vector3(0, 270, 0);
            playerCtr.transform.localPosition = new Vector3(5.749735f, 0.5099205f, 2.954082f);
        }
    }
}
