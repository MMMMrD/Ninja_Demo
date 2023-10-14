using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BringerSkillAttackState : ChildStateMachine
{
    public override void EnterState(Boss boss)
    {
        boss.anim.SetTrigger("SkillAttack");
        boss.lastSkillAttack = Time.time;
        boss.lastAttack = Time.time;
    }

    public override void UpdateState(Boss boss)
    {
        if(!boss.anim.GetCurrentAnimatorStateInfo(1).IsName("Skill Attack"))
        {
            //技能攻击完成后，转换为普通攻击
            boss.currentParentState.TransitionChildState(boss.currentParentState.bringerAttackTargetState, boss);
        }
    }

    public override void ExitState(Boss boss)
    {

    }
}
