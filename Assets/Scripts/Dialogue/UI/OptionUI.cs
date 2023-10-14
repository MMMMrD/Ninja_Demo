using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

///////////////////////////////////////
//////////需要挂载在回答选项中//////////
//////////////////////////////////////

public class OptionUI : MonoBehaviour
{
    public TMP_Text optionText;     //对话文本
    private Button thisButton;  //按钮自身
    private DialoguePiece currentPiece;    //当前对话
    private string nextPieceID; //下一个对话的ID
    private bool canDo; //需要执行一些方法
    private bool takeQuest; //获取任务

    void Awake() 
    {
        thisButton = GetComponent<Button>();
        thisButton.onClick.AddListener(OnOptionClicked);
    }

    public void UpdateOption(DialoguePiece piece, DialogueOption option)    //外部调用，更新对话选项的各项数据。
    {
        currentPiece = piece;   //上一句对话
        optionText.text = option.text;  //回答文本
        nextPieceID = option.targetID;  //下一句对话ID
        canDo = option.canDo;   //判断是否有函数需要执行
        takeQuest = option.takeQuest;   //选择该回答选项后，是否可以获得任务
    }

    public void OnOptionClicked()
    {
        if(currentPiece.haveEvent)
        {
            if(canDo)
            {
                //TODO:执行某些函数
            }
        }

        //判断是否会获得任务
        if(currentPiece.questData != null)
        {
            var newTask = new QuestManager.QuestTask 
            {
                questData = Instantiate(currentPiece.questData)
            };

            if(takeQuest)
            {
                //获取任务
                //判断任务列表中是否已经存在该任务
                if(QuestManager.Instance.HaveQuest(newTask.questData))
                {
                    //当任务存在，判断是否完成并给予相应奖励
                    if(QuestManager.Instance.GetQuestTask(newTask.questData).IsComplete)    
                    {
                        /*这里不用上面创建的newTask，因为上面创建的newTask是临时变量，其中的值可能会与当前任务进度有所区别*/
                        QuestManager.Instance.GetQuestTask(newTask.questData).questData.GetRewards();
                    }
                }
                else
                {
                    //没有任务就接受新任务
                    QuestManager.Instance.tasks.Add(newTask);
                    QuestManager.Instance.GetQuestTask(newTask.questData).IsStart = true;

                    //检测背包中物品个数，并更新任务进度
                    //TODO:该步骤在游戏项目中不太需要，后期可以考虑删除（因为该项目没有可以采摘的食物或是其他物品，所以大概率也不会设置收集物品的任务）
                    foreach(var requireItem in newTask.questData.GetRequireNames())
                    {
                        InventoryManager.Instance.CheckQusetItemInBag(requireItem);
                    }
                }
            }
        }

        if(nextPieceID == "")   //判断下一个对话的ID是否为空
        {
            //若为空，则关闭对话窗口
            DialogueUIManager.Instance.dialoguePanel.SetActive(false);
        }
        else
        {
            //如果不为空，那么根据当前下一句对话的ID，通过字典找到对应的 DialoguePiece 数据
            DialogueUIManager.Instance.currentDialogueIndex = 0;
            DialogueUIManager.Instance.currentDialogueListKey = nextPieceID;
            DialogueUIManager.Instance.UpdateMainDialogue(DialogueUIManager.Instance.dialogueData.dialogueListIndex[nextPieceID].dialoguePieces[0]);
        }
    }
}
