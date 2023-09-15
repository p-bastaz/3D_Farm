using Model;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DirtCtr : MonoBehaviour
{
    public r_crop r_crop;
    public p_dirt p_dirt;

    public FarmManager farmManager;
    [SerializeField] private PointerEventData ped;

    public Camera fieldFarmingCamera;

    [SerializeField] GameObject crops;
    private GameObject crop_Obj;

    [SerializeField] MeshRenderer dirt_Material;
    [SerializeField] List<Material> material_list;


    void Start()
    {
        farmManager = GameObject.Find("FarmManager").GetComponent<FarmManager>();

        //gr = uiController.GetComponent<GraphicRaycaster>();
        ped = new PointerEventData(null);

        fieldFarmingCamera = farmManager.dirts_Camera_Obj.GetComponent<Camera>();

    }

    // Update is called once per frame
    void Update()
    {
        if(fieldFarmingCamera.isActiveAndEnabled) //경작지 오브젝트 클릭
        {
            var ray = fieldFarmingCamera.ScreenPointToRay(Input.mousePosition);
                //Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit Hit;

            ped.position = Input.mousePosition;
            List<RaycastResult> results = new List<RaycastResult>();
            //gr.Raycast(ped, results);

            if (Input.GetMouseButtonDown(0))
            {
               
                if (Physics.Raycast(ray, out Hit) && Hit.collider.gameObject == gameObject && results.Count == 0)
                {
                    SoundManager.Ins.PlaySound(SoundManager.click_Sound);
                    UIManager.Ins.dirtPopupCtr.Init(p_dirt);
                    farmManager.TurnFarmingCamera(true);
                }
            }
        }
    }

    public void Init(p_dirt _p_dirt) // 작물 초기화
    {
        p_dirt = _p_dirt;

        for (int i = 0; i < crops.transform.childCount; i++)
            crops.transform.GetChild(i).gameObject.SetActive(false);

        dirt_Material.material = material_list[0];

        if (p_dirt.dirt_crop_id != 0)
        {
            r_crop = DataManager.Ins.data_r_crop_list.SingleOrDefault(r => r.crop_id == p_dirt.dirt_crop_id);
            
            crop_Obj = crops.transform.Find(r_crop.crop_id.ToString()).gameObject;
            crop_Obj.SetActive(true);    

                for (int i = 0; i < crop_Obj.transform.childCount; i++)
                    crop_Obj.transform.GetChild(i).gameObject.SetActive(false);

            int reference_point = r_crop.crop_growth / 2;

                //성장도에 따라 싹 or 열매 표시

            if(p_dirt.dirt_crop_growth == r_crop.crop_growth)
                crop_Obj.transform.Find("2").gameObject.SetActive(true);
            else
            {
                if(p_dirt.dirt_crop_growth < reference_point)
                    crop_Obj.transform.Find("0").gameObject.SetActive(true);
                else
                    crop_Obj.transform.Find("1").gameObject.SetActive(true);
            }

            //수분 상태에 따라 땅이 젖었는지 안젖었는지 마테리얼 교체
            if (p_dirt.dirt_moisture)
                dirt_Material.material = material_list[1];
            else
                dirt_Material.material = material_list[0];
        }

    }

}
