using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogueOption
{
    [TextArea]
    public string text; //回答文本
    public string targetID; //选择该回答后，需要跳转到的对话ID
    public bool takeQuest;  //设置选择该选项后能否获得对话包含的任务
    public bool canDo;  //设置选择该选项后，需要执行特定函数
}
