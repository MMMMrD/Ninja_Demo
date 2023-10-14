using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recovery : UseableFunction
{
    public override void UseableAction(UseableData_SO useableData)
    {
        GameManager.Instance.player.characterStats.CurrentHealth = 
        Mathf.Clamp(GameManager.Instance.player.characterStats.CurrentHealth + useableData.healthPoint, 
        useableData.healthPoint, GameManager.Instance.player.characterStats.MaxHealth);

    }
}
