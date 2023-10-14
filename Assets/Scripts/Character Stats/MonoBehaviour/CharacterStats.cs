using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//角色数值系统
public class CharacterStats : MonoBehaviour
{
    public CharacterData_SO tampletData;    //基础数值模板
    public CharacterData_SO characterData; //角色当前基础数值
    public AttackData_SO tampletAttackData;
    public AttackData_SO attackData;
    [HideInInspector]public bool isCritical;    //用于判断是否暴击 
    void Awake() 
    {
        //初始化，根据数值模板生成一个可更改的数值
        if(tampletData != null)
        {
            characterData = Instantiate(tampletData);
        }

        if(tampletAttackData != null)
        {
            attackData = Instantiate(tampletAttackData);
        }
    }

    #region get Info form CharacterData_So
    public int MaxHealth    //最大生命值
    {
        get{if(characterData != null){return characterData.maxHealth;} else return 0;}

        set{characterData.maxHealth = value;}
    }

    public int CurrentHealth    //当前生命值
    {
        get{if(characterData != null){return characterData.currentHealth;} else return 0;}

        set{characterData.currentHealth = value;}
    }

    public int BaseSpeed    //基础移动速度
    {
        get{if(characterData != null){return characterData.baseSpeed;} else return 0;}

        set{characterData.baseSpeed = value;}
    }

    public int CurrentSpeed //当前移动速度
    {
        get{if(characterData != null){return characterData.currentSpeed;} else return 0;}

        set{characterData.currentSpeed = value;}
    }

    public int BaseJumpForce    //基础跳跃力
    {
        get{if(characterData != null){return characterData.baseJumpForce;} else return 0;}

        set{characterData.baseJumpForce = value;}
    }

    public int CurrentJumpForce //当前跳跃力
    {
        get{if(characterData != null){return characterData.currentJumpForce;} else return 0;}

        set{characterData.currentJumpForce = value;}
    }

    public int Money    //当前金币数
    {
        get{if(characterData != null){return characterData.money;} else return 0;}

        set{characterData.money = value;}
    }
    
    #endregion

    #region get Info form AttackData_SO

    public int BaseDefence  //基础防御力
    {
        get{if(attackData != null){return attackData.baseDefence;} else return 0;}

        set{attackData.baseDefence = value;}
    }

    public int CurrentDefence   //当前防御力
    {
        get{if(attackData != null){return attackData.currentDefence;} else return 0;}

        set{attackData.currentDefence = value;}
    }

    public int BaseDamage   //基础攻击力
    {
        get{if(attackData != null){return attackData.baseDamage;} else return 0;}

        set{attackData.baseDamage = value;}
    }

    public int CurrentDamage    //当前攻击力
    {
        get{if(attackData != null){return attackData.currentDamage;} else return 0;}

        set{attackData.currentDamage = value;}
    }

    public float CriticalChance //角色暴击率
    {
        get{if(attackData != null){return attackData.criticalChance;} else return 0;}

        set{attackData.criticalChance = value;}
    }

    public float CriticalMultiple
    {
        get{if(attackData != null){return attackData.criticalMultiple;} else return 0;}

        set{attackData.criticalMultiple = value;}
    }

    public float AttackDistance //攻击距离
    {
        get{if(attackData != null){return attackData.attackDistance;} else return 0;}

        set{attackData.attackDistance = value;}
    }

    public float SkillDistance  //技能距离
    {
        get{if(attackData != null){return attackData.skillDistance;} else return 0;}

        set{attackData.skillDistance = value;}
    }

    #endregion 

    
    //受伤函数，由受伤者自身调用
    //TODO：调用位置可能会更改,在这插个眼
    public void TakeDamage(CharacterStats attacker)
    {
        //造成伤害
        int damage = Mathf.Max(attacker.MakeDamage() - CurrentDefence, 1);
        CurrentHealth = Mathf.Max(CurrentHealth - damage, 0);

        //更改角色血条
        

        //生命值小于零时，将自身经验值给attack
        if(CurrentHealth <= 0)
        {
            if(attacker.characterData != null)
            {
                attacker.characterData.UpdateExp(characterData.killExp);
            }
        }
    }

    //造成伤害函数，受伤函数调用
    int MakeDamage()
    {
        if(isCritical)
        {
            //暴击情况
            isCritical = false;
            return (int)(CurrentDamage * (1 + attackData.criticalMultiple));
        }
        else
        {
            //不暴击情况
            return CurrentDamage;
        }
    }

    #region Equip Weapon

    public void GetEquip(ItemData_SO Equip)
    {
        //TODO:更新属性
        attackData.ApplyEquipData(Equip.EquipData);
    }

    public void RemoveEquipment(ItemData_SO Equip)
    {
        attackData.RemoveEquipData(Equip.EquipData);
    }

    #endregion
}
