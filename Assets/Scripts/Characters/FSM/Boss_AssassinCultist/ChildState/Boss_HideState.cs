using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_HideState : Boss_AssassinCultist_ChildState
{
    float random;   //随机数
    public override void EnterState(Boss_AssassinCultist boss)
    {
        if(boss.attackDashShadow != null)
        {
            AttackDashShadow attackDashShadow = ObjectPool.Instance.GetObjectButNoSetActive(boss.attackDashShadow).GetComponent<AttackDashShadow>();
            boss.attackDashShadows.Add(attackDashShadow);
            attackDashShadow.Init(boss.GetComponent<SpriteRenderer>(), GameManager.Instance.player.transform, boss.transform, boss.characterStats, true, boss.attackDashShadows);
            attackDashShadow.gameObject.SetActive(true);
        }

        boss.lastskill = Time.time;
        random = Random.Range(0, 10);
        boss.anim.SetTrigger("Hide");
    }

    public override void UpdateState(Boss_AssassinCultist boss)
    {
        if(boss.attackDashShadows.Count >= 3)
        {
            if(random <= 4f)
            {
                boss.currentParentState.TransitionChildState(boss.currentParentState.attackDashState, boss);
            }
        }

        if(!boss.anim.GetCurrentAnimatorStateInfo(3).IsName("Hide"))
        {   
            if(GameManager.Instance.player.isGround)    //判断如果是Player而且在地板上
            {
                if(random <= 4)
                {
                    //转入现形攻击状态
                    ExitState(boss);
                    boss.currentParentState.TransitionChildState(boss.currentParentState.appearAttackState, boss);
                }
                else
                {
                    //转入显性状态
                    ExitState(boss);
                    boss.currentParentState.TransitionChildState(boss.currentParentState.appearState, boss);
                }
                return;
            }

            if((Time.time - boss.lastskill) >= 5f)
            {
                //转入现形状态
                ExitState(boss);
                boss.currentParentState.TransitionChildState(boss.currentParentState.appearState, boss);
            }
        }
    }

    public override void ExitState(Boss_AssassinCultist boss)
    {

    }
}
