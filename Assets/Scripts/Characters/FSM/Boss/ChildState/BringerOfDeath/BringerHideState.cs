using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BringerHideState : ChildStateMachine
{
    float random;   //随机数
    public override void EnterState(Boss boss)
    {
        boss.lastskill = Time.time;
        random = Random.Range(1f, 1.5f);
        boss.anim.SetTrigger("Hide");
    }

    public override void UpdateState(Boss boss)
    {
        if(!boss.anim.GetCurrentAnimatorStateInfo(3).IsName("Hide"))
        {    
            if(Time.time >= boss.lastskill + random)
            {
                //转入显性状态
                ExitState(boss);
                boss.currentParentState.TransitionChildState(boss.currentParentState.bringerAppearState, boss);
            }
        }
    }

    public override void ExitState(Boss boss)
    {

    }
}
