using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UseableType { Recovery, Attack, Skill }

[CreateAssetMenu(fileName = "New Useable Data", menuName = "Inventory/Useable Data")]
public class UseableData_SO : ScriptableObject
{
    public UseableType useableType;

    [Header("Recovery Data")]
    public int healthPoint;     //生命回复量

    [Header("Attack Data")]
    public float duration;     //持续时间
    public int damagePoint;    //攻击力提高点数
    public int defencePoint;   //攻击力提高点数
    public int speedPoint;    //速度提高点数
    public float criticalMultiplePoint;  //暴击加成提高点数
    public float criticalChancePoint;    //暴击率提高点数

    [Header("Skill")]
    public string skillName;    //技能名字
    public float skillCoolDown; //技能冷却时间
    public float skillDuration; //技能持续时间
}
