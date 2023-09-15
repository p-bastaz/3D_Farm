using Model;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

public class ItemCtr : MonoBehaviour
{
    [SerializeField] SpriteAtlas atlas;

    [SerializeField] Image item_spr;
    [SerializeField] Text item_amount;

    r_item r_item;
    public p_item p_item;

    public void Init()
    {
        item_spr.gameObject.SetActive(false);
        item_amount.gameObject.SetActive(false);
    }

    public void Init(int _item_id, int _amount = 0)
    {
        item_spr.sprite = atlas.GetSprite(_item_id.ToString());

        item_spr.gameObject.SetActive(true);

        if(_amount == 0)
            item_amount.gameObject.SetActive(false);
        else
        {
            item_amount.gameObject.SetActive(true);
            item_amount.text = _amount.ToString();
        }
    }

    public void Init(p_item _p_item)
    {
        p_item = _p_item;

        item_spr.sprite = atlas.GetSprite(p_item.item_id.ToString());
        item_amount.text = p_item.item_amount.ToString();

        item_spr.gameObject.SetActive(true);
        item_amount.gameObject.SetActive(true);
    }

   
}
