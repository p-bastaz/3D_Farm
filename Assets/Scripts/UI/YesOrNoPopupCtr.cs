using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class YesOrNoPopupCtr : MonoBehaviour
{
    [SerializeField] Text text;

    Action yes_Action;
    Action no_Action;

    public void Init(string _text, Action _yes_Action, Action _no_Action = null)
    {
        text.text = _text;
        yes_Action = _yes_Action;
        no_Action = _no_Action;

        gameObject.SetActive(true);
    }

    public void ClickYes()
    {
        SoundManager.Ins.PlaySound(SoundManager.click_Sound);
        yes_Action();
        gameObject.SetActive(false);
    }

    public void ClickNo()
    {
        SoundManager.Ins.PlaySound(SoundManager.click_Sound);
        no_Action?.Invoke();
        gameObject.SetActive(false);
    }
}
