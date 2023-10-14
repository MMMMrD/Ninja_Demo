using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssassinCultist : Boss
{
    bool isDush;  //判断是否在冲刺状态
    bool spriteRendererActive;  //用于更改SpriteRender开关状态
    public GameObject shadow;
    SpriteRenderer spriteRenderer;
    
    public override void Start()
    {
        base.Start();
        spriteRenderer = GetComponent<SpriteRenderer>();
        TransitionState(patrolParentState);
    }

    public override void Update()
    {
        base.Update();
        anim.SetBool("IsAttack", isAttack);
        if(isDush)
        {
            ShadowSprite shadowSprite = ObjectPool.Instance.GetObjectButNoSetActive(shadow).GetComponent<ShadowSprite>();
            shadowSprite.Init(transform, spriteRenderer);
            shadowSprite.gameObject.SetActive(true);
        }
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
        OnDrawRay();
    }

    public override void Movement()
    {
        if(isDush) return;

        base.Movement();
    }

    public void OnDrawRay()    
    {
        RaycastHit2D leftCheck = Raycast(new Vector2(-1f, 0), Vector2.left, 0.2f, playerMask);
        RaycastHit2D rightCheck = Raycast(new Vector2(1f, 0), Vector2.right, 0.2f, playerMask);

        if(leftCheck || rightCheck)
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
        }
    }

    RaycastHit2D Raycast(Vector2 startPoint, Vector2 rayDirection, float Lenth, LayerMask layerMask)    //射线
    {
        Vector2 pos = transform.position;
        RaycastHit2D hit = Physics2D.Raycast(pos + startPoint, rayDirection, Lenth, layerMask);
        Debug.DrawRay(pos + startPoint, rayDirection*Lenth);
        
        return hit;
    }

    #region Animation Event

    public void AttackDush()    //攻击冲刺
    {
        int dir = 0;
        dir = TurnAround(dir);

        if(!isDush)
        {
            speedMultiple = 5f;
            rb.velocity = new Vector2(dir * characterStats.CurrentSpeed * speedMultiple, rb.velocity.y);
        }
        else
        {
            speedMultiple = baseSpeedMultiple;
            rb.velocity = new Vector2(0, rb.velocity.y);
        }

        isDush = !isDush;
    }

    public void SetSpriteRender() //更改图片可见状态
    {
        spriteRenderer.enabled  = spriteRendererActive;
        spriteRendererActive = !spriteRendererActive;
    }

    public void SetSpriteRenderTrue()
    {
        spriteRenderer.enabled = true;
    }

    #endregion
}
