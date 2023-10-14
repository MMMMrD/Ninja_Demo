using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BringerAttackState : ParentStateMachine
{
    public override void EnterState(Boss boss)
    {
        if(boss.characterStats.CurrentHealth > 0)
        {
            EventManager.Instance?.InvokeEvent<UI>(UI.UpdateBossHealth, boss.characterStats.CurrentHealth, boss.characterStats.MaxHealth);
            GameManager.Instance.scenceEvent?.Invoke();
        }

        boss.target = GameManager.Instance.player.transform;     //直接将Player永久设置为Boss的目标
        TransitionChildState(boss.currentParentState.bringerFollowState, boss);
    }

    public override void UpdateState(Boss boss)
    {
        currentChildState.UpdateState(boss);
        if(boss.characterStats.CurrentHealth <= boss.characterStats.MaxHealth * 0.5f)
        {
            if(!boss.isHide)
            {
                ExitState(boss);
                boss.TransitionState(boss.bringerHalfHealthState);   //半血以下转入半血以下状态
            }
        }
    }

    public override void ExitState(Boss boss)
    {
        bringerFollowState.ExitState(boss);
        bringerAttackTargetState.ExitState(boss);
        bringerSkillAttackState.ExitState(boss);
    }
}
