using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BringerPatrolState : ParentStateMachine
{
    public override void EnterState(Boss boss)
    {
        TransitionChildState(boss.currentParentState.bringerIdleState, boss);
    }

    public override void UpdateState(Boss boss)
    {
        currentChildState.UpdateState(boss);
        if(boss.targets.Count != 0)
        {
            ExitState(boss);
            boss.TransitionState(boss.bringerAttackState);
        }
    }

    public override void ExitState(Boss boss)
    {
        bringerIdleState.ExitState(boss);
        bringerWalkState.ExitState(boss);
    }
}
