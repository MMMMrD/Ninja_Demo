using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_AssassinHalfHealthParentState : Boss_AssassinCultist_ParentState
{
    public override void EnterState(Boss_AssassinCultist boss)
    {
        TransitionChildState(boss.currentParentState.hideState, boss);
    }

    public override void UpdateState(Boss_AssassinCultist boss)
    {
        currentChildState.UpdateState(boss);
    }

    public override void ExitState(Boss_AssassinCultist boss)
    {
        followTargetState.ExitState(boss);
        attackTargetState.ExitState(boss);
        skillAttackState.ExitState(boss);
        appearAttackState.ExitState(boss);
        appearState.ExitState(boss);
        hideState.ExitState(boss);
    }
}
