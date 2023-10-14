using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType { Useable, Weapon, Armor, Ornament, Skill }

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item Data")]
public class ItemData_SO : ScriptableObject
{
    public ItemType itemType;   //item种类
    public string itemName; //item名字
    public Sprite itemIcon; //item图片
    public int itemCount;   //item数量

    [TextArea]public string description = "";   //物品信息
    public bool stackable;  //用于判断是否可以堆叠

    [Header("Weapon Data")]
    public AttackData_SO EquipData;    //武器包含的信息

    [Header("Useable Data")]
    public UseableData_SO useableData;  //使用物品信息

    [Header("Shop Item Data")]
    public ShopItemData_SO shopItemData;    //商品需要金额信息
}
