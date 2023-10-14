using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_RunState : Boss_AssassinCultist_ChildState
{
    public override void EnterState(Boss_AssassinCultist boss)
    {
        boss.anim.SetBool("Run", true);
    }

    public override void UpdateState(Boss_AssassinCultist boss)
    {
        if(Mathf.Abs(boss.target.position.x - boss.transform.position.x) > 0.1f)
        {
            boss.Movement();
        }
        else
        {
            //转换为站立形态
            ExitState(boss);
            boss.currentParentState.TransitionChildState(boss.currentParentState.idleState, boss);
        }
    }

    public override void ExitState(Boss_AssassinCultist boss)
    {
        boss.anim.SetBool("Run", false);
    }
}
