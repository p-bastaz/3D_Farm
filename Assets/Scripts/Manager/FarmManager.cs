using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarmManager : MonoBehaviour
{
      //private static FarmManager Instance;
      //public static FarmManager Ins
      //{
      //    get
      //    {
      //        return Instance;
      //    }
      //}
      //
      //
      //private void Awake()
      //{
      //    if (Instance == null || Instance != this)
      //        Instance = this;
      //}

    [Header("player")]
    [SerializeField] private PlayerCtr playerCtr;
    [SerializeField] GameObject mainCamera;

    [Header("Dirts")] // 경작지 오브젝트들
    public GameObject dirts_Camera_Obj;
    [SerializeField] GameObject dirts_Obj;

    [SerializeField] private DirtCtr dirt_prefab;
    [SerializeField] private List<DirtCtr> dirts_list;


    [Header("Farming")] // 경작지 농사
    public bool farming = false;
    [SerializeField] GameObject farming_Camera_Obj;
    [SerializeField] GameObject farming_Obj;
    [SerializeField] public DirtCtr farming_dirt;


    [Header("Coop")] // 닭장 사육
    [SerializeField] GameObject coop_Camera_Obj;
    [SerializeField] List<ChickenCtr> chickenCtr_list;

    void Start()
    {
        //경작지 초기화
        dirts_list = new List<DirtCtr>(); 
        for (int j = 0; j < 3; j++)
            for (int i = 1; i < 10; i++)
            { 
                GameObject obj = dirt_prefab.gameObject;
                obj = Instantiate(dirt_prefab.gameObject, new Vector3(dirt_prefab.transform.position.x + 2.5f * i, 0.6f, dirt_prefab.gameObject.transform.position.z - 2.5f * j), Quaternion.identity);
                obj.gameObject.name = ((j * 9) + i).ToString() + "번째 경작지";
                obj.transform.parent = gameObject.transform.Find("Dirts");
                obj.gameObject.SetActive(true);

                dirts_list.Add(obj.GetComponent<DirtCtr>());
                
            }

        UpdateDirts();

        dirts_Camera_Obj.SetActive(false);



        //닭 초기화
        for(int i = 0; i < chickenCtr_list.Count; i++)
        {
            chickenCtr_list[i].gameObject.SetActive(false);
        }

        for(int i = 0; i < DataManager.Ins.p_chicken_list.Count; i++)
        {
            chickenCtr_list[i].gameObject.SetActive(true);
        }


        playerCtr = GameObject.Find("Player").GetComponent<PlayerCtr>();

        //이전 씬에 따라 플레이어 위치 변경
        if (GameSceneManager.beforeScene == "TownScene")
        {
            playerCtr.transform.localEulerAngles = new Vector3(0, 90, 0);
            playerCtr.transform.localPosition = new Vector3(-15, (float)0.562, (float)-1.8);
        }
       
    }


    public void TurnDirtsCamera(bool _toggle)
    {
        UIManager.Ins.TextBox(!_toggle, "Space : 농사짓기");
        playerCtr.inputLock = _toggle;
        mainCamera.SetActive(!_toggle);
        dirts_Camera_Obj.SetActive(_toggle);
        UIManager.Ins.playerUICtr.SetActive(!_toggle);

        if (_toggle)
            UIManager.Ins.farmLandUICtr.Init();
        else
            UIManager.Ins.farmLandUICtr.gameObject.SetActive(false);
    }

    public void TurnFarmingCamera(bool _toggle)
    {
        farming = _toggle;
        dirts_Camera_Obj.SetActive(!_toggle);
        dirts_Obj.SetActive(!_toggle);
        farming_Camera_Obj.SetActive(_toggle);
        farming_Obj.SetActive(_toggle);

        if (_toggle)
            UIManager.Ins.farmLandUICtr.gameObject.SetActive(false);
        else
            UIManager.Ins.farmLandUICtr.Init();
    }

    public void TurnCoopCamera(bool _toggle)
    {
        UIManager.Ins.TextBox(!_toggle, "Space :  닭 사육하기");
        playerCtr.inputLock = _toggle;
        mainCamera.SetActive(!_toggle);
        coop_Camera_Obj.SetActive(_toggle);
        UIManager.Ins.playerUICtr.SetActive(!_toggle);
        if (_toggle)
            UIManager.Ins.coopCtr.Init();
        else
            UIManager.Ins.coopCtr.gameObject.SetActive(false);
    }

    public void UpdateDirts()
    {
        for (int i = 0; i < dirts_list.Count; i++)
            dirts_list[i].Init(DataManager.Ins.p_dirt_list[i]);
    }
}
