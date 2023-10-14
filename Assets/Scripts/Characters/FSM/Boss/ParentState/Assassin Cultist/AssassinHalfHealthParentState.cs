using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssassinHalfHealthParentState : ParentStateMachine
{
    public override void EnterState(Boss boss)
    {
        TransitionChildState(boss.currentParentState.hideState, boss);
    }

    public override void UpdateState(Boss boss)
    {
        currentChildState.UpdateState(boss);
    }

    public override void ExitState(Boss boss)
    {
        followTargetState.ExitState(boss);
        attackTargetState.ExitState(boss);
        skillAttackState.ExitState(boss);
        appearAttackState.ExitState(boss);
        appearState.ExitState(boss);
        hideState.ExitState(boss);
    }
}
