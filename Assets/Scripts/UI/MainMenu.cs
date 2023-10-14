using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public GameObject textUp;
    public GameObject textDown;
    
    void Awake()
    {
        gameObject.SetActive(false);
    }

    public void CloseMainMenu()
    {
        GetComponent<Animator>().SetTrigger("Close");
        textDown.GetComponent<Animator>().SetTrigger("Close");
        textUp.GetComponent<Animator>().SetTrigger("Close");
    }

    //Animation Event
    public void Close()
    {
        gameObject.SetActive(false);
    }
}
