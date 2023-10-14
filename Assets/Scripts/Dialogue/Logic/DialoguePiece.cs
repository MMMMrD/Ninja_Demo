using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialoguePiece
{
    public Sprite image;    //对话者头像
    
    [TextArea]
    public string text;     //对话语句
    public bool haveEvent;  //判断当前对话是否拥有事件
    public QuestData_SO questData;  //任务数据，如果此段对话包含任务，则将对应任务数据加入
    public List<DialogueOption> dialogueOptions = new List<DialogueOption>();   //回答列表，包含玩家可以选择的所有回答
}
