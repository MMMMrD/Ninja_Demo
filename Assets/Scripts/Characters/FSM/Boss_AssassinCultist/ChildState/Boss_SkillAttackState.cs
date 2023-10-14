using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_SkillAttackState : Boss_AssassinCultist_ChildState
{
    public override void EnterState(Boss_AssassinCultist boss)
    {
        boss.isAttack = true;
        boss.anim.SetTrigger("SkillAttack");
        boss.lastSkillAttack = Time.time;
        boss.lastAttack = Time.time;
    }

    public override void UpdateState(Boss_AssassinCultist boss)
    {
        if(!boss.anim.GetCurrentAnimatorStateInfo(1).IsName("Skill Attack"))
        {
            //技能攻击完成后，转换为普通攻击
            boss.currentParentState.TransitionChildState(boss.currentParentState.attackTargetState, boss);
        }
    }

    public override void ExitState(Boss_AssassinCultist boss)
    {

    }
}
