using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeCtr : MonoBehaviour
{
    [SerializeField] Image image;
    float fadeDuration = 0.5f;

    
    public void FadeIn(Action _action = null)
    {
        gameObject.SetActive(true);

        image.DOFade(1, fadeDuration)
           .OnComplete(
           () =>
           {
               _action?.Invoke();
           }
           );
    }

    public void FadeOut(Action _action = null)
    {
        image.DOFade(0, fadeDuration)
            .OnComplete(
            () =>
            {
                _action?.Invoke();
                gameObject.SetActive(false);
            }
            );
    }
}
