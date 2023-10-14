using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Shop Item Data", menuName = "Inventory/Shop Item Data")]
public class ShopItemData_SO : ScriptableObject
{
    public int itemNeedMoney;   //该物品作为商品需要的价格
}
