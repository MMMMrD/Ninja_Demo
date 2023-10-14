using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour, IDamageable, GameOverReset
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
    [HideInInspector]public bool isHide;    //判断敌人是否隐身
    [HideInInspector]public bool isHalfHealth;  //判断是否已经半血
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
    public ParentStateMachine currentParentState;   //用于记录当前父类状态机
    public PatrolParentState patrolParentState = new PatrolParentState(); //巡逻大类
    public AttackParentState attackParentState = new AttackParentState(); //攻击大类
    public AssassinHalfHealthParentState halfHealthParentState = new AssassinHalfHealthParentState();   //半血以下大类

    //Bringer Of Death
    public BringerPatrolState bringerPatrolState = new BringerPatrolState();
    public BringerAttackState bringerAttackState = new BringerAttackState();
    public BringerHalfHealthState bringerHalfHealthState = new BringerHalfHealthState();

    [Header("Check Data")]
    public LayerMask playerMask;    //选择Player所在的图层

    [Header("Get Hit Data")]
    [HideInInspector]public bool isDead = false;    //用于判断是否死亡
    public List<FallObject> falls = new List<FallObject>();
    [HideInInspector]public List<GameObject> enemies;
    [HideInInspector]public GameObject _enemy;

    public virtual void Start()
    {
        //初始化
        InitData();
        InitComponent();
        GameManager.Instance.RegisterEnemies(this);
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
    }

    public virtual void FixedUpdate()
    {
        if(isDead) return;
        Attack();
    }

    public virtual void Movement()
    {
        if(isDead) return;
        if(isAttack) return;

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

    public void TransitionState(ParentStateMachine state)//切换状态类的函数
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

    public void RegisterEnemies(List<GameObject> otherEnemies, GameObject enemy)
    {
        enemies = otherEnemies;
        _enemy = enemy;
    }

    public void GetHit(CharacterStats attacker)    //受伤函数，由外部调用
    {
        if(!anim.GetCurrentAnimatorStateInfo(2).IsName("GetHit"))
        {
            //更改 Model
            characterStats.TakeDamage(attacker);

            //根据 Model 参数，调用事件总线更改 View
            EventManager.Instance?.InvokeEvent(UI.UpdateBossHealth, characterStats.CurrentHealth, characterStats.MaxHealth);

            //Boss Die
            if(characterStats.CurrentHealth <= 0)
            {
                isDead = true;

                if(enemies.Contains(_enemy))
                {
                    enemies.Remove(_enemy);
                }

                for(int i = 0; i < falls.Count; i++)
                {
                    float random = Random.Range(0f, 10f);
                    if(random <= falls[i].fallProbability)
                    {
                        for(int j = 0; j < falls[i].objectCount; j++)
                        {
                            GameObject _object = Instantiate(falls[i].fall);
                            _object.transform.position = transform.position;
                            _object.SetActive(true);

                            Vector2 force = new Vector2(Random.Range(-5f, 5f), Random.Range(1f, 3f));
                            _object.GetComponent<Rigidbody2D>().AddForce(force, ForceMode2D.Impulse);
                        }
                    }
                }

                if(QuestManager.IsInitialized)
                {
                    QuestManager.Instance.UpdateQuestProgress(transform.parent.name, 1);
                }
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

    #endregion

}
