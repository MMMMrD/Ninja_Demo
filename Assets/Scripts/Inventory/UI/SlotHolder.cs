using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum SlotType  { USEABLE, WEAPON, ARMOR, EQUIP, ACTION, SHOP }
public class SlotHolder : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public SlotType slotType;
    public ItemUI itemUI;
    bool canColse = false; //根据SlotHolder类型 判断该slot对应背包中的物品为空的时，能否关闭Slot

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(itemUI.GetItemDataFromBag())
        {
            InventoryManager.Instance.tooltip.SetUpTooltip(itemUI.GetItemDataFromBag());
            InventoryManager.Instance.tooltip.gameObject.SetActive(true);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        InventoryManager.Instance.tooltip.gameObject.SetActive(false);
    }

    public void UpdateItem()
    {
        switch(slotType)
        {
            case SlotType.USEABLE:
                itemUI.Bag = InventoryManager.Instance.UseableBagData;break;
            case SlotType.WEAPON:
                itemUI.Bag = InventoryManager.Instance.WeaponBagData;break;
            case SlotType.ARMOR:
                itemUI.Bag = InventoryManager.Instance.ArmorBagData;break;
            case SlotType.EQUIP:
                itemUI.Bag = InventoryManager.Instance.EquipmentBagData;
                if(itemUI.Bag.items[itemUI.Index].itemData != null)
                {
                    GameManager.Instance.player.characterStats.GetEquip(itemUI.GetItemDataFromBag());
                }
                break;
            case SlotType.ACTION:
                itemUI.Bag = InventoryManager.Instance.ActionBagData;
                if(itemUI.Bag.items[itemUI.Index].itemData != null)
                {
                    if(!(itemUI.GetItemDataFromBag().useableData.useableType == UseableType.Skill))
                    {
                        switch(itemUI.Index)
                        {
                            case 0:
                                GameManager.Instance.player.skillQ += UseableManager.Instance.useableFunction[itemUI.Bag.items[itemUI.Index].itemData.useableData.useableType].UseableAction;
                                break;
                            case 1:
                                GameManager.Instance.player.skillE += UseableManager.Instance.useableFunction[itemUI.Bag.items[itemUI.Index].itemData.useableData.useableType].UseableAction;
                                break;
                            case 2:
                                GameManager.Instance.player.skillU += UseableManager.Instance.useableFunction[itemUI.Bag.items[itemUI.Index].itemData.useableData.useableType].UseableAction;
                                break;
                            case 3:
                                GameManager.Instance.player.skillI += UseableManager.Instance.useableFunction[itemUI.Bag.items[itemUI.Index].itemData.useableData.useableType].UseableAction;
                                break;
                        }
                    }
                    else
                    {
                        if(!UseableManager.Instance.skillNumber.ContainsKey(itemUI.GetItemDataFromBag().useableData.skillName))
                            UseableManager.Instance.skillNumber.Add(itemUI.GetItemDataFromBag().useableData.skillName, itemUI.Index);
                        UseableManager.Instance.useableFunction[itemUI.GetItemDataFromBag().useableData.useableType].UseableAction(itemUI.GetItemDataFromBag().useableData);
                    }
                }
                break;
            case SlotType.SHOP:
                itemUI.Bag = InventoryManager.Instance.ShopBagData;
                canColse = true;
                break;
        }

        var item = itemUI.Bag.items[itemUI.Index];  //这个item的类型为 InventoryItem，不是ScriptableObject数据
        itemUI.SetUpItemUI(item.itemData, item.count, canColse);
    }

    void OnDisable() 
    {
        InventoryManager.Instance.tooltip.gameObject.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(itemUI.GetItemDataFromBag())
        {
            if(eventData.clickCount % 2 == 0)   //判断当角色点击次数为两次以上时
            {
                if(slotType == SlotType.SHOP)
                {
                    var item = itemUI.Bag.items[itemUI.Index];
                    if(GameManager.Instance.player.characterStats.Money >= item.itemData.shopItemData.itemNeedMoney * item.count)
                    {
                        InventoryManager.Instance.AddItem(item.itemData, item.count);
                        GameManager.Instance.player.characterStats.Money -= item.itemData.shopItemData.itemNeedMoney * item.count;
                        item.count = 0;
                        InventoryManager.Instance.ShopInventoryUI.RefreshUI();
                    }
                }
            }
        }
    }
}
