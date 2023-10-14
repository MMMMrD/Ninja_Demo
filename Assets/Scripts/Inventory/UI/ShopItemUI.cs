using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopItemUI : ItemUI
{
    public Text shopObjectName;
    public Text moneyCount;

    public override void SetUpItemUI(ItemData_SO item, int itemCount, bool canColse)
    {
        if(itemCount == 0)
        {
            Bag.items[Index].itemData = null;
            icon.gameObject.SetActive(false);
            shopObjectName.gameObject.SetActive(false);
            moneyCount.gameObject.SetActive(false);

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
            shopObjectName.text = item.itemName;
            moneyCount.text = (item.shopItemData.itemNeedMoney * item.itemCount).ToString();
            icon.sprite = item.itemIcon;
            count.text = itemCount.ToString();


            icon.gameObject.SetActive(true);
            shopObjectName.gameObject.SetActive(true);
            moneyCount.gameObject.SetActive(true);
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
}
