using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

[RequireComponent(typeof(ItemUI))]
public class DragItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    bool isSameItem;
    ItemUI currentItemUI;
    SlotHolder currentHolder;
    SlotHolder targetHolder;

    void Awake()
    {
        currentItemUI = GetComponent<ItemUI>();
        currentHolder = GetComponentInParent<SlotHolder>();
    }

    public void OnBeginDrag(PointerEventData eventData) //开始拖拽
    {
        ResetCharacterStats();
        InventoryManager.Instance.currentDrag = new InventoryManager.DragData();
        InventoryManager.Instance.currentDrag.originalHolder = GetComponent<SlotHolder>();
        InventoryManager.Instance.currentDrag.originalParent = (RectTransform)transform.parent;
        transform.SetParent(InventoryManager.Instance.dragCanvas.transform, true);

    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        //放下物品，交换数据
        if(EventSystem.current.IsPointerOverGameObject())
        {
            if(InventoryManager.Instance.CheckInInventory(eventData.position))
            {
                if(eventData.pointerEnter.gameObject.GetComponent<SlotHolder>())
                {
                    targetHolder = eventData.pointerEnter.gameObject.GetComponent<SlotHolder>();
                }
                else
                {
                    targetHolder = eventData.pointerEnter.gameObject.GetComponentInParent<SlotHolder>();
                }

                switch(targetHolder.slotType)
                {
                    case SlotType.USEABLE:
                        if(currentItemUI.Bag.items[currentItemUI.Index].itemData.itemType == ItemType.Useable)
                        {
                            SwapItem();
                        }
                        break;
                    case SlotType.WEAPON:
                        if(currentItemUI.Bag.items[currentItemUI.Index].itemData.itemType == ItemType.Weapon)
                        {
                            SwapItem();
                        }
                        break;
                    case SlotType.ARMOR:
                        if(currentItemUI.Bag.items[currentItemUI.Index].itemData.itemType == ItemType.Armor)
                        {
                            SwapItem();
                        }
                        break;
                    case SlotType.EQUIP:
                        if(currentItemUI.Bag.items[currentItemUI.Index].itemData.itemType == ItemType.Weapon || 
                            currentItemUI.Bag.items[currentItemUI.Index].itemData.itemType == ItemType.Armor ||
                            currentItemUI.Bag.items[currentItemUI.Index].itemData.itemType == ItemType.Ornament)
                        {
                            SwapItem();
                        }
                        break;
                    case SlotType.ACTION:
                        if(currentItemUI.Bag.items[currentItemUI.Index].itemData.itemType == ItemType.Useable ||
                            currentItemUI.Bag.items[currentItemUI.Index].itemData.itemType == ItemType.Skill)
                        {
                            SwapItem();
                        }
                        break;
                }

                if(!isSameItem)
                    targetHolder.UpdateItem();
            }
        }
        currentHolder.UpdateItem();

        transform.SetParent(InventoryManager.Instance.currentDrag.originalParent);

        RectTransform t = transform as RectTransform;
        t.offsetMax = -Vector2.one * 5;
        t.offsetMin = Vector2.one * 5;
    }

    public void SwapItem()  //对真实的背包数据库进行操作
    {
        var targetItem = targetHolder.itemUI.Bag.items[targetHolder.itemUI.Index];
        var tempItem = currentHolder.itemUI.Bag.items[currentHolder.itemUI.Index];

        bool isSameIndex = targetHolder.itemUI.Index == currentHolder.itemUI.Index;
        isSameItem = tempItem.itemData == targetItem.itemData;

        if(isSameItem && targetItem.itemData.stackable && !isSameIndex)
        {
            targetItem.count += tempItem.count;
            tempItem.itemData = null;
            tempItem.count = 0;
        }
        else
        {
            currentHolder.itemUI.Bag.items[currentHolder.itemUI.Index] = targetItem;
            targetHolder.itemUI.Bag.items[targetHolder.itemUI.Index] = tempItem;
        }
    }

    void ResetCharacterStats()  //当装备被拖动时，恢复数据或是删除组件
    {
        if(currentHolder.slotType == SlotType.EQUIP)
        {
            GameManager.Instance.player.characterStats.RemoveEquipment(currentItemUI.Bag.items[currentItemUI.Index].itemData);
        }

        if(currentHolder.slotType == SlotType.ACTION)
        {
            Destroy(GameManager.Instance.player.gameObject.GetComponent(Type.GetType(currentItemUI.GetItemDataFromBag().useableData.skillName)));
            UseableManager.Instance.skillNumber.Remove(currentHolder.itemUI.GetItemDataFromBag().useableData.skillName);
        }
    }
}
