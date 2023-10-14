using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_AppearState : Boss_AssassinCultist_ChildState
{
    public override void EnterState(Boss_AssassinCultist boss)
    {
        float randomX = Random.Range(boss.sceneLeftPoint.position.x, boss.sceneRightPoint.position.x);
        Vector2 destination = new Vector2(randomX, boss.transform.position.y);
        Collider2D aroundObject = Physics2D.OverlapCircle(destination, 10, boss.playerMask);

        if(aroundObject != null)
        {
            boss.currentParentState.TransitionChildState(boss.currentParentState.appearState, boss);
            return;
        }

        boss.transform.position = destination;
        boss.anim.SetTrigger("Appear");
        boss.TurnAround();
    }

    public override void UpdateState(Boss_AssassinCultist boss)
    {
        if(!boss.anim.GetCurrentAnimatorStateInfo(3).IsName("Appear"))
        {
            boss.currentParentState.TransitionChildState(boss.currentParentState.followTargetState, boss);
        }
    }

    public override void ExitState(Boss_AssassinCultist boss)
    {
        
    }
}
