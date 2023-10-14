using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour, PortalEnvironment
{
    [Header("Component")]
    protected Animator anim;
    private SpriteRenderer spriteRenderer;
    [HideInInspector]public CharacterStats characterStats;

    [Header("Base Data")]
    public float baseSpeed; //移动速度
    [HideInInspector]public float currentSpeed; //当前速度
    [HideInInspector]public int damage; //伤害
    public float stayTime;   //物体可以存在时间
    [HideInInspector]public float startTime; //物体出现时间

    Transform targetTransform;

    [Header("Shadow")]
    public GameObject shadow;
    

    public virtual void OnEnable() 
    {
        currentSpeed = baseSpeed;
        startTime = Time.time;

        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        characterStats = GetComponent<CharacterStats>();
    }

    public virtual void Update()
    {
        if(Time.time >= startTime + stayTime)
        {
            Disappear();
        }

        Movement();
    }

    public virtual void FixedUpdate()
    {
        
    }

    public virtual void Movement() //移动函数
    {
        
        transform.position += currentSpeed * transform.up * Time.deltaTime;
        GameObject _object = ObjectPool.Instance.GetObjectButNoSetActive(shadow);
        _object.GetComponent<ShadowSprite>().Init(transform, spriteRenderer);
        _object.SetActive(true);
    }

    public virtual void Init(Transform userDiraction, CharacterStats attacker)  //初始化函数
    {
        characterStats = attacker;
        transform.rotation = userDiraction.rotation;
    }

    public virtual void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.CompareTag("Player"))
        {
            GameManager.Instance.player.characterStats.TakeDamage(characterStats);
        }
    }

    public void InitEnvironment(Vector2 point, Transform userTransform, Transform targetTransform, CharacterStats attacker)
    {
        transform.position = point;
        characterStats.attackData = attacker.attackData;
        transform.right = -userTransform.up;
        this.targetTransform = targetTransform;
        
        gameObject.SetActive(true);
    }

    //Animation Event
    public virtual void Disappear()  //动画结束，物体消失
    {
        ObjectPool.Instance.PushObject(gameObject);
    }
}
