using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : Singleton<InventoryManager>
{
    public class DragData
    {
        public SlotHolder originalHolder;
        public RectTransform originalParent;
    }

    //TODO:最后添加模板用于保存数据
    [Header("Iventory Data")]
    public InventoryData_SO templateUseableBagData;
    public InventoryData_SO templateWeaponBagData;
    public InventoryData_SO templateArmorBagData;
    public InventoryData_SO templateEquipmentBagData;
    public InventoryData_SO templateActionBagData;
    public InventoryData_SO templateShopBagData;

    [Header("Template")]
    [HideInInspector]public InventoryData_SO UseableBagData;
    [HideInInspector]public InventoryData_SO WeaponBagData;
    [HideInInspector]public InventoryData_SO ArmorBagData;
    [HideInInspector]public InventoryData_SO EquipmentBagData;
    [HideInInspector]public InventoryData_SO ActionBagData;
    [HideInInspector]public InventoryData_SO ShopBagData;

    [Header("Containers")]
    public ContainerUI UseableInventoryUI;
    public ContainerUI WeaponInventoryUI;
    public ContainerUI ArmorInventoryUI;
    public ContainerUI EquipmentInventoryUI;
    public ContainerUI ActionInventoryUI;
    public ContainerUI ShopInventoryUI;

    public ContainerUI currentContainer;
    
    [Header("Bag Active")]
    public GameObject bagObject;
    [HideInInspector]public bool setBagObjectActive;

    [Header("Shop Active")]
    public GameObject shopObject;
    [HideInInspector]public bool setShopObjectActive;

    [Header("Drag Canvas")]
    public Canvas dragCanvas;
    public DragData currentDrag;

    [Header("Tooltip")]
    public ItemTooltip tooltip;

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
    }

    void Start() 
    {
        if(templateUseableBagData != null)
        {
            UseableBagData = Instantiate(templateUseableBagData);
        }

        if(templateWeaponBagData != null)
        {
            WeaponBagData = Instantiate(templateWeaponBagData);
        }

        if(templateArmorBagData != null)
        {
            ArmorBagData = Instantiate(templateArmorBagData);
        }

        if(templateEquipmentBagData != null)
        {
            EquipmentBagData = Instantiate(templateEquipmentBagData);
        }

        if(templateActionBagData != null)
        {
            ActionBagData = Instantiate(templateActionBagData);
        }

        if(templateShopBagData != null)
        {
            ShopBagData = Instantiate(templateShopBagData);
        }

        RefreshAllUI();

        currentContainer = WeaponInventoryUI;
        ArmorInventoryUI.gameObject.SetActive(false);
        UseableInventoryUI.gameObject.SetActive(false);
        ShopInventoryUI.gameObject.SetActive(false);
    }

    void Update()
    {
        setBagObjectActive = bagObject.activeSelf;
        setShopObjectActive = shopObject.activeSelf;
    }

    public void RefreshAllUI()
    {
        UseableInventoryUI.RefreshUI();
        WeaponInventoryUI.RefreshUI();
        ArmorInventoryUI.RefreshUI();
        EquipmentInventoryUI.RefreshUI();
        ActionInventoryUI.RefreshUI();
        ShopInventoryUI.RefreshUI();
    }

    public void AddItem(ItemData_SO itemData)   //外部调用，向背包中添加物品
    {
        switch (itemData.itemType)
        {
            case ItemType.Useable:
                UseableBagData.AddItem(itemData, itemData.itemCount);
                UseableInventoryUI.RefreshUI();
                break;
            case ItemType.Weapon:
                WeaponBagData.AddItem(itemData, itemData.itemCount);
                WeaponInventoryUI.RefreshUI();
                break;
            case ItemType.Armor:
                ArmorBagData.AddItem(itemData, itemData.itemCount);
                ArmorInventoryUI.RefreshUI();
                break;
        }
    }

    public void AddItem(ItemData_SO itemData, int itemCount)
    {
        switch (itemData.itemType)
        {
            case ItemType.Useable:
                UseableBagData.AddItem(itemData, itemCount);
                UseableInventoryUI.RefreshUI();
                break;
            case ItemType.Weapon:
                WeaponBagData.AddItem(itemData, itemCount);
                WeaponInventoryUI.RefreshUI();
                break;
            case ItemType.Armor:
                ArmorBagData.AddItem(itemData, itemCount);
                ArmorInventoryUI.RefreshUI();
                break;
        }
    }

    #region 检测是否在背包中对应的空格上
    public bool CheckInInventory(Vector3 position)
    {
        if(CheckInActionInventory(position) || CheckInArmorInventory(position) || CheckInEquipmentInventory(position) || 
        CheckInUseableInventory(position) || CheckInWeaponInventory(position))
        {
            return true;
        }

        return false;
    }

    public bool CheckInWeaponInventory(Vector3 position)    //判断是否在武器背包的格子上
    {
        for(int i = 0; i < WeaponInventoryUI.slotHolders.Length; i++)
        {
            RectTransform t = WeaponInventoryUI.slotHolders[i].transform as RectTransform;
            if(RectTransformUtility.RectangleContainsScreenPoint(t, position))
            {
                return true;
            }
        }
        return false;
    }

    public bool CheckInArmorInventory(Vector3 position)    //判断是否在防具背包的格子上
    {
        for(int i = 0; i < ArmorInventoryUI.slotHolders.Length; i++)
        {
            RectTransform t = ArmorInventoryUI.slotHolders[i].transform as RectTransform;
            if(RectTransformUtility.RectangleContainsScreenPoint(t, position))
            {
                return true;
            }
        }
        return false;
    }

    public bool CheckInUseableInventory(Vector3 position)    //判断是否在使用物品背包的格子上
    {
        for(int i = 0; i < UseableInventoryUI.slotHolders.Length; i++)
        {
            RectTransform t = UseableInventoryUI.slotHolders[i].transform as RectTransform;
            if(RectTransformUtility.RectangleContainsScreenPoint(t, position))
            {
                return true;
            }
        }
        return false;
    }

    public bool CheckInEquipmentInventory(Vector3 position)    //判断是否在装备栏的格子上
    {
        for(int i = 0; i < EquipmentInventoryUI.slotHolders.Length; i++)
        {
            RectTransform t = EquipmentInventoryUI.slotHolders[i].transform as RectTransform;
            if(RectTransformUtility.RectangleContainsScreenPoint(t, position))
            {
                return true;
            }
        }
        return false;
    }

    public bool CheckInActionInventory(Vector3 position)    //判断是否在道具栏的格子上
    {
        for(int i = 0; i < ActionInventoryUI.slotHolders.Length; i++)
        {
            RectTransform t = ActionInventoryUI.slotHolders[i].transform as RectTransform;
            if(RectTransformUtility.RectangleContainsScreenPoint(t, position))
            {
                return true;
            }
        }
        return false;
    }

    #endregion

    #region Button Event

    public void OpenWeaponInventoryUI()
    {
        currentContainer.gameObject.SetActive(false);
        currentContainer = WeaponInventoryUI;
        currentContainer.gameObject.SetActive(true); 
    }

    public void OpenArmorInventoryUI()
    {
        currentContainer.gameObject.SetActive(false);
        currentContainer = ArmorInventoryUI;
        currentContainer.gameObject.SetActive(true);
    }

    public void OpenUseableInventoryUI()
    {
        currentContainer.gameObject.SetActive(false);
        currentContainer = UseableInventoryUI;
        currentContainer.gameObject.SetActive(true);
    }

    public void CloseBag()
    {
        bagObject.SetActive(false);
    }

    #endregion

    #region 检测任务物品

    public void CheckQusetItemInBag(string questItemName)   //外部调用，检测背包中物品个数
    {
        foreach (var item in UseableBagData.items)
        {
            if(item.itemData != null)
            {
                if(item.itemData.itemName == questItemName)
                {
                    QuestManager.Instance.UpdateQuestProgress(item.itemData.itemName, item.itemData.itemCount);
                }
            }
        }

        foreach (var item in ActionBagData.items)
        {
            if(item.itemData != null)
            {
                if(item.itemData.itemName == questItemName)
                {
                    QuestManager.Instance.UpdateQuestProgress(item.itemData.itemName, item.itemData.itemCount);
                }
            }
        }
    }

    #endregion
}
