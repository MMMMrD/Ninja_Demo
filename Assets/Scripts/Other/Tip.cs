using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Tip : MonoBehaviour
{
    public Action openTip;
    bool haveOpen = false;
    private void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.CompareTag("Player"))
        {
            if(!haveOpen)
            {
                haveOpen = true;
                openTip?.Invoke();
            }
        }
    }
}
