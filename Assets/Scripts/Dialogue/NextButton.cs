using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public enum ButtonState
{
    Simple,
    Dialoge,
}

public class NextButton : MonoBehaviour
{
    string[] dialoge;
    TMP_Text text;
    Button button;
    ButtonState buttonState = ButtonState.Simple;

    void Start()
    {
        InitComponent();
        Init();

        button?.onClick.AddListener(OnClickButton);
    }

    void InitComponent()
    {
        text = GetComponentInChildren<TMP_Text>();
        button = GetComponent<Button>();
    }

    void Init()
    {
        if(buttonState == ButtonState.Dialoge)
        {
            text.text = dialoge[4];
        }
        else if(buttonState == ButtonState.Simple)
        {
            text.text = "Next";
        }
    }

    public void Init(ButtonState buttonState, string[] dialoge) //外部调用初始化
    {
        this.buttonState = buttonState;
        this.dialoge = dialoge;
    }

    void OnClickButton()
    {
        
        switch(buttonState)
        {
            case ButtonState.Simple:
                EventManager.Instance?.InvokeEvent(DialogeState.ShowDialogeRow);
                break;
            
            case ButtonState.Dialoge:
                EventManager.Instance?.InvokeEvent(DialogeState.ShowDialogeRowUseIndex, float.Parse(dialoge[5]));
                break;
        }
    }


#region Create Funtion
    public NextButton(){}
    public NextButton(ButtonState buttonState, string[] dialoge)
    {
        this.buttonState = buttonState;
        this.dialoge = dialoge;
    }
#endregion

}
