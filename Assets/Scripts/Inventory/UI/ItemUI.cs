using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemUI : MonoBehaviour
{
    public Image icon = null;   //物品图片
    public Text count = null;   //物品数量
    [HideInInspector]public ItemData_SO currentItemData; //当前的物品信息
    public InventoryData_SO Bag{get; set;}  //物品属于哪个数据库
    public int Index{get; set;} = -1;   //物品所在数据库的编号

    public virtual void SetUpItemUI(ItemData_SO item, int itemCount, bool canColse)    //根据Item信息设置UI
    {
        if(itemCount == 0)
        {
            Bag.items[Index].itemData = null;
            icon.gameObject.SetActive(false);

            if(canColse)
            {
                transform.parent.gameObject.SetActive(false);
            }

            return;
        }

        if(itemCount < 0)
        {
            item = null;
        }

        if(item != null)
        {
            currentItemData = item;
            icon.sprite = item.itemIcon;
            count.text = itemCount.ToString();
            icon.gameObject.SetActive(true);
            transform.parent.gameObject.SetActive(true);
        }
        else
        {
            icon.gameObject.SetActive(false);

            if(canColse)
            {
                transform.parent.gameObject.SetActive(false);
            }
        }
    }

    public virtual ItemData_SO GetItemDataFromBag()
    {
        //从背包中获取物品信息
        return Bag.items[Index].itemData;
    }
}
