using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Attack Data", menuName = "Attack Stats/attack Data")]
public class AttackData_SO : ScriptableObject
{
    public int baseDefence; //基础防御力
    public int currentDefence;  //当前防御力
    public int baseDamage;  //基础攻击力
    public int currentDamage;   //当前攻击力
    public float criticalMultiple;  //暴击加成
    public float criticalChance;    //暴击率
    public float attackDistance;    //攻击距离
    public float skillDistance;     //技能距离

    public void ApplyEquipData(AttackData_SO Equip) //装上装备
    {
        currentDamage += Equip.currentDamage;
        currentDefence += Equip.currentDefence;
        criticalChance += Equip.criticalChance;
        criticalMultiple += Equip.criticalMultiple;
    }

    public void RemoveEquipData(AttackData_SO Equip)    //卸下装备
    {
        currentDamage -= Equip.currentDamage;
        currentDefence -= Equip.currentDefence;
        criticalChance -= Equip.criticalChance;
        criticalMultiple -= Equip.criticalMultiple;
    }
}
