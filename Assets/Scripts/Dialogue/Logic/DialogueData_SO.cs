using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Dialogue", menuName = "Dialogue/Dialogue Data")]
public class DialogueData_SO : ScriptableObject
{
    // public List<DialoguePiece> dialoguePieces = new List<DialoguePiece>();

    // public Dictionary<string, DialoguePiece> dialogueIndex = new Dictionary<string, DialoguePiece>();

    public List<DialogueList> dialogueLists = new List<DialogueList>();

    public Dictionary<string, DialogueList> dialogueListIndex = new Dictionary<string, DialogueList>();


#if UNITY_EDITOR

    void OnValidate()
    {
        // dialogueIndex.Clear();
        dialogueListIndex.Clear();
        // foreach(var piece in dialoguePieces)
        // {
        //     if(!dialogueIndex.ContainsKey(piece.ID))
        //     {
        //         dialogueIndex.Add(piece.ID, piece);
        //     }
        // }

        foreach(var list in dialogueLists)
        {
            if(!dialogueListIndex.ContainsKey(list.ID))
            {
                dialogueListIndex.Add(list.ID, list);
            }
        }
    }
    
#endif

    public QuestData_SO GetQuest()
    {
        QuestData_SO currentQuest = null;
        foreach (var list in dialogueLists)
        {
            foreach(var piece in list.dialoguePieces)
            {
                if(piece.questData != null)
                {
                    currentQuest = piece.questData;
                }   
            }   
        }
        return currentQuest;
    }

}
