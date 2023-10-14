using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[CreateAssetMenu(fileName = "New Quest", menuName = "Quest/Quest Data")]
public class QuestData_SO : ScriptableObject
{
    [System.Serializable]
    public class QuestRequire
    {
        public string name; //目标名字
        public int requireAmount;   //需要数量
        public int currentAmount;   //当前完成数量
    }
    public string questName;    //任务名字
    [TextArea]
    public string description;  //任务描述

    public bool isStarted;
    public bool isComplete;
    public bool isFinished;
    public List<QuestRequire> questRequires = new List<QuestRequire>(); //需要完成的目标List
    public List<InventoryItem> rewards = new List<InventoryItem>(); //InventoryManager中的类，其中包含物品的信息以及物品的数量

    public void CheckQuestProgress()    //外部调用，更新是否已经完成任务
    {
        var finishRequires = questRequires.Where(r => r.requireAmount <= r.currentAmount);
        isComplete = finishRequires.Count() == questRequires.Count();
    }

    public List<string> GetRequireNames()   //外部调用，获取所有任务需求的Name
    {
        List<string> requireNames = new List<string>();

        foreach(var item in questRequires)
        {
            requireNames.Add(item.name);
        }

        return requireNames;
    }

    public void GetRewards()
    {
        foreach(var reward in rewards)
        {
            if(reward.itemData.stackable)
            {
                InventoryManager.Instance.AddItem(reward.itemData, reward.count);
            }
            else
            {
                int needCount = Mathf.Abs(reward.count);
                for(int i = 0; i < needCount; i++)
                {
                    if(reward.count > 0)
                    {
                        InventoryManager.Instance.AddItem(reward.itemData, 1);
                    }
                    else if(reward.count < 0)
                    {
                        InventoryManager.Instance.AddItem(reward.itemData, -1);
                    }
                }
            }
        }
    }
}
