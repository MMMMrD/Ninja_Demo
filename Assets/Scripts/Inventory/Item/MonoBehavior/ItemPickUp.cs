using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickUp : MonoBehaviour
{
    public ItemData_SO itemData;
    public void OnTriggerEnter2D(Collider2D other) 
    {   
        if(other.CompareTag("Player"))
        {
            //TODO:将物品添加进背包
            InventoryManager.Instance.AddItem(itemData);

            // GameManager.Instance.player.characterStats.EquipWeapon(itmeData);

            //如果任务需要收集该物品，则更新任务进度
            QuestManager.Instance.UpdateQuestProgress(itemData.itemName, itemData.itemCount);
            Destroy(gameObject);
        } 
    }
}
