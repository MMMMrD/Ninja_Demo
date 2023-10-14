using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseableManager : Singleton<UseableManager>
{
    Recovery recovery;
    Attack attack;
    Skill skill;
    public Dictionary<UseableType, UseableFunction> useableFunction = new Dictionary<UseableType, UseableFunction>(); //根据类型Useable类型执行相应方法
    public Dictionary<string, int> skillNumber = new Dictionary<string, int>();

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
    }

    void Start()
    {
        recovery = GetComponent<Recovery>();
        attack = GetComponent<Attack>();
        skill = GetComponent<Skill>();

        useableFunction.Add(UseableType.Recovery, recovery);
        useableFunction.Add(UseableType.Attack, attack);
        useableFunction.Add(UseableType.Skill, skill);
    }
}
