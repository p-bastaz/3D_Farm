using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUICtr : MonoBehaviour
{
    public Text day;
    public Text money;


    private void Update()
    {
        UIUpdate();
    }

    public void SetActive(bool _toggle)
    {
        gameObject.SetActive(_toggle);
    }

    public void UIUpdate()
    {
        day.text = DataManager.Ins.player_data.day.ToString() + "일 차";
        money.text = DataManager.Ins.p_item_list.SingleOrDefault(r => r.item_id == 10001).item_amount.ToString();
    }

    public void ClickInventory()
    {
        SoundManager.Ins.PlaySound(SoundManager.click_Sound);
        UIManager.Ins.inventoryPopupCtr.Init();
    }
}
