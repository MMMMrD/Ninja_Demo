using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/////////////////////////////////////////////////
///////////////需要挂载在NPC身上//////////////////
////////////////////////////////////////////////

public class DialogueController : MonoBehaviour 
{
    [HideInInspector]public TextAsset dialogueText;
    [HideInInspector]public DialogueData_SO dialogueData;    //对话信息
    protected bool canTalk = false;   //是否可以对话

    protected virtual void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.CompareTag("Player"))
        {
            canTalk = true;
        }    
    }
    
    protected virtual void OnTriggerExit2D(Collider2D other) 
    {
        if(other.CompareTag("Player"))
        {
            // DialogueUIManager.Instance.dialoguePanel.gameObject.SetActive(false);
            canTalk = false;
        }    
    }

    protected void Update()
    {
        if(Input.GetKeyDown(KeyCode.W) && canTalk)
        {
            if(dialogueText != null)
            {
                OpenDialoguePanel();
            }
            else
            {
                DoWhenGetKeyDown();
            }
        }
    }

    protected void OpenDialoguePanel()
    {
        //打开对话UI面板
        //传输对话内容信息
        // DialogueUIManager.Instance.UpdateDialogueData(dialogueData);
        // DialogueUIManager.Instance.UpdateMainDialogue(dialogueData.dialogueLists[0].dialoguePieces[0]);
        DialogueManager.Instance.ReadDialogeData(dialogueText);
    }

    public virtual void DoWhenGetKeyDown(){}  //当不需要对话的时候执行
}
