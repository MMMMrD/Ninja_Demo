using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ParentStateMachine
{
    [Header("Assassin Cultist State")]
    public ChildStateMachine currentChildState;
    public IdleState idleState = new IdleState();
    public RunState runState = new RunState();
    public HideState hideState = new HideState();
    public AppearState appearState = new AppearState();
    public AppearAttackState appearAttackState = new AppearAttackState();
    public FollowTargetState followTargetState = new FollowTargetState();
    public AttackTargetState attackTargetState = new AttackTargetState();
    public SkillAttackState skillAttackState = new SkillAttackState();

    [Header("Bringer Of Death State")]
    public BringerIdleState bringerIdleState = new BringerIdleState();
    public BringerWalkState bringerWalkState = new BringerWalkState();
    public BringerHideState bringerHideState = new BringerHideState();
    public BringerFollowState bringerFollowState = new BringerFollowState();
    public BringerAppearState bringerAppearState = new BringerAppearState();
    public BringerCreateThunder bringerCreateThunder = new BringerCreateThunder();
    public BringerHalfSkillState bringerHalfSkillState = new BringerHalfSkillState();
    public BringerAttackTargetState bringerAttackTargetState = new BringerAttackTargetState();
    public BringerSkillAttackState bringerSkillAttackState = new BringerSkillAttackState();

    public abstract void EnterState(Boss boss);
    public abstract void UpdateState(Boss boss);
    public abstract void ExitState(Boss boss);
    
    public void TransitionChildState(ChildStateMachine state, Boss boss)
    {
        currentChildState = state;
        currentChildState.EnterState(boss);
    }
}
