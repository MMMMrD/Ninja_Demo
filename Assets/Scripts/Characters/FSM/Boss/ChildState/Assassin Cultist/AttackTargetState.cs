using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackTargetState : ChildStateMachine
{
    float randomTime;   //记录取随机数的时间
    float random;   //记录随机数

    public override void EnterState(Boss boss)
    {
        boss.isAttack = true;
        random = Random.Range(0, 10);
        randomTime = Time.time;
    }

    public override void UpdateState(Boss boss)
    {
        if(!boss.anim.GetCurrentAnimatorStateInfo(1).IsName("Base Attack"))
        {
            boss.TurnAround();

            if(Mathf.Abs(boss.target.position.x - boss.transform.position.x) >= boss.characterStats.AttackDistance)
            {
                ExitState(boss);
                boss.currentParentState.TransitionChildState(boss.currentParentState.followTargetState, boss);
                return;
            }
        }

        if(boss.characterStats.CurrentHealth >= boss.characterStats.MaxHealth * 0.5f) return;   //大于半血 不执行以上操作

        if((Time.time - boss.lastskill) > boss.skillCoolDown)
        {
            if((Time.time - randomTime) >= 0.5f)
            {
                randomTime = Time.time;
                random = Random.Range(0, 10);
                if(random <= 4)
                {
                    ExitState(boss);
                    boss.currentParentState.TransitionChildState(boss.currentParentState.hideState, boss);   
                }
            }
        }
    }

    public override void ExitState(Boss boss)
    {
        boss.isAttack = false;
    }
}
