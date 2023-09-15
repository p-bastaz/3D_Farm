using Model;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class DirtPopupCtr : MonoBehaviour
{
    FarmManager farmManager;

    DirtCtr dirtCtr; //경작지 오브젝트 스크립트
    p_dirt p_dirt;
    r_crop r_crop;

    [SerializeField] Button left_Button;
    [SerializeField] Button right_Button;

    [SerializeField] Text dirt_name_txt; //경작지 이름 (-번째 경작지)
    [SerializeField] Text crop_name_txt; //작물 이름
    [SerializeField] GameObject deleteButton;   //삭제 버튼

    [Header("Management")]
    [SerializeField] GameObject management_obj; //관리 오브젝트
    [SerializeField] Image growth_progress;  //성장 진행도
    [SerializeField] Text state_text;    //상태 텍스트
    [SerializeField] GameObject waterButton;    //물주기 버튼
    [SerializeField] GameObject getButton;    //수확 버튼

    [Header("Seed")]
    [SerializeField] GameObject seed_Obj;       //씨앗 인벤토리 오브젝트
    [SerializeField] List<ItemCtr> item_ctr_list;   //씨앗 슬롯

    private void Awake()
    {
        left_Button.onClick.AddListener(() => ClickArrow(left_Button.gameObject));
        right_Button.onClick.AddListener(() => ClickArrow(right_Button.gameObject));

        for (int i = 0; i < item_ctr_list.Count; i++)
        {
            //버튼 클릭 이벤트 할당
            int temp = i;
            Button tempButton = item_ctr_list[temp].GetComponent<Button>();
            tempButton.onClick.AddListener(() => ClickSeedItem(item_ctr_list[temp].p_item));
        }
    }

    // 초기화
    public void Init(p_dirt _p_dirt)
    {
        if (farmManager == null)
        {
            farmManager = GameObject.Find("FarmManager").GetComponent<FarmManager>();
            dirtCtr = farmManager.transform.Find("FarmingDirt").GetComponent<DirtCtr>();
        }

        p_dirt = _p_dirt;
        dirtCtr.Init(p_dirt);

        //경작지 이름 부여
        dirt_name_txt.text = p_dirt.dirt_id.ToString() + "번째 경작지";

        if(p_dirt.dirt_crop_id != 0) // 작물이 심어져 있을 시
        {
            deleteButton.SetActive(true);
            management_obj.SetActive(true);
            seed_Obj.SetActive(false);

            r_crop = DataManager.Ins.data_r_crop_list.SingleOrDefault(r => r.crop_id == p_dirt.dirt_crop_id);
            crop_name_txt.text = r_crop.crop_name;

            growth_progress.fillAmount = (float)p_dirt.dirt_crop_growth / (float)r_crop.crop_growth;

            if(p_dirt.dirt_crop_growth < r_crop.crop_growth)
            {
                getButton.SetActive(false);
                state_text.text = p_dirt.dirt_moisture ? "수분 충분" : "수분 부족";
                state_text.color = p_dirt.dirt_moisture ? new Color32(67, 130, 187, 255) : new Color32(255, 60, 60, 255);
            }
            else
            {
                getButton.SetActive(true);
                deleteButton.SetActive(false);
                state_text.text = "성장 완료";
                state_text.color = new Color32(60, 155, 60, 255);
            }
            
        }
        else
        {
            deleteButton.SetActive(false);
            management_obj.SetActive(false);
            seed_Obj.SetActive(true);

            crop_name_txt.text = "작물 없음";

            //씨앗 패널 초기화
            List<r_item> data_r_item_list = DataManager.Ins.data_r_item_list.Where(r => r.item_type == 2).ToList();
            List<p_item> p_item_list = DataManager.Ins.p_item_list.Where(r => data_r_item_list.Exists(r2 => r2.item_id == r.item_id)).ToList();

            for (int i = 0; i < item_ctr_list.Count; i++)
            {
                if (i < p_item_list.Count)
                    item_ctr_list[i].Init(p_item_list[i]);
                else
                    item_ctr_list[i].Init();

                //버튼 클릭 이벤트 할당
                //int temp = i;
                //Button tempButton = item_ctr_list[temp].GetComponent<Button>();
                //tempButton.onClick.AddListener(() => ClickSeedItem(item_ctr_list[temp].p_item));
            }
        }

        

        gameObject.SetActive(true);
    }

    public void Close()
    {
        SoundManager.Ins.PlaySound(SoundManager.click_Sound);
        farmManager.UpdateDirts();
        farmManager.TurnFarmingCamera(false);
        gameObject.SetActive(false);
    }

    public void ClickSeedItem(p_item p_item)
    {
        SoundManager.Ins.PlaySound(SoundManager.click_Sound);

        if (p_item.item_amount <= 0)
            return;

        r_item r_item = DataManager.Ins.data_r_item_list.SingleOrDefault(r => r.item_id == p_item.item_id);

        UIManager.Ins.yesOrNoPopupCtr.Init("<color=#E86A17>" + r_item.item_name + "</color>" + "\n 를 심으시겠습니까?",
           () =>
           {
               DataManager.Ins.ItemUpdate(r_item, -1);

               p_dirt.dirt_crop_id = r_item.item_id + 10000;
               p_dirt.dirt_moisture = false;
               p_dirt.dirt_crop_growth = 0;

               SoundManager.Ins.PlaySound(SoundManager.hoe_Sound);

               Init(p_dirt);
           }
           );

    }


    public void ClickWaterButton()
    {
        SoundManager.Ins.PlaySound(SoundManager.water_Sound);


        p_dirt.dirt_moisture = true;
        state_text.text = p_dirt.dirt_moisture ? "충분" : "부족";
        state_text.color = p_dirt.dirt_moisture ? new Color32(67, 130, 187, 255) : new Color32(255, 60, 60, 255);

        Init(p_dirt);
    }

    public void ClickGetButton()
    {
        SoundManager.Ins.PlaySound(SoundManager.harvest_Sound);

        r_item r_item = DataManager.Ins.data_r_item_list.SingleOrDefault(r => r.item_id == r_crop.crop_id);
        DataManager.Ins.ItemUpdate(r_item, r_crop.crop_amount);

        UIManager.Ins.getItemPopupCtr.Init(r_item, r_crop.crop_amount);

        if(!r_crop.crop_regrow)
        {
            p_dirt.dirt_crop_id = 0;
            p_dirt.dirt_moisture = false;
            p_dirt.dirt_crop_growth = 0;
        }
        else
        {
            p_dirt.dirt_moisture = false;
            p_dirt.dirt_crop_growth = (int)(p_dirt.dirt_crop_growth / 2);
        }

        Init(p_dirt);
    }

    public void ClickDeleteButton()
    {
        SoundManager.Ins.PlaySound(SoundManager.click_Sound);

        UIManager.Ins.yesOrNoPopupCtr.Init("경작지를 초기화 하시겠습니까?\n심은 씨앗은 돌려받을 수 없습니다",
            () => 
            {
                p_dirt.dirt_crop_id = 0;
                p_dirt.dirt_moisture = false;
                p_dirt.dirt_crop_growth = 0;
                Init(p_dirt);
            }
            );
    }

    public void ClickArrow(GameObject go)
    {
        SoundManager.Ins.PlaySound(SoundManager.click_Sound);

        int dirt_id = p_dirt.dirt_id;

        if(go == left_Button.gameObject)
        {
            dirt_id--;

            if (dirt_id == 0)
            {
                dirt_id = 1;
                return;
            }
        }
        else
        {
            dirt_id++;

            if (dirt_id > DataManager.Ins.data_r_dirt.dirt_max)
                dirt_id = DataManager.Ins.data_r_dirt.dirt_max;
        }

        p_dirt _p_dirt = DataManager.Ins.p_dirt_list.SingleOrDefault(r => r.dirt_id == dirt_id);

        Init(_p_dirt);
    }
}
