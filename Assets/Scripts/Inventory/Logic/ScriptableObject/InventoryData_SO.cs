using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory/Iventory Data")]
public class InventoryData_SO : ScriptableObject
{
    public List<InventoryItem> items = new List<InventoryItem>();

    public void AddItem(ItemData_SO newItemData, int count)
    {
        bool found = false;
        if(newItemData.stackable)
        {
            foreach (var item in items)
            {
                if(item.itemData == newItemData)
                {
                    item.count += count;
                    if(item.count < 0)
                    {
                        item.itemData = null;
                    }
                    found = true;
                    break;
                }
            }
        }

        if(!found)
        {
            if(count > 0)
            {
                for(int i = 0; i < items.Count; i++)
                {
                    if(items[i].itemData == null)
                    {
                        items[i].itemData = newItemData;
                        items[i].count = count;
                        break;
                    }
                }
            }

            if(count < 0)
            {
                int needCount = -count;
                Debug.Log(needCount);
                for(int i = 0; needCount > 0 && i < items.Count; i++)
                {
                    if(items[i].itemData == newItemData)
                    {
                        items[i].itemData = null;
                        needCount--;
                    }
                }
            }
        }
    }
}

[System.Serializable]   //将下面的类序列化
public class InventoryItem
{
    public ItemData_SO itemData;
    public int count;
}
