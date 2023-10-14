using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogueList
{
    public string ID;   //记录对话组的ID
    public List<DialoguePiece> dialoguePieces = new List<DialoguePiece>();
}
