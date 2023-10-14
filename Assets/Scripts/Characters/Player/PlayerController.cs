using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

public class PlayerController : MonoBehaviour,IDamageable
{
    [Header("Component")]
    private Rigidbody2D rb;
    private Collider2D coll;
    private PlayerAnimation playerAnimation;    //玩家动画信息，必须要有，以后可能外部需要了解玩家动画进程
    [HideInInspector]public CharacterStats characterStats;
    
    [Header("check Data")]
    public LayerMask Ground;    //选择地板图层
    public float footPoint; //两脚位置
    public float bodyPoint;
    public float jumpOffLenth;  //跳跃检测距离
    [HideInInspector]public bool isGround;   //用于判断是否在地板上
    public bool isTouchingWall;    //用于判断是否触碰墙体
    [HideInInspector]public bool canTouchWall;
    [HideInInspector]public float verticalInput; //用于获得垂直输入（w & s）
    [HideInInspector]public float horizontalInput;  //用于获得水平输入（a & d）

    [Header("Move Data")]
    public bool isMove; //判断角色是否在移动，保证角色动画机正常运行
    public bool applyMove;  //外部申请控制角色移动时，将该值设置为true，保证角色自身Move函数不会干扰外部控制
    
    [Header("Jump Data")]
    private bool applyJump; //申请跳跃
    public int jumpCount;   //跳跃次数，初始值为1
    [HideInInspector]public int baseJumpCount;  //初始跳跃次数
    [HideInInspector]public bool isJump;    //用于动画器判断是否跳跃

    [Header("Attack Data")]
    private bool applyAttack;   //申请攻击
    private float attackTime;   //记录攻击时的时间
    [HideInInspector]public bool isAttack;  //用于判断是否处于攻击状态
    [HideInInspector]public int attackCount = 1;    //用于判断攻击次数
    
    [Header("Skill Data")]
    public GameObject shadow;
    public GameObject attackDashShadow;
    public GameObject shield;

    [Header("Useable Data")]
    public Action action;
    public Action<UseableData_SO> skillQ;
    public Action<UseableData_SO> skillE;
    public Action<UseableData_SO> skillU;
    public Action<UseableData_SO> skillI;

    ///////////////////////////////////////////////////////
    //////////////////用于判断是那个攻击方式/////////////////
    ///////////////////////////////////////////////////////
    #region Attack Data
    [HideInInspector]public bool attack1;
    [HideInInspector]public bool attack2;
    [HideInInspector]public bool attack3;
    [HideInInspector]public bool airAttack1;
    [HideInInspector]public bool airAttack2;
    [HideInInspector]public bool airAttack3;
    [HideInInspector]public bool attackDown;
    #endregion
    ///////////////////////////////////////////////////////
    //////////////////用于判断是那个攻击方式/////////////////
    ///////////////////////////////////////////////////////

    [Header("GetHit Data")]
    public bool isDead; //用于判断是否死亡
    public bool isHit; //用于判断是否在受伤状态
    public bool canBeHit;   //用于判断是否可以受伤


    //public event Action<CharacterStats> GetHitAction;

    private void OnEnable() 
    {
        GameManager.Instance.RegisterPlayer(this);
        //EventManager.Instance?.InvokeEvent(UI.UpdateMoney);
        //InventoryManager.Instance.RefreshAllUI();
    }

    void Awake()
    {
        InitComponent();
        GameManager.Instance.RegisterPlayer(this);
    }

    void Start() 
    {
        EventManager.Instance?.InvokeEvent<UI>(UI.UpdateMoney);
        baseJumpCount = jumpCount;
    }

    void Update() 
    {
        if(isDead)
        {
            // if(rb.velocity.x != 0)
            // {
            //     rb.velocity = new Vector2(0f, 0f);
            // }
            return;
        }

        CheckInput();
        OnDrawRay();
    }

    void FixedUpdate()
    {
        if(isDead) return;
        if(applyMove) return;
        PhysicsCheck();
        PhysicsChange();
        Movement();
        Jump();
        Attack();
    }

