using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessagePopupCtr : MonoBehaviour
{

    [SerializeField] Text title;
    [SerializeField] Text text;

    public void Init(string _text, string _title = null)
    {
        text.text = _text;

        title.text = _title != null ? _title : "알림";

        gameObject.SetActive(true);
    }

    public void ClickConfirm()
    {
        SoundManager.Ins.PlaySound(SoundManager.click_Sound);
        gameObject.SetActive(false);
    }

}
