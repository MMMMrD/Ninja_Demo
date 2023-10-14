using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_AssassinCultist : MonoBehaviour, IDamageable, GameOverReset
{
    [Header("Component")]
    [HideInInspector]public Animator anim;
    [HideInInspector]public Rigidbody2D rb;
    [HideInInspector]public Collider2D coll;
    [HideInInspector]public CharacterStats characterStats;   //记录Boss的各项数据

    [Header("Point Data")]
    
    //巡逻点
    public Transform pointA;
    public Transform pointB;

    //场景范围
    public Transform sceneLeftPoint;    //场景左端点
    public Transform sceneRightPoint;   //场景右端点

    [Header("Find target Data")]
    [HideInInspector]public Transform target;   //用于记录目标
    public List<Transform> targets = new List<Transform>(); //用于记录目标点的列表

    [Header("Attack Data")]
    [HideInInspector]public bool isAttack;  //判断是否在攻击状态
    [HideInInspector]public int attackCount = 1;    //判断攻击次数
    public float skillCoolDown; //技能冷却时间
    public float attackCoolDown; //攻击冷却时间
    public float skillAttackCoolDown; //特殊攻击冷却时间
    [HideInInspector]public float lastskill;  //用于记录上一次释放技能的时间
    [HideInInspector]public float lastAttack; //用于记录上一次攻击时间
    [HideInInspector]public float lastSkillAttack;    //用于记录上一次释放特殊攻击时间
    [HideInInspector]public float speedMultiple = 0.5f;    //速度倍数，进入攻击状态后更改
    [HideInInspector]public float baseSpeedMultiple;    //记录刚开始角色的速度倍数

    [Header(" 状态机")]
    public Boss_AssassinCultist_ParentState currentParentState;   //用于记录当前父类状态机

    public Boss_PatrolParentState patrolParentState = new Boss_PatrolParentState();
    public Boss_AttackParentState attackParentState = new Boss_AttackParentState();
    public Boss_AssassinHalfHealthParentState halfHealthParentState = new Boss_AssassinHalfHealthParentState();

    [Header("Boss_AssassinCultist")]
    public GameObject shadow;
    public GameObject attackDashShadow;
    bool isDush;  //判断是否在冲刺状态
    bool spriteRendererActive;  //用于更改SpriteRender开关状态
    SpriteRenderer spriteRenderer;
    public List<AttackDashShadow> attackDashShadows = new List<AttackDashShadow>();

    [Header("Check Data")]
    public LayerMask playerMask;    //选择Player所在的图层

    [Header("Get Hit Data")]
    [HideInInspector]public bool isDead = false;    //用于判断是否死亡

    public virtual void Start()
    {
        //初始化
        InitData();
        InitComponent();
        GameManager.Instance.RegisterEnemies(this);

        spriteRenderer = GetComponent<SpriteRenderer>();
        TransitionState(patrolParentState);
    }

    public void InitComponent() //初始化组件
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        coll = GetComponent<Collider2D>();
        characterStats = GetComponent<CharacterStats>();
    }

    public void InitData()  //某些参数的初始化
    {
        //将攻击于技能cd设置为冷却时间的负数，保证能够一开始就放出技能
        lastSkillAttack = -skillAttackCoolDown;
        lastAttack = -attackCoolDown;
        lastskill = -skillCoolDown;

        //记录初始的速度倍数
        baseSpeedMultiple = speedMultiple;
    }

    public virtual void Update()
    {
        anim.SetBool("Dead", isDead);
        if(isDead) return;
        if(anim.GetCurrentAnimatorStateInfo(2).IsName("GetHit")) return;
        currentParentState.UpdateState(this);

        anim.SetBool("IsAttack", isAttack);
        if(isDush)
        {
            ShadowSprite shadowSprite = ObjectPool.Instance.GetObjectButNoSetActive(shadow).GetComponent<ShadowSprite>();
            shadowSprite.Init(transform, spriteRenderer);
            shadowSprite.gameObject.SetActive(true);
        }
    }

    public virtual void FixedUpdate()
    {
        if(isDead) return;
        Attack();

        OnDrawRay();
    }

    public void Movement()
    {
        if(isDead) return;

        if(isDush) return;

        if(target != null)
        {
            transform.position = Vector2.MoveTowards(transform.position, new Vector2(target.position.x, transform.position.y), characterStats.CurrentSpeed * Time.deltaTime * speedMultiple);
        }
        TurnAround();
    }

    public void Attack()    //攻击方式
    {
        if((Time.time - lastAttack) <= attackCoolDown) return;

        if(isAttack)
        {
            AttackMode();
        }
    }

    public virtual void AttackMode()    //可以更改的攻击方式
    {
        anim.Play("Base Attack", 1);
        lastAttack = Time.time;
    }

    #region Tools Function

    public void TransitionState(Boss_AssassinCultist_ParentState state)//切换状态类的函数
    {
        currentParentState = state;
        currentParentState.EnterState(this);//进入状态需要执行的函数
    }

    public void SwitchTargetPoint() //转换默认目标点,由状态机调用
    {
        if(Mathf.Abs(pointA.position.x - transform.position.x) > Mathf.Abs(pointB.position.x - transform.position.x))
        {
            target = pointA;
        }
        else
        {
            target = pointB;
        }
    }

    public void TurnAround()    //转向函数，通过改变角色图像的旋转角度实现
    {
        if(transform.position.x < target.position.x)
        {
            transform.rotation = Quaternion.Euler(0f, 180f, 0f);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0f, 0f,0f);
        }
    }

    public int TurnAround(int dir)    //转向函数，通过改变角色图像的旋转角度实现,并返回角色的方向
    {
        if(transform.position.x < target.position.x)
        {
            dir = 1;
            transform.rotation = Quaternion.Euler(0f, 180f, 0f);
        }
        else
        {
            dir = -1;
            transform.rotation = Quaternion.Euler(0f, 0f,0f);
        }
        return dir;
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if(!targets.Contains(other.transform) && other.CompareTag("Player") && !GameManager.Instance.gameOver)
        {
            targets.Add(other.transform);
        }    
    }

    public void OnTriggerStay2D(Collider2D other) 
    {
        if(!targets.Contains(other.transform) && other.CompareTag("Player") && !GameManager.Instance.gameOver)
        {
            targets.Add(other.transform);
        }    
    }

    public void OnTriggerExit2D(Collider2D other) 
    {
        if(targets.Contains(other.transform))
        {
            targets.Remove(other.transform);
        }    
    }

    public void GetHit(CharacterStats attacker)    //受伤函数，由外部调用
    {
        if(!anim.GetCurrentAnimatorStateInfo(2).IsName("GetHit"))
        {
            anim.SetTrigger("GetHit");
            characterStats.TakeDamage(attacker);
            EventManager.Instance?.InvokeEvent(UI.UpdateBossHealth, characterStats.CurrentHealth, characterStats.MaxHealth);

            if(characterStats.CurrentHealth <= 0)
            {
                isDead = true;
                GameManager.Instance.enemies.Remove(this);
            }
        }
    }

    public void GameOverReset() //游戏结束时，重置
    {   
        if(target != null)
        {
            target = null;
            currentParentState.ExitState(this);
            TransitionState(patrolParentState);
        }
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

    #endregion

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
