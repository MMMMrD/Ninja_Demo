using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestUIManager : Singleton<QuestUIManager>
{
    [Header("Base Data")]
    public GameObject questPanel;
    public ItemTooltip tooltip;
    bool isOpen;
    
    [Header("Quest Name")]
    public RectTransform questListTransform;
    public QuestNameButton questNameButton;

    [Header("Text Content")]
    public Text questContentText;

    [Header("Requirement")]
    public RectTransform requireTransform;
    public QuestRequirement questRequirement;

    [Header("Reward")]
    public RectTransform RewardTransform;
    public ItemUI rewardItem;

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.N))
        {
            isOpen = !isOpen;
            questPanel.gameObject.SetActive(isOpen);
            questContentText.text = "";

            if(tooltip.gameObject.activeSelf)
            {
                tooltip.gameObject.SetActive(isOpen);
            }

            //更新任务列表
            SetupQuestList();
        }
    }

    public void SetupQuestList()    //更新任务列表
    {
        //当按下按钮后，先将之前的所有数据删除
        foreach (Transform item in questListTransform)
        {
            Destroy(item.gameObject);
        }

        foreach (Transform item in requireTransform)
        {
            Destroy(item.gameObject);
        }

        foreach(Transform item in RewardTransform)
        {
            Destroy(item.gameObject);
        }

        foreach(var tark in QuestManager.Instance.tasks)    //从QuestManager中的任务列表中寻找任务
        {
            var newButton = Instantiate(questNameButton, questListTransform);
            newButton.SetupNameButton(tark.questData);
            newButton.questContentText = questContentText;  //将任务信息文本框赋值给任务按钮
        }
    }

    public void SetupRequireList(QuestData_SO questData)    //任务按钮调用，重置任务进度
    {
        foreach (Transform item in requireTransform)    //首先将之前的任务进度删除
        {
            Destroy(item.gameObject);
        }

        foreach (var require in questData.questRequires)    //然后根据当前任务，更新新的任务进度
        {
            var rq = Instantiate(questRequirement, requireTransform);
            rq.SetupRequirement(require.name, require.requireAmount, require.currentAmount);
        }
    }

    public void SetupRewardItem(QuestData_SO questData) //任务按钮调用，重置任务奖励
    {
        foreach (Transform item in RewardTransform)
        {
            Destroy(item.gameObject); 
        }

        foreach (var item in questData.rewards)
        {
            ItemUI rewardItemUI = Instantiate(rewardItem, RewardTransform);
            rewardItemUI.SetUpItemUI(item.itemData, item.count, false);
        }
    }
}
