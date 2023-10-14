using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//////////////////////////////////////////////////
////////////需要将该脚本挂载在任务按钮上////////////
/////////////////////////////////////////////////

public class QuestNameButton : MonoBehaviour
{
    public Text questNameText;  //任务名
    public QuestData_SO currentData;  //任务信息
    public Text questContentText;   //任务说明文本框

    void Awake()
    {
        GetComponent<Button>().onClick.AddListener(UpdateQuestContent);
    }

    void UpdateQuestContent()   //按下按钮后，更新物品信息
    {
        questContentText.text = currentData.description;    //设置任务详情
        QuestUIManager.Instance.SetupRequireList(currentData);  //更新任务完成度
        QuestUIManager.Instance.SetupRewardItem(currentData);   //更新任务奖励
    }

    //由QuestUIManager调用，更新任务按钮信息
    public void SetupNameButton(QuestData_SO questData) 
    {
        currentData = questData;
        
        if(questData.isComplete)
        {
            questNameText.text = questData.questName + "(完成)";
            // Destroy(gameObject);
        }
        else
        {
            questNameText.text = questData.questName;
        }
    }
}
