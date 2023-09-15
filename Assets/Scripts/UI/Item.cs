using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Tool,
    Seed,
    Crop
}

public class Item
{
    public string itemName;
    public int itemValue;
    public int itemPrice;
    public ItemType itemType;
    public Sprite itemImage;
}
