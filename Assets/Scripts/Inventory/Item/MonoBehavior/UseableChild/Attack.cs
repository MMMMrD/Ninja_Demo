using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : UseableFunction
{
    float startTime;    //记录开始使用时间
    float test;
    public override void UseableAction(UseableData_SO useableData)
    {
        StartCoroutine(Wait(useableData));
    }

    void UseableStart(UseableData_SO useableData)
    {
        GameManager.Instance.player.characterStats.CurrentDamage += useableData.damagePoint;
        GameManager.Instance.player.characterStats.CurrentSpeed += useableData.speedPoint;
        GameManager.Instance.player.characterStats.CurrentDefence += useableData.defencePoint;
        GameManager.Instance.player.characterStats.CriticalChance += useableData.criticalChancePoint;
        GameManager.Instance.player.characterStats.CriticalMultiple += useableData.damagePoint;
        startTime = Time.time;
    }

    void UseableRemove(UseableData_SO useableData)
    {
        GameManager.Instance.player.characterStats.CurrentDamage -= useableData.damagePoint;
        GameManager.Instance.player.characterStats.CurrentSpeed -= useableData.speedPoint;
        GameManager.Instance.player.characterStats.CurrentDefence -= useableData.defencePoint;
        GameManager.Instance.player.characterStats.CriticalChance -= useableData.criticalChancePoint;
        GameManager.Instance.player.characterStats.CriticalMultiple -= useableData.damagePoint;
    }

    IEnumerator Wait(UseableData_SO useableData)
    {
        UseableStart(useableData);

        while(Time.time < useableData.duration + startTime)
        {
            yield return null;
        }

        UseableRemove(useableData);
    }
}
