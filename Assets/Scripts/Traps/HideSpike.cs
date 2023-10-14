using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideSpike : MonoBehaviour
{
    Animator anim;
    public float attackCoolDown;
    float startAttackTime;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        startAttackTime = - attackCoolDown;
    }

    private void OnTriggerStay2D(Collider2D other) 
    {
        if(other.CompareTag("Player"))
        {
            if(Time.time >= startAttackTime + attackCoolDown)
            {
                startAttackTime = Time.time;
                if(anim != null)
                {
                    anim.SetTrigger("Attack");
                }
            }
        }   
    }
}
