using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warrior_Dialogue : DialogueController
{
    bool shopActive = false;    //判断商店打开状态
    public override void DoWhenGetKeyDown()
    {
        shopActive = InventoryManager.Instance.shopObject.activeSelf;
        shopActive = !shopActive;
        InventoryManager.Instance.shopObject.SetActive(shopActive);
    }
}
