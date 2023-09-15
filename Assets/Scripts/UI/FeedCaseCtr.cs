using Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class FeedCaseCtr : MonoBehaviour
{
    public p_item coop_box;

    [SerializeField] Image feed_img;

    public void Init(p_item _p_item)
    {
        coop_box = _p_item;

        feed_img.gameObject.SetActive(coop_box != null && coop_box.item_id != 0);
    }

    public void SetFeed()
    {
        coop_box.item_id = 10002;
        coop_box.item_amount = 1;

        feed_img.gameObject.SetActive(true);
    }
}
