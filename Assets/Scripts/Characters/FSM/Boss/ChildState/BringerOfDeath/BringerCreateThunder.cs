using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BringerCreateThunder : ChildStateMachine
{
    public override void EnterState(Boss boss)
    {
        boss.anim.SetTrigger("ThunderBirdAttack");
        boss.lastSkillAttack = Time.time;
        boss.lastAttack = Time.time;
    }

    public override void UpdateState(Boss boss)
    {
        boss.rb.velocity = new Vector2(0,0);
        if(!boss.anim.GetCurrentAnimatorStateInfo(1).IsName("ThunderBirdAttack"))
        {
            ExitState(boss);
            boss.currentParentState.TransitionChildState(boss.currentParentState.bringerAttackTargetState, boss);
        }
    }

    public override void ExitState(Boss boss)
    {
    }
}
