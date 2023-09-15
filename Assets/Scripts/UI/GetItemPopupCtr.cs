using Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

public class GetItemPopupCtr : MonoBehaviour
{
    r_item r_item;

    [SerializeField] SpriteAtlas atlas;

    [SerializeField] Image item_image;
    [SerializeField] Text item_amount;

    [SerializeField] Text get_text;

    public void Init(r_item _r_item, int _amount)
    {
        r_item = _r_item;

        item_image.sprite = atlas.GetSprite(r_item.item_id.ToString());
        item_amount.text = _amount.ToString();

        get_text.text = "<color=#73CD4B>" + r_item.item_name + "</color>" + "\n 획득하였습니다.";

        gameObject.SetActive(true);
    }

    public void ClickConfirm()
    {
        gameObject.SetActive(false);
    }
}
