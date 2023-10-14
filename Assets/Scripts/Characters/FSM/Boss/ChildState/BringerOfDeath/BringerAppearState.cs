using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BringerAppearState : ChildStateMachine
{
    public override void EnterState(Boss boss)
    {
        float randomX = Random.Range(boss.sceneLeftPoint.position.x, boss.sceneRightPoint.position.x);
        Vector2 destination = new Vector2(randomX, boss.transform.position.y + 0.5f);

        // if(destination.x)

        boss.transform.position = destination;
        boss.anim.SetTrigger("Appear");
        boss.TurnAround();
    }

    public override void UpdateState(Boss boss)
    {
        if(!boss.anim.GetCurrentAnimatorStateInfo(3).IsName("Appear"))
        {
            boss.currentParentState.TransitionChildState(boss.currentParentState.bringerAttackTargetState, boss);
        }
    }

    public override void ExitState(Boss boss)
    {
        
    }
}
