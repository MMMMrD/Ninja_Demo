using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// [RequireComponent(typeof(DialogueController))]
public class QuestGiver : MonoBehaviour
{
    DialogueController controller;
    QuestData_SO currentQuest;

    public TextAsset startText;
    public TextAsset progressText;
    public TextAsset completeText;
    public TextAsset finishText;


    public DialogueData_SO startDialogue;
    public DialogueData_SO progressDialogue;
    public DialogueData_SO completeDialogue;
    public DialogueData_SO finishDialogue;

    #region 获得任务状态

    public bool IsStarted
    {
        get
        {
            if(QuestManager.Instance.HaveQuest(currentQuest))
            {
                return QuestManager.Instance.GetQuestTask(currentQuest).IsStart;
            }
            else
            {
                return false;
            }
        }
    }

    public bool IsComplete
    {
        get
        {
            if(QuestManager.Instance.HaveQuest(currentQuest))
            {
                return QuestManager.Instance.GetQuestTask(currentQuest).IsComplete;
            }
            else
            {
                return false;
            }
        }
    }

    public bool IsFinished
    {
        get
        {
            if(QuestManager.Instance.HaveQuest(currentQuest))
            {
                return QuestManager.Instance.GetQuestTask(currentQuest).IsFinished;
            }
            else
            {
                return false;
            }
        }
    }

    #endregion

    protected void Awake()
    {
        controller = GetComponent<DialogueController>();
    }

    protected void Start()
    {
        controller.dialogueText = startText;
        // currentQuest = controller.dialogueData.GetQuest();
    }

    protected void Update()
    {
        // if(IsStarted)
        // {
        //     if(IsComplete)
        //     {
        //         controller.dialogueData = completeDialogue;
        //     }
        //     else
        //     {
        //         controller.dialogueData = progressDialogue;
        //     }
        // }

        // if(IsFinished)
        // {
        //     controller.dialogueData = finishDialogue;
        // }
    }
}
