using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackParentState : ParentStateMachine
{
    public override void EnterState(Boss boss)
    {
        if(boss.characterStats.CurrentHealth > 0)
            EventManager.Instance?.InvokeEvent<UI>(UI.UpdateBossHealth, boss.characterStats.CurrentHealth, boss.characterStats.MaxHealth);

        //更新boss的速度倍数
        boss.speedMultiple = 0.8f;
        boss.baseSpeedMultiple = boss.speedMultiple;

        boss.target = GameManager.Instance.player.transform;     //直接将Player永久设置为Boss的目标
        TransitionChildState(boss.currentParentState.followTargetState, boss);
    }

    public override void UpdateState(Boss boss)
    {
        currentChildState.UpdateState(boss);
        if(boss.characterStats.CurrentHealth < boss.characterStats.MaxHealth * 0.5f)
        {
            ExitState(boss);
            boss.TransitionState(boss.halfHealthParentState);   //半血以下转入半血以下状态
        }
    }

    public override void ExitState(Boss boss)
    {
        followTargetState.ExitState(boss);
        attackTargetState.ExitState(boss);
        skillAttackState.ExitState(boss);
    }
}
