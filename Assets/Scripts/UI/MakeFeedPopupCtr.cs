using Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class MakeFeedPopupCtr : MonoBehaviour
{
    [SerializeField] Text material_Amount;
    [SerializeField] Text feed_Amount;

    [SerializeField] Button plusButton;
    [SerializeField] Button minusButton;

    int amount = 1;
    [SerializeField] Text amount_text;
    [SerializeField] GameObject make_Button;

    r_coop r_coop;

    Action update_Action;


    public void Init(Action _action)
    {
        if (r_coop == null)
            r_coop = DataManager.Ins.data_r_coop;

        update_Action = _action;

        amount = 1;
        amount_text.text = amount.ToString();

        MaterialCheck();

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
        amount_text.text = amount.ToString();

        material_Amount.text = amount.ToString();
        feed_Amount.text = (r_coop.feed_amount * amount).ToString();

        MaterialCheck();
    }

    private void MaterialCheck()
    {
        int material_Amount = DataManager.Ins.p_item_list.FirstOrDefault(r => r.item_id == r_coop.feed_material) != null ? DataManager.Ins.p_item_list.FirstOrDefault(r => r.item_id == r_coop.feed_material).item_amount : 0;

        if (amount > material_Amount)
            amount_text.color = Color.red;
        else
            amount_text.color = new Color32(50, 50, 50, 255);

    }

    public void ClickMake()
    {
        SoundManager.Ins.PlaySound(SoundManager.click_Sound);

        int material_Amount = DataManager.Ins.p_item_list.FirstOrDefault(r => r.item_id == r_coop.feed_material) != null ? DataManager.Ins.p_item_list.FirstOrDefault(r => r.item_id == r_coop.feed_material).item_amount : 0;

        if (amount <= material_Amount)
        {
            r_item material_item = DataManager.Ins.data_r_item_list.SingleOrDefault(r => r.item_id == r_coop.feed_material);
            r_item feed_item = DataManager.Ins.data_r_item_list.SingleOrDefault(r => r.item_id == 10002);

            DataManager.Ins.ItemUpdate(material_item, (amount * -1));
            DataManager.Ins.ItemUpdate(feed_item, (amount * r_coop.feed_amount));
            update_Action();
            gameObject.SetActive(false);

            UIManager.Ins.getItemPopupCtr.Init(feed_item, (amount * r_coop.feed_amount));
        }
    }

    public void Close()
    {
        SoundManager.Ins.PlaySound(SoundManager.click_Sound);
        gameObject.SetActive(false);
    }

}
