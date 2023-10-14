using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goddess_Dialogue : DialogueController
{
    public GameObject recovery_FX; //给角色回复特效
    private bool haveRecovery = false;  //判断是否已经恢复过

    public override void DoWhenGetKeyDown()
    {
        if(!haveRecovery)
        {
            haveRecovery = true;
            StartCoroutine(Recovery());
        }
    }

    IEnumerator Recovery()
    {
        recovery_FX.SetActive(true);

        CharacterStats playerCharacterStats = GameManager.Instance.player.characterStats;
        playerCharacterStats.CurrentHealth = playerCharacterStats.MaxHealth;

        //保存角色数据以及角色位置
        SaveManager.Instance.Save();
        SaveManager.Instance.SaveTransform();
        
        while(recovery_FX.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Recovery_FX"))
        {
            yield return null;
        }

        recovery_FX.SetActive(false);
    }
}
