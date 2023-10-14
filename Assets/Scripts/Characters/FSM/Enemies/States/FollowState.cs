using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowState : BaseFSM
{
    public override void EnterState(Enemy enemy)
    {
        if(enemy.isBoss && enemy.characterStats.CurrentHealth > 0)
        {
            EventManager.Instance?.InvokeEvent<UI>(UI.UpdateBossHealth, enemy.characterStats.CurrentHealth, enemy.characterStats.MaxHealth);
        }
        
        enemy.anim.SetBool("Patrol", true);
        foreach (var item in enemy.targets)
        {
            if(item.gameObject.CompareTag("Player"))
            {
                enemy.target = item;
            } 
        }
    }

    public override void UpdateState(Enemy enemy)
    {
        if(enemy.targets.Count <= 0)
        {
            enemy.anim.SetBool("Patrol", false);
            enemy.TransitionState(enemy.patrolState);
        }

        if(Mathf.Abs(enemy.transform.position.x - enemy.target.position.x) < enemy.characterStats.AttackDistance)
        {
            enemy.anim.SetBool("Patrol", false);
            enemy.TransitionState(enemy.attackState);
        }

        if(!enemy.anim.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            enemy.Movement();
        }
    }
}
