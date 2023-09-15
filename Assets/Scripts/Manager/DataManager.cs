using Model;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;



[System.Serializable]
public class Serialization<T>
{
    [SerializeField]
    public List<T> data;
    public List<T> ToList() { return data; }

    public Serialization(List<T> data)
    {
        this.data = data;
    }

}


[System.Serializable]
public class SaveData
{
    public p_data p_data = new p_data();
    //p_item
    public List<int> item_id_list = new List<int>();
    public List<int> item_amount_list = new List<int>();
    
    //p_dirt
    public List<int> dirt_id_list = new List<int>();
    public List<int> dirt_crop_list = new List<int>();
    public List<int> dirt_crop_growth_list = new List<int>();
    public List<bool> dirt_moisture_list = new List<bool>();

    //p_chicken
    public List<int> chicken_id_list = new List<int>();
    public List<bool> egg_list = new List<bool>();

}

public class DataManager : MonoBehaviour
{
    private static DataManager Instance;
    public static DataManager Ins
    {
        get
        {
            return Instance;
        }
    }

    public bool initDataLoad = false;
    public bool playerDataLoad = false;

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


    [Header("Reference Data")]
    public r_dirt data_r_dirt = new r_dirt();       //경작지 정보
    public r_coop data_r_coop = new r_coop();       //닭장 정보
    public List<r_crop> data_r_crop_list = new List<r_crop>();  //작물 정보
    public List<r_item> data_r_item_list = new List<r_item>();  //아이템 정보
    public List<r_shop> data_r_shop_list = new List<r_shop>();  //상점 정보


    [Header("Player Data")]
    public p_data player_data = new p_data();      //플레이어 데이터
    public List<p_dirt> p_dirt_list = new List<p_dirt>();   //플레이어 경작지
    public List<p_item> p_item_list = new List<p_item>();   //플레이어 아이템
    public List<p_chicken> p_chicken_list = new List<p_chicken>();  //플레이어 닭
    private SaveData saveData = new SaveData();


    public List<p_item> sell_item_box = new List<p_item>(); //판매용 상자
    public p_item[] coop_feed_box;      //먹이 상자


    private void Start()
    {
        StartCoroutine(LoadInitData());
        StartCoroutine(LoadPlayerData());
    }

    //플레이어 데이터 생성
    private void CreatePlayerData()
    {
        player_data.day = 1;
        player_data.max_HP = 100;
        player_data.current_HP = player_data.max_HP;

        for(int i = 1; i <= data_r_dirt.dirt_max; i++)
        {
            p_dirt p_dirt = new p_dirt();
            p_dirt.dirt_id = i;
            p_dirt.dirt_crop_id = 0;
            p_dirt.dirt_crop_growth = 0;
            p_dirt.dirt_moisture = false;

            p_dirt_list.Add(p_dirt);
        }

        p_item p_item = new p_item();
        p_item.item_id = 10001;
        p_item.item_amount = 500;

        p_item_list.Add(p_item);

        p_item = new p_item();
        p_item.item_id = 10002;
        p_item.item_amount = 0;

        p_item_list.Add(p_item);

        Save();
    }

    //플레이어 저장하기
    public void Save()
    {
        //player_data
        saveData.p_data = player_data;


        //p_item
        saveData.item_id_list.Clear();
        saveData.item_amount_list.Clear();

        for(int i = 0; i < p_item_list.Count; i++)
        {
            saveData.item_id_list.Add(p_item_list[i].item_id);
            saveData.item_amount_list.Add(p_item_list[i].item_amount);
        }


        //p_dirt
        saveData.dirt_id_list.Clear();
        saveData.dirt_crop_list.Clear();
        saveData.dirt_crop_growth_list.Clear();
        saveData.dirt_moisture_list.Clear();

        
        for (int i = 0; i < p_dirt_list.Count; i++)
        {
            saveData.dirt_id_list.Add(p_dirt_list[i].dirt_id);
            saveData.dirt_crop_list.Add(p_dirt_list[i].dirt_crop_id);
            saveData.dirt_crop_growth_list.Add(p_dirt_list[i].dirt_crop_growth);
            saveData.dirt_moisture_list.Add(p_dirt_list[i].dirt_moisture);
        }

        //p_chicken
        saveData.chicken_id_list.Clear();
        saveData.egg_list.Clear();

        for(int i = 0; i < p_chicken_list.Count; i++)
        {
            saveData.chicken_id_list.Add(p_chicken_list[i].chicken_id);
            saveData.egg_list.Add(p_chicken_list[i].egg);
        }

        File.WriteAllText(Application.dataPath + "/SaveFile.json", JsonUtility.ToJson(saveData));
    }




