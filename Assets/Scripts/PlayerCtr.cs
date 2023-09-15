using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCtr : MonoBehaviour
{
    //[SerializeField] private FarmManager farmController;
    [SerializeField] private Animator anim;

    public FarmManager farmManager;
    public HouseManager houseManager;
    public TownManager townManager;

    public bool inputLock = false; // 키 입력 잠금

    void Start()
    {
        //farmController = GameObject.Find("GameController").GetComponent<FarmManager>();
        //anim = transform.GetComponent<Animator>();


        //현재 씬 상태에 따라 씬 별 매니저 파싱하기
        switch(GameSceneManager.currentScene)
        {
            case "HouseScene":
                houseManager = GameObject.Find("HouseManager").GetComponent<HouseManager>();
                break;
            case "FarmScene":
                farmManager = GameObject.Find("FarmManager").GetComponent<FarmManager>();
                break;
            case "TownScene":
                townManager = GameObject.Find("TownManager").GetComponent<TownManager>();
                break;
        }
    }

    public float speed;
    float hAxis;
    float vAxis;

    Vector3 moveVec;

    void Update()
    {
        //2023-08-17 이전의 이동 스크립트
        //if(!fixation)
        //{
        //    anim.SetFloat("Speed", Mathf.Abs(Input.GetAxis("Vertical")));
        //    transform.Translate(0, 0, 0.025f * Input.GetAxis("Vertical"));
        //    transform.Rotate(0, 0.5f * Input.GetAxis("Horizontal"), 0);
        //}

        if(!inputLock)
        {
            hAxis = Input.GetAxisRaw("Horizontal");
            vAxis = Input.GetAxisRaw("Vertical");

            moveVec = new Vector3(hAxis, 0, vAxis).normalized;

            transform.position += moveVec * speed * Time.deltaTime;

            anim.SetBool("IsWalk", moveVec != Vector3.zero);

            transform.LookAt(transform.position + moveVec);
        }
    }


    private void OnTriggerEnter(Collider collider)
    {
        if (!UIManager.Ins.obj_text_box.activeSelf)
        {
            switch (collider.gameObject.name)
            {
                case "BedArea":
                        UIManager.Ins.TextBox(true, "Space : 잠자기");
                    break;
                case "HouseDoorArea":
                        UIManager.Ins.TextBox(true, GameSceneManager.currentScene == "HouseScene" ? "Space : 나가기" : "Space : 들어가기");
                    break;
                case "FieldFarmArea":
                        UIManager.Ins.TextBox(true, "Space : 농사짓기");
                    break;
                case "TownArea":
                        UIManager.Ins.TextBox(true, GameSceneManager.currentScene == "FarmScene" ? "Space : 마을로 가기" : "Space : 농장으로 가기");
                    break;
                case "ChicShopArea":
                case "SeedShopArea":
                        UIManager.Ins.TextBox(true, "Space : 대화하기");
                    break;
                case "SellBoxArea":
                        UIManager.Ins.TextBox(true, "Space : 납품하기");
                    break;
                case "CoopArea":
                        UIManager.Ins.TextBox(true, "Space : 닭 사육하기");
                    break;
            }
        }
    }

    private void OnTriggerStay(Collider collider)
    {
        if(!inputLock)
        {
            switch (collider.gameObject.name)
            {
                case "HouseDoorArea":       //집 <-> 농장
                    if (Input.GetKey("space"))
                    {
                        SoundManager.Ins.PlaySound(SoundManager.doorOpen_Sound);
                        GameSceneManager.Ins.LoadScene(GameSceneManager.currentScene == "HouseScene" ? "FarmScene" : "HouseScene");
                        UIManager.Ins.TextBox(false);
                    }
                    break;
                case "TownArea":       //마을 <-> 농장
                    if (Input.GetKey("space"))
                    {
                        GameSceneManager.Ins.LoadScene(GameSceneManager.currentScene == "FarmScene" ? "TownScene" : "FarmScene");
                        UIManager.Ins.TextBox(false);
                    }
                    break;
                case "FieldFarmArea":       //농사짓기
                    if (!farmManager.farming && Input.GetKey("space"))
                    {
                        farmManager.TurnDirtsCamera(true);
                        UIManager.Ins.inventoryPopupCtr.Close();
                    }
                    break;
                case "CoopArea":       //닭 사육하기
                    if (Input.GetKey("space"))
                    {
                        farmManager.TurnCoopCamera(true);
                        UIManager.Ins.inventoryPopupCtr.Close();
                    }
                    break;
                case "SeedShopArea":       //씨앗 상점
                    if (Input.GetKey("space"))
                    {
                        UIManager.Ins.inventoryPopupCtr.Close();
                        townManager.TurnSeedShop(true);

                    }
                    break;
                case "ChicShopArea":       //닭 상점
                    if (Input.GetKey("space"))
                    {
                        UIManager.Ins.inventoryPopupCtr.Close();
                        townManager.TurnChicShop(true);

                    }
                    break;
                case "BedArea":             //잠자기
                    if (Input.GetKey("space"))
                    {
                        inputLock = true;
                        UIManager.Ins.TextBox(false);
                        UIManager.Ins.inventoryPopupCtr.Close();
                        UIManager.Ins.yesOrNoPopupCtr.Init("잠을 자시겠습니까?\n하루가 흘러갑니다",
                            () => 
                            {
                                SoundManager.Ins.StopBGM();
                                UIManager.Ins.fadeCtr.FadeIn(UIManager.Ins.calculatePopupCtr.Init);
                            },
                            () =>
                            {
                                inputLock = false;
                                UIManager.Ins.TextBox(true, "Space : 잠자기");
                            }
                        );
                    }
                    break;  
                case "SellBoxArea":     // 납품하기
                    if (Input.GetKey("space"))
                    {
                        inputLock = true;
                        UIManager.Ins.TextBox(false);
                        UIManager.Ins.inventoryPopupCtr.Close();
                        SoundManager.Ins.PlaySound(SoundManager.openChest_Sound);
                        UIManager.Ins.sellPopupCtr.Init(() => { 
                            inputLock = false;
                            UIManager.Ins.TextBox(true, "Space : 납품하기");
                        });
                    }
                    break;
            }
        }
       
    }

    private void OnTriggerExit(Collider collider)
    {
       
        if (UIManager.Ins.obj_text_box.activeSelf)
            UIManager.Ins.TextBox(false);

    }
}
