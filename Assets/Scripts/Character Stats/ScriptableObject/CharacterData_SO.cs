using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Base Data", menuName = "charactor Stats/Data")]
public class CharacterData_SO : ScriptableObject
{
    [Header("Base Stats Info")]
    public int maxHealth;   //角色最大生命值
    public int currentHealth;   //角色当前生命值
    public int baseSpeed;    //角色初始移动速度
    public int currentSpeed;    //角色当前速度
    public int baseJumpForce;    //角色初始跳跃力
    public int currentJumpForce;    //角色当前跳跃力

    [Header("Exp Info")]
    public int maxExp;     //当前等级最大经验值
    public int currentExp;  //当前经验值
    public int killExp; //击杀该角色获得的经验值

    [Header("Level Info")]
    public int maxLevel;    //最高等级
    public int currentLevel;    //当前等级
    public float development;     //升级时各项数据需要提升的数值
    public float developMultiple    //计算出每次升级需要乘上的值
    {
        get{ return 1 + (currentLevel - 1) * development;}
    }

    [Header("Money Info")]
    public int money;

    //提升经验值，由CharacterStats类调用
    public void UpdateExp(int getPoint)
    {
        currentExp += getPoint;
        if(currentExp >= maxExp)
        {
            LevelUp();
        }
    }

    //升级！
    void LevelUp()
    {
        //等级 +1 （在一定范围内）
        currentLevel = Mathf.Clamp(currentLevel + 1, 1, maxLevel);
        
        //经验值重置
        currentExp -= maxExp;
        maxExp = (int)(maxExp*developMultiple);

        //生命值重置
        maxHealth = (int)(maxHealth*developMultiple);
        currentHealth = maxHealth;
    }
}