    //레퍼런스 데이터 불러오기
    public IEnumerator LoadInitData()
    {
        string str;

#if UNITY_EDITOR
        //경작지 정보
        if (System.IO.File.Exists(Application.dataPath + "/Resources/Data/r_dirt.json"))
        {
            str = File.ReadAllText(Application.dataPath + "/Resources/Data/r_dirt.json");

            var loadData = JsonUtility.FromJson<r_dirt>(str);

            if (loadData != null)
            {
                data_r_dirt = loadData;
            }
        }

        //닭장 정보
        if (System.IO.File.Exists(Application.dataPath + "/Resources/Data/r_coop.json"))
        {
            str = File.ReadAllText(Application.dataPath + "/Resources/Data/r_coop.json");
            var loadData = JsonUtility.FromJson<r_coop>(str);

            if (loadData != null)
            {
                data_r_coop = loadData;
            }
        }


        //작물 정보
        if (System.IO.File.Exists(Application.dataPath + "/Resources/Data/r_crop.json"))
        {
            str = File.ReadAllText(Application.dataPath + "/Resources/Data/r_crop.json");
            Serialization<r_crop> loadData = JsonUtility.FromJson<Serialization<r_crop>>(str);


            data_r_crop_list.Clear();

            if (loadData != null)
            {
                foreach (r_crop r_crop in loadData.data)
                {
                    data_r_crop_list.Add(r_crop);
                }
            }
        }

        //아이템 정보
        if (System.IO.File.Exists(Application.dataPath + "/Resources/Data/r_item.json"))
        {
            str = File.ReadAllText(Application.dataPath + "/Resources/Data/r_item.json");
            Serialization<r_item> loadData = JsonUtility.FromJson<Serialization<r_item>>(str);

            data_r_item_list.Clear();

            if (loadData != null)
            {
                foreach (r_item r_item in loadData.data)
                {
                    data_r_item_list.Add(r_item);
                }
            }
        }

        //상점 정보
        if (System.IO.File.Exists(Application.dataPath + "/Resources/Data/r_shop.json"))
        {
            str = File.ReadAllText(Application.dataPath + "/Resources/Data/r_shop.json");
            Serialization<r_shop> loadData = JsonUtility.FromJson<Serialization<r_shop>>(str);

            data_r_shop_list.Clear();

            if (loadData != null)
            {
                foreach (r_shop r_shop in loadData.data)
                {
                    data_r_shop_list.Add(r_shop);
                }
            }
        }
#else

        //경작지 정보 (r_dirt)
        if (Resources.Load<TextAsset>("Data/r_dirt") != null)
        {
            str = Resources.Load<TextAsset>("Data/r_dirt").ToString();
            var loadData = JsonUtility.FromJson<r_dirt>(str);

            if (loadData != null)
            {
                data_r_dirt = loadData;
            }
        }

        //닭장 정보 (r_coop)
        if (Resources.Load<TextAsset>("Data/r_coop") != null)
        {
            str = Resources.Load<TextAsset>("Data/r_coop").ToString();
            var loadData = JsonUtility.FromJson<r_coop>(str);

            if (loadData != null)
            {
                data_r_coop = loadData;
            }
        }


        //작물 정보 (r_crop)
        if (Resources.Load<TextAsset>("Data/r_crop") != null)
        {
            str = Resources.Load<TextAsset>("Data/r_crop").ToString();
            Serialization<r_crop> loadData = JsonUtility.FromJson<Serialization<r_crop>>(str);

            data_r_crop_list.Clear();


            if (loadData != null)
            {
                foreach (r_crop r_crop in loadData.data)
                {
                    data_r_crop_list.Add(r_crop);
                }
            }
        }


        //아이템 정보 (r_item)
        if (Resources.Load<TextAsset>("Data/r_item") != null)
        {
            str = Resources.Load<TextAsset>("Data/r_item").ToString();
            Serialization<r_item> loadData = JsonUtility.FromJson<Serialization<r_item>>(str);

            data_r_item_list.Clear();


            if (loadData != null)
            {
                foreach (r_item r_item in loadData.data)
                {
                    data_r_item_list.Add(r_item);
                }
            }
        }

        //상점 정보 (r_shop)
        if (Resources.Load<TextAsset>("Data/r_shop") != null)
        {
            str = Resources.Load<TextAsset>("Data/r_shop").ToString();
            Serialization<r_shop> loadData = JsonUtility.FromJson<Serialization<r_shop>>(str);

            data_r_shop_list.Clear();


            if (loadData != null)
            {
                foreach (r_shop r_shop in loadData.data)
                {
                    data_r_shop_list.Add(r_shop);
                }
            }
        }
#endif



        initDataLoad = true;

        yield return null;
    }

