using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton_Dialogue : DialogueController
{
    // private overrid void OnTriggerEnter2D(Collider2D other)
    // {
    //     if(other.CompareTag("HitBox"))
    //     {
    //         GetComponent<Animator>().SetTrigger("BeAttack");
    //     }    
    // }

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        base.OnTriggerEnter2D(other);
        
        if(other.CompareTag("HitBox"))
        {
            GetComponent<Animator>().SetTrigger("BeAttack");
        }    
    }
}
