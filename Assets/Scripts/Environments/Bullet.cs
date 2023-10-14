using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour, PortalEnvironment
{
    [Header("Component")]
    protected Animator anim;
    protected Rigidbody2D rb;
    [HideInInspector]public CharacterStats characterStats;

    [Header("Base Data")]
    public float baseSpeed; //移动速度
    [HideInInspector]public float currentSpeed; //当前速度
    [HideInInspector]public int damage; //伤害
    [HideInInspector]public float diraction;  //方向
    public float stayTime;   //物体可以存在时间
    [HideInInspector]public float startTime; //物体出现时间
    

    public virtual void OnEnable() 
    {
        currentSpeed = baseSpeed;
        startTime = Time.time;

        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        characterStats = GetComponent<CharacterStats>();
    }

    public virtual void Update()
    {
        if(Time.time >= startTime + stayTime)
        {
            Disappear();
        }
    }

    public virtual void FixedUpdate()
    {
        Movement();
    }

    public virtual void Movement() //移动函数
    {
        if(transform.rotation.y == 0)
        {
            rb.velocity = new Vector2(-currentSpeed, rb.velocity.y);
        }
        else
        {
            rb.velocity = new Vector2(currentSpeed, rb.velocity.y);
        }
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
            currentSpeed = 0;
            GameManager.Instance.player.characterStats.TakeDamage(characterStats);
            anim.SetTrigger("Attack");
        }

        if(other.CompareTag("Ground"))
        {
            currentSpeed = 0;
            anim.SetTrigger("Attack");
        }

        if(other.CompareTag("HitBox"))
        {
            currentSpeed = 0;
            anim.SetTrigger("Attack");
        }
    }

    public virtual void InitEnvironment(Vector2 point, Transform userTransform, Transform targetTransform, CharacterStats attacker)
    {
        transform.position = point;
        characterStats.attackData = attacker.attackData;
        
        gameObject.SetActive(true);
    }

    //Animation Event
    public virtual void Disappear()  //动画结束，物体消失
    {
        ObjectPool.Instance.PushObject(gameObject);
    }
}
