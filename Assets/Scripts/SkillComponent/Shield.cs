using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    float uesShieldTime;
    public GameObject shield;
    void OnEnable() 
    {
        InitComponent();

        int num = UseableManager.Instance.skillNumber["Shield"];
        switch(num)
        {
            case 0:
                GameManager.Instance.player.skillQ += GetShield;
                break;
            case 1:
                GameManager.Instance.player.skillE += GetShield;
                break;
            case 2:
                GameManager.Instance.player.skillU += GetShield;
                break;
            case 3:
                GameManager.Instance.player.skillI += GetShield;
                break;
        }
    }

    void InitComponent()
    {
        shield = GameManager.Instance.player.shield;
    }

    void GetShield(UseableData_SO useableData)
    {
        if(Time.time >= useableData.skillCoolDown + uesShieldTime)
        {
            uesShieldTime = Time.time;
            GameObject _object = ObjectPool.Instance.GetObjectButNoSetActive(shield);
            _object.transform.position = GameManager.Instance.player.transform.position;
            _object.SetActive(true);
        }
    }
}
