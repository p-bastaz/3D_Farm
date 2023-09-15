using Model;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CoopCtr : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    FarmManager farmManager;

    [SerializeField] p_item feed_item;

    [SerializeField] Image drag_feed;
    [SerializeField] Text feed_amount;

    [SerializeField] List<FeedCaseCtr> feedCaseCtr_list;

    public void Init()
    {
        if (farmManager == null)
            farmManager = GameObject.Find("FarmManager").GetComponent<FarmManager>();

        for (int i = 0; i < feedCaseCtr_list.Count; i++)
        {
            feedCaseCtr_list[i].Init(DataManager.Ins.coop_feed_box[i]);
        }

        feed_item = DataManager.Ins.p_item_list.SingleOrDefault(r => r.item_id == 10002);
        feed_amount.text = feed_item != null ? feed_item.item_amount.ToString() : "0";

        gameObject.SetActive(true);

        //달걀 획득
        if(DataManager.Ins.p_chicken_list.Exists(r => r.egg == true))
        {
            r_item egg_item = DataManager.Ins.data_r_item_list.SingleOrDefault(r => r.item_id == 31001);

            int egg_amount = 0;

            for(int i = 0; i < DataManager.Ins.p_chicken_list.Count; i++)
            {
                if (DataManager.Ins.p_chicken_list[i].egg)
                {
                    egg_amount++;
                    DataManager.Ins.p_chicken_list[i].egg = false;
                }
            }

            SoundManager.Ins.PlaySound(SoundManager.get_Sound);
            DataManager.Ins.ItemUpdate(egg_item, egg_amount);
            UIManager.Ins.getItemPopupCtr.Init(egg_item, egg_amount);
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {

        if (eventData.rawPointerPress.transform.parent.gameObject.name == "FeedStrorage")
        {
            drag_feed.gameObject.SetActive(true);
        }


    }

    public void OnDrag(PointerEventData eventData)
    {
        if(drag_feed.gameObject.activeSelf)
        {
            drag_feed.transform.position = eventData.position;
        }

    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (eventData.pointerEnter != null && eventData.pointerEnter.transform.parent.gameObject.name == "FeedCase")
        {
            FeedCaseCtr feedCaseCtr = eventData.pointerEnter.GetComponent<FeedCaseCtr>();

            if (feedCaseCtr.coop_box.item_id == 0)
            {
                if(feed_item != null && feed_item.item_amount > 0)
                {
                    feedCaseCtr.SetFeed();

                    r_item r_feed_item = DataManager.Ins.data_r_item_list.SingleOrDefault(r => r.item_id == feed_item.item_id);
                    DataManager.Ins.ItemUpdate(r_feed_item, -1);
                    SoundManager.Ins.PlaySound(SoundManager.feed_Sound);
                    Init();
                }
            }
        }
        drag_feed.gameObject.SetActive(false);
    }


    public void ClickMakeFeed()
    {
        SoundManager.Ins.PlaySound(SoundManager.click_Sound);
        UIManager.Ins.makeFeedPopupCtr.Init(Init);
    }

    public void Close()
    {
        SoundManager.Ins.PlaySound(SoundManager.click_Sound);
        farmManager.TurnCoopCamera(false);
    }

}
