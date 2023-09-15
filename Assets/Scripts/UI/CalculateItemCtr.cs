using Model;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class CalculateItemCtr : MonoBehaviour
{
    [SerializeField] ItemCtr itemCtr;
    [SerializeField] Text amount;
    [SerializeField] Text price;

    public void Init(p_item _p_item)
    {
        r_item r_item = DataManager.Ins.data_r_item_list.SingleOrDefault(r => r.item_id == _p_item.item_id);

        itemCtr.Init(_p_item.item_id);

        amount.text = "x " + _p_item.item_amount.ToString();

        price.text = (r_item.item_price * _p_item.item_amount).ToString();
    }
}
