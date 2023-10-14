using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BringerHalfSkillState : ChildStateMachine
{
    float skillTime;
    float skillCoolDown = 1;
    float skillCount = 0;
    public override void EnterState(Boss boss)
    {
        skillCount = 0;
        boss.characterStats.CurrentDefence = 100;
        skillTime = -skillCoolDown;
    }

    public override void UpdateState(Boss boss)
    {
        boss.rb.velocity = new Vector2(0f,0f);

        if(!boss.anim.GetCurrentAnimatorStateInfo(1).IsName("HalfSkillAttack") && Time.time >= skillCoolDown + skillTime)
        {
            if(skillCount > 2)
            {
                boss.lastSkillAttack = Time.time;
                ExitState(boss);
                boss.currentParentState.TransitionChildState(boss.currentParentState.bringerFollowState, boss);
            }
        }

        if(Time.time >= skillCoolDown + skillTime)
        {
            if(skillCount <= 2)
            {
                boss.anim.SetTrigger("HalfSkillAttack");
                skillTime = Time.time;
                skillCount++;
            }
        }
    }

    public override void ExitState(Boss boss)
    {
        boss.characterStats.CurrentDefence = boss.characterStats.BaseDefence;
    }
}
