using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_PatrolParentState : Boss_AssassinCultist_ParentState
{
    public override void EnterState(Boss_AssassinCultist boss)
    {
        TransitionChildState(boss.currentParentState.idleState, boss);
    }

    public override void UpdateState(Boss_AssassinCultist boss)
    {
        currentChildState.UpdateState(boss);
        if(boss.targets.Count != 0)
        {
            ExitState(boss);
            boss.TransitionState(boss.attackParentState);
        }
    }

    public override void ExitState(Boss_AssassinCultist boss)
    {
        idleState.ExitState(boss);
        runState.ExitState(boss);
    }
}
