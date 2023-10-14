﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_IdleState : Boss_AssassinCultist_ChildState
{
    float time; //用于记录需要等待的时长
    float startTime;    //用于记录开始站立的时间点
    public override void EnterState(Boss_AssassinCultist boss)
    {
        time = Random.Range(1f, 1.5f);  //等待1-1.5秒钟
        startTime = Time.time;
        boss.SwitchTargetPoint();
    }

    public override void UpdateState(Boss_AssassinCultist boss)
    {
        if((Time.time - startTime) > time)
        {
            //等待时间结束，切换为移动状态
            boss.currentParentState.TransitionChildState(boss.currentParentState.runState, boss);
        }
    }

    public override void ExitState(Boss_AssassinCultist boss)
    {

    }
}
