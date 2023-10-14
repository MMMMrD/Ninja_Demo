using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowSprite : MonoBehaviour
{
    Transform user;

    SpriteRenderer thisSprite;
    SpriteRenderer userSprite;

    [Header("Time Data")]
    public float activeTime;    //影子持续时间
    private float activeStartTime;   //分身开始时间

    [Header("Sprite Data")]
    private Color color; //控制图片颜色
    private float alpha; //图片透明度
    public float alphaSet;  //图片透明度初始值
    public float alphaMultiplier;   //透明度减少程度

    private void OnEnable() 
    {
        thisSprite = GetComponent<SpriteRenderer>();

        alpha = alphaSet;

        if(user == null) return;

        thisSprite.sprite = userSprite.sprite;
        thisSprite.flipX = userSprite.flipX;

        transform.position = user.position;
        transform.rotation = user.rotation;
        transform.localScale = user.localScale;
        
        activeStartTime = Time.time;
    }
    
    public void Init(Transform userTransform, SpriteRenderer spriteRenderer)
    {
        user = userTransform;
        userSprite = spriteRenderer;
    }

    void Update() 
    {
        alpha *= alphaMultiplier;
        color = new Color(0.5f, 0.5f, 1f, alpha);

        thisSprite.color = color;

        if(Time.time >= activeStartTime + activeTime)
        {
            ObjectPool.Instance.PushObject(gameObject);
        }
    }
}
