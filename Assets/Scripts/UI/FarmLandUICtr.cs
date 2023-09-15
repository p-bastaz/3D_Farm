using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class FarmLandUICtr : MonoBehaviour
{
    FarmManager farmManager;

    public void Init()
    {
        if (farmManager == null)
            farmManager = GameObject.Find("FarmManager").GetComponent<FarmManager>();

        gameObject.SetActive(true);
    }

    public void Close()
    {
        SoundManager.Ins.PlaySound(SoundManager.click_Sound);
        farmManager.TurnDirtsCamera(false);
    }
}
