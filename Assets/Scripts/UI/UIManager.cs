using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager: MonoBehaviour
{
    private static UIManager Instance;
    public static UIManager Ins
    {
        get
        {
            return Instance;
        }
    }

    public List<Action> close_action_list;

    [Header("Fade")]
    public FadeCtr fadeCtr;

    [Header("TextBox")]
    public GameObject obj_text_box;
    [SerializeField] Text label_text_box;

    [Header("MessagePopup")]
    public MessagePopupCtr messagePopupCtr;

    [Header("YesOrNoPopup")]
    public YesOrNoPopupCtr yesOrNoPopupCtr;

    [Header("PlayerUI")]
    public PlayerUICtr playerUICtr;

    [Header("FarmLandUICtr")]
    public FarmLandUICtr farmLandUICtr;
    
    [Header("DirtPopup")]
    public DirtPopupCtr dirtPopupCtr;

    [Header("ShopPopup")]
    public ShopPopupCtr shopPopupCtr;

    [Header("BuyPopup")]
    public BuyPopupCtr buyPopupCtr;

    [Header("InventoryPopup")]
    public InventoryPopupCtr inventoryPopupCtr;

    [Header("CalculatePopup")]
    public CalculatePopupCtr calculatePopupCtr;

    [Header("SellPopup")]
    public SellPopupCtr sellPopupCtr;

    [Header("GetItemPopupCtr")]
    public GetItemPopupCtr getItemPopupCtr;

    [Header("CoopCtr")]
    public CoopCtr coopCtr;

    [Header("MakeFeedPopup")]
    public MakeFeedPopupCtr makeFeedPopupCtr;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            if (Instance != this)
            {
                Destroy(gameObject);
            }
        }
    }

    public void TextBox(bool Active, string text = "")
    {
        obj_text_box.SetActive(Active);
        label_text_box.text = text;
    }

}
