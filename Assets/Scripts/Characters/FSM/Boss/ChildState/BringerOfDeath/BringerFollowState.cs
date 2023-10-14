using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BringerFollowState : ChildStateMachine
{
    static float randomTime;   //记录取随机数的时间
    float random;   //记录随机数
    public override void EnterState(Boss boss)
    {
        randomTime = Time.time;
        boss.anim.SetBool("Run", true);
        boss.target = GameManager.Instance.player.transform;
    }

    public override void UpdateState(Boss boss)
    {
        //如果为技能攻击范围，则进进行技能攻击
        if(Mathf.Abs(boss.transform.position.x - boss.target.position.x) >= boss.characterStats.SkillDistance)
        {
            if((Time.time - boss.lastSkillAttack) > boss.skillAttackCoolDown)
            {
                ExitState(boss);
                random = Random.Range(0, 10);
                if(random <= 4)
                {
                    boss.currentParentState.TransitionChildState(boss.currentParentState.bringerCreateThunder, boss);
                }
                else
                {
                    boss.currentParentState.TransitionChildState(boss.currentParentState.bringerCreateThunder, boss);
                }
            }
        }

        if(Mathf.Abs(boss.transform.position.x - boss.target.position.x) < boss.characterStats.AttackDistance)
        {
            ExitState(boss);
            boss.currentParentState.TransitionChildState(boss.currentParentState.bringerAttackTargetState, boss);
        }
        
        // if(!boss.anim.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        boss.Movement();

        if(!boss.isHalfHealth) return;   //大于半血 不执行以上操作

        if((Time.time - boss.lastskill) > boss.skillCoolDown)
        {
            if((Time.time - randomTime) >= 0.5f)
            {
                randomTime = Time.time;
                random = Random.Range(0, 10);
                if(random <= 4)
                {
                    ExitState(boss);
                    boss.currentParentState.TransitionChildState(boss.currentParentState.bringerHideState, boss);   
                }
            }
        }
    }

    public override void ExitState(Boss boss)
    {
        boss.rb.velocity = new Vector2(0,0);
        boss.anim.SetBool("Run", false);
    }
}
