using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AttackDashShadow : MonoBehaviour
{
    public float speed;
    bool canMove = false;
    bool canBreak;
    bool user_SameWith_Target = false;
    private string user;
    [HideInInspector]public bool canClose;
    Color color;
    Transform targetPoint;
    Transform userPoint;
    public GameObject shadow;
    SpriteRenderer spriteRenderer;
    SpriteRenderer userSprite;
    CharacterStats characterStats;
    List<AttackDashShadow> DashShadows = new List<AttackDashShadow>();
    // CharacterStats userCharaterState = null;
    private void OnEnable()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        characterStats = GetComponent<CharacterStats>();

        if(userSprite != null)
        {
            spriteRenderer.sprite = userSprite.sprite;
            spriteRenderer.flipX = userSprite.flipX;
        }
        

        // if(targetPoint != null)
        // {
        //     transform.position = targetPoint.position;
        //     transform.localScale = targetPoint.localScale;
        // }

        if(userPoint != null)
        {
            transform.position = userPoint.position;
            transform.localScale = userPoint.localScale;
        }
    }

    void Update()
    {
        if(canMove)
        {
            if(targetPoint.position == transform.position)
            {
                ReturnObjectPool();
            }

            transform.position = Vector2.MoveTowards(transform.position, targetPoint.position, speed * Time.deltaTime);
            GameObject _object = ObjectPool.Instance.GetObjectButNoSetActive(shadow);
            _object.GetComponent<ShadowSprite>().Init(transform, spriteRenderer);
            _object.SetActive(true);
        }
    }

    public void Init(SpriteRenderer targetSprite, Transform targetTransform, Transform userTransform, CharacterStats userCharacter, bool canBreak_or_Not, List<AttackDashShadow> attackDashShadows)
    {
        canBreak = canBreak_or_Not;
        if(userTransform == targetTransform)
        {
            user_SameWith_Target = true;
        }

        userSprite = targetSprite;
        targetPoint = targetTransform;
        user = userTransform.parent.name;
        userPoint = userTransform;
        // userCharaterState = userCharacter;
        characterStats.attackData = userCharacter.attackData;

        DashShadows = attackDashShadows;
    }

    public void MoveToTarget()
    {
        canMove = true;
        gameObject.AddComponent(Type.GetType("HitBox"));
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if(canClose)
        {
            if(user_SameWith_Target)
            {
                if(user == other.transform.parent.name)
                {
                    ReturnObjectPool();
                }
            }
            else
            {
                if(targetPoint.parent.name == other.transform.parent.name)
                {
                    ReturnObjectPool();
                }
            }
        }
    }

    public void BeAttack(CharacterStats userCharacterStats)
    {
        if(canBreak && (characterStats.attackData != userCharacterStats.attackData))
        {
            if(DashShadows != null)
            {
                if(DashShadows.Contains(this))
                {
                    DashShadows.Remove(this);
                }
            }

            ReturnObjectPool();
        }
    }

    void ReturnObjectPool()
    {
        canClose = false;
        canMove = false;
        canBreak = false;
        user_SameWith_Target = false;
        Destroy(gameObject.GetComponent<HitBox>());
        ObjectPool.Instance.PushObject(gameObject);
    }
}
