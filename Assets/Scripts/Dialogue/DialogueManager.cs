using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;

public enum DialogeState
{
    ReadDialogeData,
    ShowDialogeRow,
    ShowDialogeRowUseIndex,
}

public class DialogeChoose
{
    int nextIndex = 0;
    public DialogeChoose(){}
    public DialogeChoose(int nextIndex)
    {
        this.nextIndex = nextIndex;
    }
}

public class DialogueManager : Singleton<DialogueManager>
{
    int dialogeIndex;
    string[] dialogeRows;
    private IEnumerator coroutine;

    public TextAsset dialogDataFile;
    public TMP_Text nameText;
    public TMP_Text dialogText;

    public Button nextButton;
    public Button dialogeChoosePrefab;  //对话选项预制体
    public Transform dialogueWindow;    //整个对话窗口
    public Transform dialogeButtonFather;

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
    }
    private void Start() 
    {

        EventManager.Instance.AddListener<DialogeState>(DialogeState.ShowDialogeRow, ShowDialogeRow);
        EventManager.Instance.AddListener<DialogeState>(DialogeState.ShowDialogeRowUseIndex, ShowDialogeRowUseIndex);

        // ReadDialogeData(dialogDataFile);
        // ShowDialogeRow();
        dialogueWindow.gameObject.SetActive(false);
    }

    public void UpdateText(string _name, string _text)
    {
        if(nameText != null)
            nameText.text = _name;
        if(dialogText != null)
        {
            // dialogText.text = _text;
            // dialogText.Dotext
            coroutine = ShowTextString(dialogText, _text);
            StartCoroutine(coroutine);
        }
    }

    IEnumerator ShowTextString(TMP_Text text, string _text)
    {
        int len = _text.Length;
        for(int i = 0; i < len; i++)
        {
            string strs = _text.Substring(0, i);
            text.text = strs;
            yield return new WaitForSeconds(0.05f);
        }

        text.text = _text;

        yield return null;
    }

    public void ReadDialogeData(TextAsset _dialogeText)
    {
        dialogeIndex = 0;   //重置对话数
        dialogeRows = _dialogeText.text.Split('\n');
        dialogueWindow.gameObject.SetActive(true);
        ShowDialogeRow();
        
        Debug.Log("Read Text Success");
    }
    void ShowSwitch(string[] dialoges)
    {
        DialogeChoose dialogeChoose = new DialogeChoose(int.Parse(dialoges[5]));
        //TODO:展示对话框
        Button dialogeButton = Instantiate(dialogeChoosePrefab, dialogeButtonFather);
        dialogeButton.GetComponent<NextButton>().Init(ButtonState.Dialoge, dialoges);
        // dialogeButton.transform.parent = dialogeButtonFather;
    }

    void DeleDialogeButton()    //用于删除对话
    {
        NextButton[] nextButtons = dialogeButtonFather.GetComponentsInChildren<NextButton>();
        foreach(var NextButton in nextButtons)
        {
            Destroy(NextButton.gameObject);
        }
    }

    void ShowDialogeRow()   //生成对话选项
    {
        bool choose = false;
        DeleDialogeButton();
        foreach(var row in dialogeRows)
        {
            string[] cell = row.Split(',');

            if(cell[0] == "#" && int.Parse(cell[1]) == dialogeIndex)
            {
                if(!choose)
                {
                    nextButton.enabled = true;
                    if(coroutine != null)
                        StopCoroutine(coroutine);
                    UpdateText(cell[2].ToString(), cell[4].ToString());
                    dialogeIndex = int.Parse(cell[5]);
                }
                break;
            }
            else if(cell[0] == "&" && int.Parse(cell[1]) == dialogeIndex)
            {
                choose = true;
                nextButton.enabled = false;
                ShowSwitch(cell);
                dialogeIndex++;
            }
            else if(cell[0] == "END")
            {
                //TODO：退出对话
                dialogueWindow.gameObject.SetActive(false);
                break;
            }
        }
    }

    void ShowDialogeRowUseIndex(object data)    //根据对话信息跳转到对应对话位置
    {
        float index = (float)data;
        
        bool choose = false;
        DeleDialogeButton();
        dialogeIndex = (int)index;

        foreach(var row in dialogeRows)
        {
            string[] cell = row.Split(',');

            if(cell[0] == "#" && int.Parse(cell[1]) == dialogeIndex)
            {
                if(!choose)
                {
                    nextButton.enabled = true;
                    if(coroutine != null)
                        StopCoroutine(coroutine);
                    UpdateText(cell[2].ToString(), cell[4].ToString());
                    dialogeIndex = int.Parse(cell[5]);
                }
                break;
            }
            else if(cell[0] == "&" && int.Parse(cell[1]) == dialogeIndex)
            {
                choose = true;
                nextButton.enabled = false;
                ShowSwitch(cell);
                dialogeIndex++;
            }
            else if(cell[0] == "END")
            {
                dialogueWindow.gameObject.SetActive(false);
                break;
            }
        }
    }
}
