using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : BaseFSM
{
    public override void EnterState(Enemy enemy)
    {
        enemy.isAttack = true;
        
    }

    public override void UpdateState(Enemy enemy)
    {
        if(!enemy.anim.GetCurrentAnimatorStateInfo(1).IsName("Attack1") && !enemy.anim.GetCurrentAnimatorStateInfo(1).IsName("Attack2"))
        {
            enemy.TurnAround();

            if(enemy.targets.Count <= 0)
            {
                ResetAttackState(enemy);
                enemy.TransitionState(enemy.patrolState);
            }
            else if(Mathf.Abs(enemy.target.position.x - enemy.transform.position.x) >= enemy.characterStats.AttackDistance)
            {
                ResetAttackState(enemy);
                enemy.TransitionState(enemy.followState);
            }
        }
    }

    void ResetAttackState(Enemy enemy)
    {
        enemy.isAttack = false;
    }
}