    void InitComponent()    //初始化组件
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
        playerAnimation = GetComponent<PlayerAnimation>();
        characterStats = GetComponent<CharacterStats>();
    }

    void CheckInput()   //检测输入
    {
        if(playerAnimation.GetCurrentAnimationInfo(2, "GetHit")) return;

        if(Input.GetButtonDown("Jump") && jumpCount != 0)
        {
            applyJump = true;
        }
        
        if(Input.GetKeyDown(KeyCode.J))
        {
            applyAttack = true;
        }
    }

    void PhysicsCheck() //物理状态检测
    {
        if(isGround)    //检测是否在地面
        {
            jumpCount = baseJumpCount;
            isJump = false;
            canTouchWall = false;
        }

        if(Time.time >= attackTime + 1) //重置普攻次数
        {
            attackCount = 1;
        }
    }

    void Movement()
    {
        //获取角色水平移动方向
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if(isAttack) return;
        if(applyMove) return;
        if(playerAnimation.GetCurrentAnimationInfo(3, "GetHit")) return;
        //根据方向控制角色移动
        rb.velocity = new Vector2(characterStats.CurrentSpeed* horizontalInput, rb.velocity.y);

        //根据方向更改角色图片方向
        if(horizontalInput != 0)
        {
            isMove = true;
            transform.localScale = new Vector3(-horizontalInput, 1, 1);
        }
        else
        {
            isMove = false;
        }
    }

    void SetTouchBool()
    {
        canTouchWall = true;
    }

    void Jump() //跳跃函数
    {
        if(applyJump)
        {
            applyJump = false;
            if(!isAttack)
            {
                if(isGround || jumpCount > 0)
                {
                    isJump = true;
                    attackCount = 1;
                    rb.velocity = new Vector2(rb.velocity.x, characterStats.CurrentJumpForce);
                    jumpCount--;
                }
            }
        }
    }

    void Attack()   //攻击函数
    {
        if(applyAttack)
        {
            applyAttack = false;
            if(verticalInput != 0 & !isAttack)  //判断玩家是否是希望上下攻击
            {
                if(!isGround)
                {
                    attackDown = true;
                    isAttack = true;
                    return;
                }
            }

            if(!isAttack)
            {
                if(isGround)
                {
                    switch(attackCount)
                    {
                        case 1: attack1 = true;attackTime = Time.time;attackCount++;break;
                        case 2: attack2 = true;attackTime = Time.time;attackCount++;break;
                        case 3: attack3 = true;attackTime = Time.time;attackCount = 1;break;
                    }
                    isAttack = true;
                }
                else if(!isGround)
                {
                    switch(attackCount)
                    {
                        case 1: airAttack1 = true;attackTime = Time.time;attackCount++;break;
                        case 2: airAttack2 = true;attackTime = Time.time;attackCount++;break;
                        case 3: airAttack3 = true;attackTime = Time.time;attackCount = 1;break;
                    }
                    isAttack = true;
                }
            }
        }
    }

    void PhysicsChange()    //判断角色物理状态并根据状态更改相应数据
    {
        if(isAttack && isGround)
        {
            rb.velocity = new Vector2(characterStats.CurrentSpeed * -transform.localScale.x * 0.4f, rb.velocity.y);
        }
        
        if(isAttack && !isGround)
        {
            rb.velocity = new Vector2(characterStats.CurrentSpeed * -transform.localScale.x * 0.4f, rb.velocity.y);
            rb.constraints = RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionY;
        }
        else
        {
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        }
    }

    public void GetHit(CharacterStats attacker)
    {
        // if(!canBeHit) return;

        //更新Model层数据
        characterStats.TakeDamage(attacker);

        //使用事件总线触发UI事件
        EventManager.Instance?.InvokeEvent(UI.UpdatePlayerHealth, attacker);
        EventManager.Instance?.InvokeEvent(UI.UpdatePlayerHealth, (float)characterStats.CurrentHealth, (float)characterStats.MaxHealth);

        
        //Player Die
        if (characterStats.CurrentHealth <= 0)
        {
            isDead = true;
            coll.enabled = false;
            rb.constraints = RigidbodyConstraints2D.FreezeAll;


            GameManager.Instance.ResetGame();
        }
    }

    public void OnDrawRay()    //使用射线判断角色是否在地板上
    {
        RaycastHit2D leftGroundCheck = Raycast(new Vector2(-footPoint, -1.05f), Vector2.down, jumpOffLenth, Ground);
        RaycastHit2D rightGroundCheck = Raycast(new Vector2(footPoint, -1.05f), Vector2.down, jumpOffLenth, Ground);
        RaycastHit2D leftCheck = Raycast(new Vector2(-bodyPoint, 0f), Vector2.left, 0.1f, Ground);
        RaycastHit2D rightCheck = Raycast(new Vector2(bodyPoint, 0f), Vector2.right, 0.1f, Ground);

        if(leftGroundCheck || rightGroundCheck)
        {
            isGround = true;
        }
        else
        {
            isGround = false;
        }

        if(leftCheck || rightCheck)
        {
            isTouchingWall = true;
        }
        else
        {
            isTouchingWall = false;
        }
    }

    RaycastHit2D Raycast(Vector2 offset, Vector2 rayDirection, float Lenth, LayerMask layerMask)    //保证射线发射点能够一直在角色身上
    {
        Vector2 pos = transform.position;
        RaycastHit2D hit = Physics2D.Raycast(pos + offset, rayDirection, Lenth, layerMask);
        Debug.DrawRay(pos + offset, rayDirection*Lenth);

        return hit;
    }
}
