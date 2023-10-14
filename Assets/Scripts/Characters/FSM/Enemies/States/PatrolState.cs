using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : BaseFSM
{
    public override void EnterState(Enemy enemy)
    {
        enemy.anim.SetBool("Patrol", false);
        enemy.isAttack = false;
        enemy.SwitchTargetPoint();
    }

    public override void UpdateState(Enemy enemy)
    {
        if(!enemy.anim.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            if(enemy.targets.Count > 0 && !GameManager.Instance.gameOver)
            {
                enemy.TransitionState(enemy.followState);
            }
            else
            {
                enemy.anim.SetBool("Patrol", true);
                enemy.Movement();
            }
        }

        if(Mathf.Abs(enemy.target.position.x - enemy.transform.position.x) < 0.1f)
        {
            enemy.TransitionState(enemy.patrolState);
        }
    }
}
