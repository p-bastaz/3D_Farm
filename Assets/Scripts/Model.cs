using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Model
{
    #region 레퍼런스 데이터

    // 작물 레퍼런스 데이터
    [System.Serializable]
    public class r_crop
    {
        public int crop_id; //작물 고유 ID
        public string crop_name; //작물 이름
        public int crop_growth; //작물 필요 성장 수치
        public int crop_amount; //작물 수확량
        public bool crop_regrow; //싹으로 돌아가 다시 재배가 가능한지
    }

    [System.Serializable]
    public class r_item
    {
        public int item_id;
        public int item_type; // 1: 재화, 2: 씨앗, 3: 작물
        public string item_name;
        public int item_price;
    }

    // 상점 품목
    [System.Serializable]
    public class r_shop
    {
        public int shop_id;
        public int shop_category;   // 1: 씨앗상점, 2: 닭 상점
        public string shop_productName;
        public int shop_productItem;
        public int shop_price;
    }

    // 경작지
    public class r_dirt
    {
        public int dirt_max;
    }

    // 닭장
    public class r_coop
    {
        public int coop_max;
        public int feed_material;
        public int feed_amount;
    }

    #endregion

    #region 플레이어 데이터

    // 플레이어 데이터 (N일차, 체력, 최대 체력)
    [System.Serializable]
    public class p_data
    {
        public int day;
        public int current_HP;
        public int max_HP;
    }


        // 플레이어 아이템
    [System.Serializable]
    public class p_item
    {
        public int item_id;
        public int item_amount;
    }

    // 플레이어 밭
    [System.Serializable]
    public class p_dirt
    {
        public int dirt_id;
        public int dirt_crop_id;
        public int dirt_crop_growth;
        public bool dirt_moisture;
    }

    [System.Serializable]
    public class p_chicken
    {
        public int chicken_id; //닭 고유 ID
        public bool egg;       //달걀을 낳았는지 낳지 않았는지
    }


    #endregion
}
