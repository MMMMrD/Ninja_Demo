using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : UseableFunction
{
    public override void UseableAction(UseableData_SO useableData)
    {
        if(Type.GetType(useableData.skillName) != null)
        {
            if(GameManager.Instance.player != null)
            {
                GameManager.Instance.player.gameObject.AddComponent(Type.GetType(useableData.skillName));
            }
        }
    }
}
