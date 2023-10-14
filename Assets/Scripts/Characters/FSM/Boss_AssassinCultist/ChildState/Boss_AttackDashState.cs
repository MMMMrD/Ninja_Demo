using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Boss_AttackDashState : Boss_AssassinCultist_ChildState
{
    int num = 0;
    bool attackDashEnd = false;
    float attackDashCoolDown = 0.2f;
    float attackDashTime = 0f;
    public override void EnterState(Boss_AssassinCultist boss)
    {
        num = 0;
        attackDashTime = 0f;
        attackDashEnd = false;
        GameManager.Instance.player.applyMove = true;
        GameManager.Instance.player.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
    }

    public override void UpdateState(Boss_AssassinCultist boss)
    {
        if(Time.time >= attackDashTime + attackDashCoolDown)
        {
            if(boss.attackDashShadows.Count > num)
            {
                if(boss.attackDashShadows[num].gameObject.activeSelf == true)
                {
                    attackDashTime = Time.time;
                    boss.attackDashShadows[num].canClose = true;
                    boss.attackDashShadows[num++].MoveToTarget();
                }
                else
                {
                    num++;
                }
            }
            else
            {
                boss.attackDashShadows.Clear();
                attackDashEnd = true;
                GameManager.Instance.player.applyMove = false;
            }
        }

        if(attackDashEnd)
        {
            if(GameManager.Instance.player.isGround)
            {
                boss.currentParentState.TransitionChildState(boss.currentParentState.appearAttackState, boss);
            }
            else
            {
                boss.currentParentState.TransitionChildState(boss.currentParentState.appearState, boss);
            }
        }
    }

    public override void ExitState(Boss_AssassinCultist boss)
    {
        boss.rb.velocity = new Vector2(0, 0);
    }
}
