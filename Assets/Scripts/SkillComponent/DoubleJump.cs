using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleJump : MonoBehaviour
{
    void OnEnable() 
    {
        if(GameManager.Instance == null) return;

        if(GameManager.Instance.player != null)
        {
            GameManager.Instance.player.baseJumpCount ++;
        }
    }

    void OnDisable() 
    {
        if(GameManager.Instance == null) return;

        if(GameManager.Instance.player != null)
        {
            GameManager.Instance.player.baseJumpCount --;
        }
    }
}
