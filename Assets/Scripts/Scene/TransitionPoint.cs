using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionPoint : MonoBehaviour
{
    public bool stopWork;   //设置传送门是否工作
    [HideInInspector]public bool canTransistion; //判断是否可以传送
    public string sceneName;    //需要去到场景的名字
    public TransitionDestination.DestinationTag destinationTag;   //目的地类型

    void Update()
    {
        TransitionToNextScene();
    }

    public void TransitionToNextScene()
    {
        if(Input.GetKeyDown(KeyCode.W) && canTransistion)
        {
            if(!stopWork)
            {
                //传送去目的地
                if(SceneManager.GetActiveScene().name != "Shop")
                {
                    Debug.Log(">>>>>>");
                    SaveManager.Instance.SaveLastScene();
                }

                SceneController.Instance.TransitionToDestination(this);
            }
        }
    }

    void OnTriggerStay2D(Collider2D other) 
    {
        if(other.CompareTag("Player"))
        {
            canTransistion = true;
        }    
    }

    void OnTriggerExit2D(Collider2D other) 
    {
        if(other.CompareTag("Player"))
        {
            canTransistion = false;
        }    
    }
}
