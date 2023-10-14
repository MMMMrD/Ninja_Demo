using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_FollowTargetState : Boss_AssassinCultist_ChildState
{
    static float randomTime;   //记录取随机数的时间
    float random;   //记录随机数
    public override void EnterState(Boss_AssassinCultist boss)
    {
        randomTime = Time.time;
        boss.anim.SetBool("Run", true);
        boss.target = GameManager.Instance.player.transform;
    }

    public override void UpdateState(Boss_AssassinCultist boss)
    {
        //如果为技能攻击范围，则进进行技能攻击
        if(Mathf.Abs(Mathf.Abs(boss.transform.position.x - boss.target.position.x) - boss.characterStats.SkillDistance) < 0.1f)
        {
            if((Time.time - boss.lastSkillAttack) > boss.skillAttackCoolDown)
            {
                ExitState(boss);
                boss.currentParentState.TransitionChildState(boss.currentParentState.skillAttackState, boss);
            }
        }

        if(Mathf.Abs(boss.transform.position.x - boss.target.position.x) < boss.characterStats.AttackDistance)
        {
            ExitState(boss);
            boss.currentParentState.TransitionChildState(boss.currentParentState.attackTargetState, boss);
        }
        
        // if(!boss.anim.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        boss.Movement();

        if(boss.characterStats.CurrentHealth >= boss.characterStats.MaxHealth * 0.5f) return;   //大于半血 不执行以上操作

        if((Time.time - boss.lastskill) > boss.skillCoolDown)
        {
            if((Time.time - randomTime) >= 0.5f)
            {
                randomTime = Time.time;
                random = Random.Range(0, 10);
                if(random <= 4)
                {
                    ExitState(boss);
                    boss.currentParentState.TransitionChildState(boss.currentParentState.hideState, boss);   
                }
            }
        }
    }

    public override void ExitState(Boss_AssassinCultist boss)
    {
        boss.anim.SetBool("Run", false);
    }
}
