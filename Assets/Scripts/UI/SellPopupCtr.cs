using Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class SellPopupCtr : MonoBehaviour
{
    [SerializeField] List<ItemCtr> inventory_Item_List;

    [SerializeField] List<ItemCtr> sell_Item_List;

    Action close_Action;

    private void Awake()
    {
        //버튼 클릭 이벤트 할당
        for (int i = 0; i < inventory_Item_List.Count; i++)
        {
            int temp = i;
            Button tempButton = inventory_Item_List[temp].GetComponent<Button>();
            tempButton.onClick.AddListener(() => ClickInventoryItem(inventory_Item_List[temp]));
        }

        for (int i = 0; i < sell_Item_List.Count; i++)
        {
            int temp = i;
            Button tempButton = sell_Item_List[temp].GetComponent<Button>();
            tempButton.onClick.AddListener(() => ClickSellItem(sell_Item_List[temp]));
        }
    }

    public void Init(Action _close_action = null)
    {
        if (_close_action != null)
            close_Action = _close_action;

        for (int i = 0; i < inventory_Item_List.Count; i++)
                inventory_Item_List[i].Init();

        for (int i = 0; i < sell_Item_List.Count; i++)
                sell_Item_List[i].Init();

        List<r_item> r_item_list = DataManager.Ins.data_r_item_list.Where(r => r.item_type == 3).ToList();
        List<p_item> p_item_list = DataManager.Ins.p_item_list.Where(r => r_item_list.Exists(r2 => r2.item_id == r.item_id)).ToList();

        for (int i = 0; i < p_item_list.Count; i++)
            inventory_Item_List[i].Init(p_item_list[i]);

        for(int i = 0; i < DataManager.Ins.sell_item_box.Count; i++)
        {
            sell_Item_List[i].Init(DataManager.Ins.sell_item_box[i]);
        }

        gameObject.SetActive(true);
    }

    public void ClickInventoryItem(ItemCtr _itemCtr)
    {
        if (_itemCtr.p_item != null && _itemCtr.p_item.item_id > 0)
        {
            p_item p_item = DataManager.Ins.p_item_list.SingleOrDefault(r => r.item_id == _itemCtr.p_item.item_id);

            if (DataManager.Ins.sell_item_box.Exists(r => r.item_id == p_item.item_id))
            {
                p_item sell_item = DataManager.Ins.sell_item_box.SingleOrDefault(r => r.item_id == p_item.item_id);

                sell_item.item_amount += p_item.item_amount;
            }
            else
                DataManager.Ins.sell_item_box.Add(p_item);

            DataManager.Ins.p_item_list.Remove(p_item);

            SoundManager.Ins.PlaySound(SoundManager.click_Sound);
            Init();
        }
    }

    public void ClickSellItem(ItemCtr _itemCtr)
    {
        if(_itemCtr.p_item != null && _itemCtr.p_item.item_id > 0)
        {
            p_item sell_item = DataManager.Ins.sell_item_box.SingleOrDefault(r => r.item_id == _itemCtr.p_item.item_id);

            if (DataManager.Ins.p_item_list.Exists(r => r.item_id == sell_item.item_id))
            {
                p_item p_item = DataManager.Ins.p_item_list.SingleOrDefault(r => r.item_id == sell_item.item_id);

                p_item.item_amount += sell_item.item_amount;
            }
            else
                DataManager.Ins.p_item_list.Add(sell_item);

            DataManager.Ins.sell_item_box.Remove(sell_item);

            SoundManager.Ins.PlaySound(SoundManager.click_Sound);
            Init();
        }
    }

    public void Close()
    {
        SoundManager.Ins.PlaySound(SoundManager.click_Sound);
        close_Action?.Invoke();
        gameObject.SetActive(false);
    }
}
