using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackDash : MonoBehaviour
{
    Rigidbody2D rb;
    Collider2D coll;
    SpriteRenderer sprite;

    float dir;  //记录角色面朝方向
    float dashStartTime;
    float dashEndTime;
    float speedMultiple = 6f;
    public bool isAttackDash;
    Vector3 originPosition;
    public GameObject attackDashShadow;

    void InitComponent()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
        sprite = GetComponent<SpriteRenderer>();
        attackDashShadow = GameManager.Instance.player.attackDashShadow;
    }

    private void OnEnable() 
    {
        int num = UseableManager.Instance.skillNumber["AttackDash"];
        switch(num)
        {
            case 0:
                GameManager.Instance.player.skillQ += PlayerAttackDash;
                break;
            case 1:
                GameManager.Instance.player.skillE += PlayerAttackDash;
                break;
            case 2:
                GameManager.Instance.player.skillU += PlayerAttackDash;
                break;
            case 3:
                GameManager.Instance.player.skillI += PlayerAttackDash;
                break;
        }

        InitComponent();
    }

    void Update()
    {
        if(isAttackDash)
        {
            GameObject playerShadow = ObjectPool.Instance.GetObjectButNoSetActive(GameManager.Instance.player.shadow);
            playerShadow.GetComponent<ShadowSprite>().Init(transform, sprite);
            playerShadow.SetActive(true);
        }
    }

    void PlayerAttackDash(UseableData_SO useableData)
    {
        originPosition = transform.position;
        StartCoroutine(AttackDashFuntion(useableData));
        
    }

    IEnumerator AttackDashFuntion(UseableData_SO useableData)
    {
        if(Time.time >= useableData.skillCoolDown + dashEndTime)
        {
            GameManager.Instance.player.applyMove = true;
            GameManager.Instance.player.canBeHit = false;

            GameObject _object = null;
            if(attackDashShadow != null)
            {
                _object = ObjectPool.Instance.GetObjectButNoSetActive(attackDashShadow);
                _object.GetComponent<AttackDashShadow>().Init(sprite, GameManager.Instance.player.transform, 
                   GameManager.Instance.player.transform , GameManager.Instance.player.characterStats, false, null);
                _object.SetActive(true);
            }

            isAttackDash = true;
            dir = GameManager.Instance.player.transform.localScale.x;
            // rb.constraints = RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionY;
            rb.velocity = new Vector2(-dir * GameManager.Instance.player.characterStats.CurrentSpeed * speedMultiple, rb.velocity.y);
            dashStartTime = Time.time;

            while((Time.time < dashStartTime + useableData.skillDuration) && !GameManager.Instance.player.isTouchingWall)
            {

                yield return null;
            }
            
            if(_object != null)
            {
                AttackDashShadow attackDashShadow = _object.GetComponent<AttackDashShadow>();
                attackDashShadow.canClose = true;
                attackDashShadow.MoveToTarget();
            }
            
            isAttackDash = false;
            GameManager.Instance.player.applyMove = false;
            GameManager.Instance.player.canBeHit = true;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            dashEndTime = Time.time;
        }
    }
}
