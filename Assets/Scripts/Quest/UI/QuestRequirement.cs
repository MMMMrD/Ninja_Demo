using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

///////////////////////////////////////////
////////////需要挂载在需求文本上/////////////
///////////////////////////////////////////
public class QuestRequirement : MonoBehaviour
{
    private Text requireName;
    private Text progressNumber;

    void Awake()
    {
        requireName = GetComponent<Text>();
        progressNumber = transform.GetChild(0).GetComponent<Text>();
    }

    public void SetupRequirement(string name, int needCount, int currentCount)
    {
        requireName.text = name;
        progressNumber.text = currentCount.ToString() + "/" + needCount.ToString();
    }
}
