using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Boss_AssassinCultist_ParentState
{
    public Boss_AssassinCultist_ChildState currentChildState;
    
    public Boss_IdleState idleState = new Boss_IdleState();
    public Boss_RunState runState = new Boss_RunState();
    public Boss_HideState hideState = new Boss_HideState();
    public Boss_AppearState appearState = new Boss_AppearState();
    public Boss_AppearAttackState appearAttackState = new Boss_AppearAttackState();
    public Boss_FollowTargetState followTargetState = new Boss_FollowTargetState();
    public Boss_AttackTargetState attackTargetState = new Boss_AttackTargetState();
    public Boss_SkillAttackState skillAttackState = new Boss_SkillAttackState();
    public Boss_AttackDashState attackDashState = new Boss_AttackDashState();

    public abstract void EnterState(Boss_AssassinCultist boss);
    public abstract void UpdateState(Boss_AssassinCultist boss);
    public abstract void ExitState(Boss_AssassinCultist boss);
    
    public void TransitionChildState(Boss_AssassinCultist_ChildState state, Boss_AssassinCultist boss)
    {
        currentChildState = state;
        currentChildState.EnterState(boss);
    }
}
