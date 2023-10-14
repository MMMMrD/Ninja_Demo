using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

/////////////////////////////////////////////////
//////////////需要挂载在对话Canvas上//////////////
////////////////////////////////////////////////

public class DialogueUIManager : Singleton<DialogueUIManager>
{
    [Header("Base Data")]
    public Image icon;  //对话窗口所显示的图片
    public Text mainText;   //对话窗口
    public Button nextButton;   //继续对话按钮
    public GameObject dialoguePanel;    //对话面板
    
    [Header("Options")]
    public RectTransform optionPanel;   //对话选项相关面板
    public OptionUI optionPrefab;   //选项框预制体

    [Header("Dialogue Data")]
    [HideInInspector]public DialogueData_SO dialogueData;
    [HideInInspector]public string currentDialogueListKey; //当前对话list的Key
    [HideInInspector]public int currentDialogueIndex = 0;   //判断当前对话进行到哪一步

    protected override void Awake()
    {
        base.Awake();
        nextButton.onClick.AddListener(ContinueDialogue);
        DontDestroyOnLoad(this);
    }

    public void UpdateDialogueData(DialogueData_SO data)    //外部调用，更新物品信息
    {
        dialogueData = data;
        currentDialogueListKey = "list1";
        currentDialogueIndex = 0;
    }

    void ContinueDialogue() //Next按钮按下后执行该函数
    {
        //往下还有对话时，需要更新对话文本
        //往下没有对话时，需要关闭对话窗口
        if(currentDialogueIndex < dialogueData.dialogueListIndex[currentDialogueListKey].dialoguePieces.Count)
        {
            UpdateMainDialogue(dialogueData.dialogueListIndex[currentDialogueListKey].dialoguePieces[currentDialogueIndex]);
        }
        else
        {
            dialoguePanel.SetActive(false);
        }
    }

    public void UpdateMainDialogue(DialoguePiece piece)  //打开对话窗口，同时更新回答选项，并且更新对话选项包含的数据
    {
        dialoguePanel.SetActive(true);
        currentDialogueIndex ++;
        
        if(piece.image != null)
        {
            icon.enabled = true;
            icon.sprite = piece.image;
        }
        else
        {
            icon.enabled = false;
        }

        mainText.text = " ";
        // mainText.text = piece.text;
        mainText.DOText(piece.text, 1f);

        if(piece.dialogueOptions.Count == 0 && dialogueData.dialogueListIndex[currentDialogueListKey].dialoguePieces.Count > 0)
        {
            nextButton.interactable = true;
            nextButton.transform.GetChild(0).gameObject.SetActive(true);
        }
        else
        {
            nextButton.interactable = false;
            nextButton.transform.GetChild(0).gameObject.SetActive(false);
        }

        //创建Option
        CreateOptions(piece);
    }

    //生成回答框函数
    void CreateOptions(DialoguePiece piece)
    {
        if(optionPanel.childCount > 0)  //找出上一次所有对话的选项并销毁
        {
            for(int i = 0; i < optionPanel.childCount; i++)
            {
                Destroy(optionPanel.GetChild(i).gameObject);
            }
        }

        for(int i = 0; i < piece.dialogueOptions.Count; i++)//根据包含的回答个数，生成对应的回答框
        {
            var option = Instantiate(optionPrefab, optionPanel);
            option.UpdateOption(piece, piece.dialogueOptions[i]);
        }
    }
}