    //플레이어 데이터 불러오기
    public IEnumerator LoadPlayerData()
    {
        
        if (System.IO.File.Exists(Application.dataPath + "/SaveFile.json"))
        {
            string str = File.ReadAllText(Application.dataPath + "/SaveFile.json");
            var loadData = JsonUtility.FromJson<SaveData>(str);

            //player_data
            player_data.day = loadData.p_data.day;
            player_data.current_HP = loadData.p_data.current_HP;
            player_data.max_HP = loadData.p_data.max_HP;


            //p_dirt
            p_dirt_list.Clear();
           
            for (int i = 0; i < loadData.dirt_id_list.Count; i++)
            {
                p_dirt p_dirt = new p_dirt();
                p_dirt.dirt_id = loadData.dirt_id_list[i];
                p_dirt.dirt_crop_id = loadData.dirt_crop_list[i];
                p_dirt.dirt_crop_growth = loadData.dirt_crop_growth_list[i];
                p_dirt.dirt_moisture = loadData.dirt_moisture_list[i];

                p_dirt_list.Add(p_dirt);
            }

            //p_item
            p_item_list.Clear();

            for(int i = 0; i < loadData.item_id_list.Count; i++)
            {
                p_item p_item = new p_item();
                p_item.item_id = loadData.item_id_list[i];
                p_item.item_amount = loadData.item_amount_list[i];

                p_item_list.Add(p_item);
            }

            //p_chicken
            p_chicken_list.Clear();
            
            for(int i = 0; i < loadData.chicken_id_list.Count; i++)
            {
                p_chicken p_Chicken = new p_chicken();
                p_Chicken.chicken_id = loadData.chicken_id_list[i];
                p_Chicken.egg = loadData.egg_list[i];

                p_chicken_list.Add(p_Chicken);
            }
        }
        else
        {
            CreatePlayerData();
        }

        coop_feed_box = new p_item[data_r_coop.coop_max];

        for(int i = 0; i < data_r_coop.coop_max; i++)
        {
            p_item p_Item = new p_item();

            p_Item.item_id = 0;
            p_Item.item_amount = 0;

            coop_feed_box[i] = p_Item;
        }

        playerDataLoad = true;

        yield return null;
    }


    public void ItemUpdate(r_item _r_item, int amount)
    {
        p_item p_item;

        if(amount > 0)
        {
            if (p_item_list.Exists(r => r.item_id == _r_item.item_id))
            {
                p_item = p_item_list.SingleOrDefault(r => r.item_id == _r_item.item_id);
                p_item.item_amount += amount;
            }
            else
            {
                p_item = new p_item();
                p_item.item_id = _r_item.item_id;
                p_item.item_amount = amount;

                p_item_list.Add(p_item);
            }
        }
        else
        {
            if (p_item_list.Exists(r => r.item_id == _r_item.item_id))
            {
                p_item = p_item_list.SingleOrDefault(r => r.item_id == _r_item.item_id);
                p_item.item_amount += amount;
            }
        }
    }

    //물품 구매
    public void Buy(r_shop _r_shop, int amount)
    {
        p_item money = p_item_list.SingleOrDefault(r => r.item_id == 10001);

        if((_r_shop.shop_price * amount) <= (money.item_amount))
        {
            r_item r_item = data_r_item_list.SingleOrDefault(r => r.item_id == _r_shop.shop_productItem);

            money.item_amount -= _r_shop.shop_price * amount;

            ItemUpdate(r_item, amount);

            SoundManager.Ins.PlaySound(SoundManager.get_Sound);
            UIManager.Ins.getItemPopupCtr.Init(r_item, amount);
        }
    }

    
    //일일 정산
    public void DayEnd()
    {
        // N일차 넘기기
        player_data.day++;


        //작물 관리
        List<p_dirt> p_dirts = p_dirt_list.Where(r => r.dirt_crop_id > 0).ToList();

        if(p_dirts.Count > 0)
        {
            r_crop r_crop;

            for (int i = 0; i < p_dirts.Count; i++)
            {
                if (p_dirts[i].dirt_moisture)
                {
                    r_crop = data_r_crop_list.SingleOrDefault(r => r.crop_id == p_dirts[i].dirt_crop_id);

                    p_dirts[i].dirt_crop_growth++;

                    if (p_dirts[i].dirt_crop_growth > r_crop.crop_growth)
                        p_dirts[i].dirt_crop_growth = r_crop.crop_growth;

                    p_dirts[i].dirt_moisture = false;
                }
            }
        }
        

        //닭 관리
        for(int i = 0; i < p_chicken_list.Count; i++)
        {
            p_item feed = coop_feed_box.FirstOrDefault(r => r.item_id == 10002);

            if(feed != null)
            {
                p_chicken_list[i].egg = true;

                feed.item_id = 0;
                feed.item_amount = 0;
            }
        }

        for(int i = 0; i < coop_feed_box.Length; i++)
        {
            if (coop_feed_box[i] != null)
            {
                coop_feed_box[i].item_id = 0;
                coop_feed_box[i].item_amount = 0;
            }
        }

    }
}
