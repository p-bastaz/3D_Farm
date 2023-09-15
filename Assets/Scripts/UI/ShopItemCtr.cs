using Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ShopItemCtr : MonoBehaviour
{
    [SerializeField] ItemCtr item_ctr;
    [SerializeField] Text item_name;
    [SerializeField] Text item_price;

    r_shop r_shop;

    Action update_Action;

    public void Init(r_shop _r_shop, Action _action)
    {
        r_shop = _r_shop;

        item_ctr.Init(_r_shop.shop_productItem);
        item_name.text = _r_shop.shop_productName;
        item_price.text = _r_shop.shop_price.ToString();

        update_Action = _action;
    }

    public void ClickItem()
    {
        SoundManager.Ins.PlaySound(SoundManager.click_Sound);
        switch (r_shop.shop_id)
        {
            case 12001:         // 닭
                {
                    p_item money = DataManager.Ins.p_item_list.SingleOrDefault(r => r.item_id == 10001);

                    if (money.item_amount >= r_shop.shop_price)
                    {
                        UIManager.Ins.yesOrNoPopupCtr.Init("<color=#E86A17>" + r_shop.shop_productName + "</color>" + "\n을 구매하시겠습니까?",
                       () =>
                       {
                           if(DataManager.Ins.p_chicken_list.Count < DataManager.Ins.data_r_coop.coop_max)
                           {
                               int chicken_id = DataManager.Ins.p_chicken_list.Count == 0 ? 1 : DataManager.Ins.p_chicken_list.Max(r => r.chicken_id) + 1;

                               p_chicken p_chicken = new p_chicken();
                               p_chicken.chicken_id = chicken_id;
                               p_chicken.egg = false;

                               DataManager.Ins.p_chicken_list.Add(p_chicken);
                               DataManager.Ins.ItemUpdate(DataManager.Ins.data_r_item_list.SingleOrDefault(r => r.item_id == money.item_id), r_shop.shop_price * -1);

                               SoundManager.Ins.PlaySound(SoundManager.get_Sound);
                               UIManager.Ins.messagePopupCtr.Init("구매 완료했습니다\n구매한 닭은 자동으로 닭장으로 배치됩니다");

                               update_Action?.Invoke();
                           }
                           else
                           {
                               UIManager.Ins.messagePopupCtr.Init("닭장 최대 수용 수를 초과했습니다");
                           }

                       }
                       );
                    }
                }
                break;
            default:
                UIManager.Ins.buyPopupCtr.Init(r_shop, update_Action);
                break;
        }

    }
}
