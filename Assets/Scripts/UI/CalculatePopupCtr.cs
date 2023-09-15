using Model;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class CalculatePopupCtr : MonoBehaviour
{
    [SerializeField] Text day_Title;
    [SerializeField] GameObject non;
    [SerializeField] CalculateItemCtr calculateItemCtr;
    [SerializeField] GameObject scrollView_Contents;
    [SerializeField] Text total_Money;

    public void Init()
    {

        #region 납품 작물 표시 및 정산

        for (int i = 0; i < scrollView_Contents.transform.childCount; i++)
            scrollView_Contents.transform.GetChild(i).gameObject.SetActive(false);

        List<p_item> sell_item_list = DataManager.Ins.sell_item_box;

        int total = 0;

        if (sell_item_list.Count == 0)
        {
            non.SetActive(true);
            total_Money.text = "0";
        }
        else
        {
            non.SetActive(false);

            //납품 작물 표시
            for(int i = 0; i < sell_item_list.Count; i++)
            {
                CalculateItemCtr temp_calculateItemCtr;

                if (i + 1 > scrollView_Contents.transform.childCount)
                {
                    GameObject temp = Instantiate(calculateItemCtr.gameObject);
                    temp.transform.SetParent(scrollView_Contents.transform);
                    //temp.GetComponent<RectTransform>().localScale = Vector3.one;
                    temp.transform.localScale = Vector3.one;
                    temp_calculateItemCtr = temp.GetComponent<CalculateItemCtr>();
                }
                else
                    temp_calculateItemCtr = scrollView_Contents.transform.GetChild(i).GetComponent<CalculateItemCtr>();

                temp_calculateItemCtr.Init(sell_item_list[i]);
                temp_calculateItemCtr.gameObject.SetActive(true);

                r_item r_item = DataManager.Ins.data_r_item_list.SingleOrDefault(r => r.item_id == sell_item_list[i].item_id);

                total += ((r_item.item_price) * (sell_item_list[i].item_amount));
            }

            DataManager.Ins.ItemUpdate(DataManager.Ins.data_r_item_list.SingleOrDefault(r => r.item_id == 10001), total);
            total_Money.text = total.ToString();

            DataManager.Ins.sell_item_box.Clear();

        }

        #endregion

        day_Title.text = DataManager.Ins.player_data.day.ToString() + "일차 정산";

        DataManager.Ins.DayEnd();

        gameObject.SetActive(true);
    }

    public void SaveAndClose()
    {
        SoundManager.Ins.PlaySound(SoundManager.click_Sound);
        DataManager.Ins.Save();
        gameObject.SetActive(false);

        if (GameObject.Find("Player") != null)
        {
            PlayerCtr playerCtr = GameObject.Find("Player").GetComponent<PlayerCtr>();
            playerCtr.inputLock = true;

            playerCtr.transform.localEulerAngles = new Vector3(0, 180, 0);
            playerCtr.transform.localPosition = new Vector3(1.11f, 0.5f, 2.51f);

            SoundManager.Ins.PlayBGM(SoundManager.farm_BGM);
            UIManager.Ins.fadeCtr.FadeOut(() => { playerCtr.inputLock = false; });

        }
    }
}

