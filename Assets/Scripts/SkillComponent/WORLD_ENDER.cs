using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum SkillState
{
    Wait,
    Action,
}

public class WORLD_ENDER : MonoBehaviour
{
    [Header("基础组件")]
    Rigidbody2D rb;
    PlayerController playerController;

    [Header("基础属性")]
    float skillStartTime = -20f;
    SkillState world_Ender_State = SkillState.Wait;

    void OnEnable()
    {
        Init();

        int num = UseableManager.Instance.skillNumber["WORLD_ENDER"];

        switch(num)
        {
            case 0:
                GameManager.Instance.player.skillQ += World_Ender;
                break;
            case 1:
                GameManager.Instance.player.skillE += World_Ender;
                break;
            case 2:
                GameManager.Instance.player.skillU += World_Ender;
                break;
            case 3:
                GameManager.Instance.player.skillI += World_Ender;
                break;
        }
        // StartCoroutine(World_Ender());
    }

    void Init()
    {
        rb = GetComponent<Rigidbody2D>();
        playerController = GetComponent<PlayerController>();
    }

    void World_Ender(UseableData_SO useableData)
    {
        if(world_Ender_State == SkillState.Wait && Time.time >= skillStartTime + useableData.skillCoolDown)
            StartCoroutine(World_Ender_Funtion(useableData));
    }

    IEnumerator World_Ender_Funtion(UseableData_SO useableData)
    {

        skillStartTime = Time.time;
        world_Ender_State = SkillState.Action;
        //开启形态
        FindObjectOfType<PlayerController>().GetComponent<SpriteRenderer>().color = Color.red;

        while(Time.time <= skillStartTime + useableData.skillDuration)
        {
            Debug.Log(">>>"); 
            yield return null;
        }

        FindObjectOfType<PlayerController>().GetComponent<SpriteRenderer>().color = Color.white;
        //关闭形态
        world_Ender_State = SkillState.Wait;

        yield return null;
    }
}
