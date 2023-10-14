using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_AppearAttackState : Boss_AssassinCultist_ChildState
{
    public override void EnterState(Boss_AssassinCultist boss)
    {
        boss.anim.SetTrigger("AppearAttack");
        boss.lastAttack = Time.time;
        boss.transform.position = boss.target.transform.position;
    }

    public override void UpdateState(Boss_AssassinCultist boss)
    {
        if(!boss.anim.GetCurrentAnimatorStateInfo(3).IsName("Appear Attack"))
        {
            boss.currentParentState.TransitionChildState(boss.currentParentState.attackTargetState, boss);
        }
    }

    public override void ExitState(Boss_AssassinCultist boss)
    {

    }
}
