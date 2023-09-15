using Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ShopPopupCtr : MonoBehaviour
{
    TownManager townManager;

    [SerializeField] ShopItemCtr shopItemCtr;
    [SerializeField] GameObject scrollView_Contents;

    [SerializeField] Text money_text;

    int shop_category = 0;


    public void Init(List<r_shop> _r_shop_list)
    {
        if (townManager == null)
            townManager = GameObject.Find("TownManager").GetComponent<TownManager>();

        shop_category = _r_shop_list[0].shop_category;

        for (int i = 0; i < scrollView_Contents.transform.childCount; i++)
            scrollView_Contents.transform.GetChild(i).gameObject.SetActive(false);

        for (int i = 0; i < _r_shop_list.Count; i++)
        {
            ShopItemCtr temp_shopItemCtr;

            if (i + 1 > scrollView_Contents.transform.childCount)
            {
                GameObject temp = Instantiate(shopItemCtr.gameObject);
                temp.transform.SetParent(scrollView_Contents.transform);
                //temp.GetComponent<RectTransform>().localScale = Vector3.one;
                temp.transform.localScale = Vector3.one;
                temp_shopItemCtr = temp.GetComponent<ShopItemCtr>();
            }
            else
                temp_shopItemCtr = scrollView_Contents.transform.GetChild(i).GetComponent<ShopItemCtr>();

            temp_shopItemCtr.Init(_r_shop_list[i], MoneyUpdate);
            temp_shopItemCtr.gameObject.SetActive(true);
        }

        MoneyUpdate();

        gameObject.SetActive(true);

    }

    private void MoneyUpdate()
    {
        money_text.text = DataManager.Ins.p_item_list.FirstOrDefault(r => r.item_id == 10001).item_amount.ToString();
    }

    public void Close()
    {
        switch (shop_category)
        {
            case 1:
                townManager.TurnSeedShop(false);
                break;
            case 2:
                townManager.TurnChicShop(false);
                break;
        }
        SoundManager.Ins.PlaySound(SoundManager.click_Sound);
        gameObject.SetActive(false);
    }
}
