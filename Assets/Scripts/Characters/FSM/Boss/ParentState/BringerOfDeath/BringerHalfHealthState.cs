using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BringerHalfHealthState : ParentStateMachine
{
    public override void EnterState(Boss boss)
    {
        TransitionChildState(boss.currentParentState.bringerHalfSkillState, boss);
        boss.isHalfHealth = true;
    }

    public override void UpdateState(Boss boss)
    {
        currentChildState.UpdateState(boss);
    }

    public override void ExitState(Boss boss)
    {
        bringerFollowState.ExitState(boss);
        bringerAttackTargetState.ExitState(boss);
        bringerSkillAttackState.ExitState(boss);
        bringerAppearState.ExitState(boss);
        bringerHideState.ExitState(boss);
    }
}
