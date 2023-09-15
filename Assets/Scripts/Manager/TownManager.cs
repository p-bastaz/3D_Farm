using Model;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TownManager : MonoBehaviour
{
    [Header("player")]
    [SerializeField] private PlayerCtr playerCtr;
    public GameObject mainCamera;

    [Header("ShopCamera")]
    [SerializeField] GameObject chicShopCamera;
    [SerializeField] GameObject seedShopCamera;

    public void TurnChicShop(bool _toggle)
    {
        UIManager.Ins.TextBox(!_toggle, "Space : 대화하기");
        playerCtr.inputLock = _toggle;
        mainCamera.SetActive(!_toggle);
        chicShopCamera.SetActive(_toggle);

        if (_toggle)
        {
            List<r_shop> _r_shop_list = DataManager.Ins.data_r_shop_list.Where(r => r.shop_category == 2).ToList();
            UIManager.Ins.shopPopupCtr.Init(_r_shop_list);
        }
    }

    public void TurnSeedShop(bool _toggle)
    {
        UIManager.Ins.TextBox(!_toggle, "Space : 대화하기");
        playerCtr.inputLock = _toggle;
        mainCamera.SetActive(!_toggle);
        seedShopCamera.SetActive(_toggle);

        if(_toggle)
        {
            List<r_shop> _r_shop_list = DataManager.Ins.data_r_shop_list.Where(r => r.shop_category == 1).ToList();
            UIManager.Ins.shopPopupCtr.Init(_r_shop_list);
        }
    }
}
