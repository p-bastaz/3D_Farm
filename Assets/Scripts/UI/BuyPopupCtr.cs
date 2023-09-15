using Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class BuyPopupCtr : MonoBehaviour
{
    [SerializeField] ItemCtr itemCtr;

    int amount = 1;
    [SerializeField] Text amount_label;

    [SerializeField] Button plusButton;
    [SerializeField] Button minusButton;

    [SerializeField] GameObject buy_Button;
    [SerializeField] Text price_label;

    r_shop r_shop;

    Action update_Action;

    private void Awake()
    {
        plusButton.onClick.AddListener(() => ClickPlusMinus(plusButton.gameObject));
        minusButton.onClick.AddListener(() => ClickPlusMinus(minusButton.gameObject));
    }

    public void Init(r_shop _r_shop, Action _action)
    {
        r_shop = _r_shop;
        update_Action = _action;

        itemCtr.Init(r_shop.shop_productItem);

        amount = 1;
        amount_label.text = amount.ToString();

        price_label.text = r_shop.shop_price.ToString();

        PriceCheck();

        gameObject.SetActive(true);
    }


    public void ClickPlusMinus(GameObject go)
    {
        SoundManager.Ins.PlaySound(SoundManager.click_Sound);

        if (go == plusButton.gameObject)
            amount++;
        else
        {
            amount--;

            if (amount <= 0)
                amount = 1;
        }
        amount_label.text = amount.ToString();

        price_label.text = (r_shop.shop_price * amount).ToString();

        PriceCheck();
    }

    private void PriceCheck()
    {
        int money = DataManager.Ins.p_item_list.FirstOrDefault(r => r.item_id == 10001).item_amount;

        if ((r_shop.shop_price * amount) > money)
            price_label.color = Color.red;
        else
            price_label.color = new Color32(50, 50, 50, 255);

    }

    public void ClickBuy()
    {
        SoundManager.Ins.PlaySound(SoundManager.click_Sound);

        int money = DataManager.Ins.p_item_list.FirstOrDefault(r => r.item_id == 10001).item_amount;

        if ((r_shop.shop_price * amount) <= money)
        {
            DataManager.Ins.Buy(r_shop, amount);
            update_Action();
            gameObject.SetActive(false);
        }
    }

    public void Close()
    {
        SoundManager.Ins.PlaySound(SoundManager.click_Sound);
        gameObject.SetActive(false);
    }
}
