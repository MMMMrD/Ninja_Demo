using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TipsManager : MonoBehaviour
{
    public GameObject tip1;
    public GameObject tip2;
    public GameObject tip3;
    public GameObject tip4;

    public GameObject trigger1;
    public GameObject trigger2;
    public GameObject trigger3;

    void OpenTip1()
    {
        tip1.SetActive(true);
        Time.timeScale = 0f;
    }

    void OpenTip2()
    {
        tip2.SetActive(true);
        Time.timeScale = 0f;
    }

    void OpenTip3()
    {
        tip3.SetActive(true);
        Time.timeScale = 0f;
    }

    void OpenTip4()
    {
        tip4.SetActive(true);
        Time.timeScale = 0f;
    }

    public void CloseTip1()
    {
        tip1.SetActive(false);
        Time.timeScale = 1f;
    }

    public void CloseTip2()
    {
        tip2.SetActive(false);
        Time.timeScale = 1f;
    }

    public void CloseTip3()
    {
        tip3.SetActive(false);
        Time.timeScale = 1f;
    }

    public void CloseTip4()
    {
        tip4.SetActive(false);
        Time.timeScale = 1f;
        GameManager.Instance.scenceEvent -= OpenTip4;
    }

    void Start()
    {
        if(trigger1 != null)
        {
            trigger1.GetComponent<Tip>().openTip += OpenTip1;
        }
        
        if(trigger2 != null)
        {
            trigger2.GetComponent<Tip>().openTip += OpenTip2;
        }

        if(trigger3 != null)
        {
            trigger3.GetComponent<Tip>().openTip += OpenTip3;
        }

        if(GameManager.Instance != null)
        {
            GameManager.Instance.scenceEvent += OpenTip4;
        }
    }

}
