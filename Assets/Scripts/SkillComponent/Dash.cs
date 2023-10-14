using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dash : MonoBehaviour
{
    Rigidbody2D rb;
    Collider2D coll;
    PlayerController player;
    SpriteRenderer spriteRenderer;

    float dir;  //记录角色面朝方向
    public float speedMultiple = 4f;    //记录速度倍数
    float dashStartTime;      //冲刺开始时间
    float dashEndTime;      //冲刺结束时间
    bool isDash;    //判断是否在冲刺

    void OnEnable() 
    {
        int num = UseableManager.Instance.skillNumber["Dash"];
        switch(num)
        {
            case 0:
                GameManager.Instance.player.skillQ += PlayerDash;
                break;
            case 1:
                GameManager.Instance.player.skillE += PlayerDash;
                break;
            case 2:
                GameManager.Instance.player.skillU += PlayerDash;
                break;
            case 3:
                GameManager.Instance.player.skillI += PlayerDash;
                break;
        }

        InitComponent();
    }

    void Update()
    {
        if(isDash)
        {
            ShadowSprite shadowSprite = ObjectPool.Instance.GetObjectButNoSetActive(GameManager.Instance.player.shadow).GetComponent<ShadowSprite>();
            shadowSprite.Init(transform, spriteRenderer);
            shadowSprite.gameObject.SetActive(true);
        }
    }

    void InitComponent()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
        player = GetComponent<PlayerController>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void PlayerDash(UseableData_SO useableData) //按下对应案件后执行该函数
    {
        if(!isDash)
        {
            isDash = true;
            StartCoroutine(DashFunction(useableData));
        }
    }

    IEnumerator  DashFunction(UseableData_SO useableData)
    {
        if(Time.time >= useableData.skillCoolDown + dashEndTime)
        {
            GameManager.Instance.player.applyMove = true;
            GameManager.Instance.player.canBeHit = false;
            dir = GameManager.Instance.player.transform.localScale.x;
            // rb.constraints = RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionY;
            rb.velocity = new Vector2(-dir * GameManager.Instance.player.characterStats.CurrentSpeed * speedMultiple, rb.velocity.y);
            dashStartTime = Time.time;

            while((Time.time < dashStartTime + useableData.skillDuration) && !GameManager.Instance.player.isTouchingWall)
            {
                yield return null;
            }
            
            GameManager.Instance.player.applyMove = false;
            GameManager.Instance.player.canBeHit = true;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            dashEndTime = Time.time;
        }
        isDash = false;
    }

    private void OnDisable() 
    {
        int num = UseableManager.Instance.skillNumber["Dash"];
        switch(num)
        {
            case 0:
                GameManager.Instance.player.skillQ -= PlayerDash;
                break;
            case 1:
                GameManager.Instance.player.skillE -= PlayerDash;
                break;
            case 2:
                GameManager.Instance.player.skillU -= PlayerDash;
                break;
            case 3:
                GameManager.Instance.player.skillI -= PlayerDash;
                break;
        }
    }
}
