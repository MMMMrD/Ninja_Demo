using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolParentState : ParentStateMachine
{
    public override void EnterState(Boss boss)
    {
        TransitionChildState(boss.currentParentState.idleState, boss);
    }

    public override void UpdateState(Boss boss)
    {
        currentChildState.UpdateState(boss);
        if(boss.targets.Count != 0)
        {
            ExitState(boss);
            boss.TransitionState(boss.attackParentState);
        }
    }

    public override void ExitState(Boss boss)
    {
        idleState.ExitState(boss);
        runState.ExitState(boss);
    }
}
