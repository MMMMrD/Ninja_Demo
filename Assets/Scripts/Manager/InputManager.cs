using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : Singleton<InputManager>
{
    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
    }
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.B))
        {
            InventoryManager.Instance.bagObject.SetActive(!InventoryManager.Instance.setBagObjectActive);
        }

        if(Input.GetKeyDown(KeyCode.P))
        {
            InventoryManager.Instance.shopObject.SetActive(!InventoryManager.Instance.setShopObjectActive);
        }

        if(Input.GetKeyDown(KeyCode.Q))
        {
            GameManager.Instance.player.skillQ?.Invoke(InventoryManager.Instance.ActionInventoryUI.slotHolders[0].itemUI.GetItemDataFromBag().useableData);
        }

        if(Input.GetKeyDown(KeyCode.E))
        {
            GameManager.Instance.player.skillE?.Invoke(InventoryManager.Instance.ActionInventoryUI.slotHolders[1].itemUI.GetItemDataFromBag().useableData);
        }

        if(Input.GetKeyDown(KeyCode.U))
        {
            GameManager.Instance.player.skillU?.Invoke(InventoryManager.Instance.ActionInventoryUI.slotHolders[2].itemUI.GetItemDataFromBag().useableData);
        }

        if(Input.GetKeyDown(KeyCode.I))
        {
            GameManager.Instance.player.skillI?.Invoke(InventoryManager.Instance.ActionInventoryUI.slotHolders[3].itemUI.GetItemDataFromBag().useableData);
        }

        if(Input.GetKeyDown(KeyCode.Escape))    //唤出主菜单
        {
            EventManager.Instance?.InvokeEvent<UI>(UI.SetMainMenu);
        }
    }
}
