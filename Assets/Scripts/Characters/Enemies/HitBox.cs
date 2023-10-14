using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBox : MonoBehaviour
{
    private int dir = 0;    //用于记录方向
    public float attackForce;   //记录该力的大小
    CharacterStats characterStats;
    
    void Awake()
    {
        characterStats = GetComponentInParent<CharacterStats>();
    }

    private void OnEnable() 
    {
        characterStats = GetComponentInParent<CharacterStats>();
    }

    void OnTriggerEnter2D(Collider2D other) 
    {
        if(transform.position.x > other.transform.position.x)
            dir = -1;
        else
            dir = 1;

        if((other.CompareTag("Player") || other.CompareTag("Enemies")) && characterStats.attackData != null)
        {
            if(characterStats.attackData != other.GetComponent<CharacterStats>().attackData)
            {
                other.GetComponent<IDamageable>().GetHit(characterStats);
                other.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(dir,0)*attackForce, ForceMode2D.Impulse);
            }
        }

        if(other.CompareTag("AttackDashShadow"))
        {
            other.GetComponent<AttackDashShadow>().BeAttack(characterStats);
        }
    }
}
