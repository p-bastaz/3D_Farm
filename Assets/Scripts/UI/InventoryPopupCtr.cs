using Model;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryPopupCtr : MonoBehaviour, IPointerDownHandler, IDragHandler
{
    [SerializeField] RectTransform window;
    private Vector2 downPosition;

    [SerializeField] List<ItemCtr> itemCtr_list;

    [SerializeField] Text money;

    bool inited = false;

    //초기화
    public void Init()
    {
        if(!inited)
        {
            for (int i = 0; i < itemCtr_list.Count; i++)
                itemCtr_list[i].Init();

            inited = true;
        }


        List<r_item> r_item_list = DataManager.Ins.data_r_item_list.Where(r => r.item_type == 2 || r.item_type == 3).ToList();
        List<p_item> p_item_list = DataManager.Ins.p_item_list.Where(r => r_item_list.Exists(r2 => r2.item_id == r.item_id)).ToList();

        for(int i = 0; i < p_item_list.Count; i++)
            itemCtr_list[i].Init(p_item_list[i]);

        money.text = DataManager.Ins.p_item_list.SingleOrDefault(r => r.item_id == 10001).item_amount.ToString();

        gameObject.SetActive(true);
    }

    public void Close()
    {
        if(gameObject.activeSelf)
        {
            SoundManager.Ins.PlaySound(SoundManager.click_Sound);
            gameObject.SetActive(false);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        downPosition = eventData.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (eventData.rawPointerPress.name == "BG1" || eventData.rawPointerPress.name == "Title" || eventData.rawPointerPress.name == "BG2")
        {
            Vector2 offset = eventData.position - downPosition;
            downPosition = eventData.position;

            window.anchoredPosition += offset;
        }

    }


}
