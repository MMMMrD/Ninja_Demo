using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FallObject
{
    public GameObject fall;
    public int objectCount;
    public float fallProbability;   //掉落概率
}

public class Enemy : MonoBehaviour, IDamageable, GameOverReset
{
    public bool isBoss;     //判断是否是boss
    [Header("Component")]
    [HideInInspector]public Animator anim;
    [HideInInspector]public Rigidbody2D rb;
    [HideInInspector]public Collider2D coll;
    [HideInInspector]public CharacterStats characterStats;   //记录Enemies的各项数据

    [Header("Find target Data")]
    public Transform pointA;
    public Transform pointB;
    [HideInInspector]public Transform target;   //用于记录目标
    public List<Transform> targets = new List<Transform>(); //用于记录目标点的列表

    [Header("Attack Data")]
    [HideInInspector]public bool isAttack;  //判断是否在攻击状态
    [HideInInspector]public int attackCount = 1;    //判断攻击次数
    public float attackCoolDown; //攻击冷却时间
    [HideInInspector]public float lastAttack; //用于记录上一次攻击时间

    [Header("状态机")]
    [HideInInspector]public BaseFSM currentState;  //记录当前状态
    public PatrolState patrolState = new PatrolState();
    public AttackState attackState = new AttackState();
    public FollowState followState = new FollowState();

    [Header("Get Hit Data")]
    [HideInInspector]public bool isDead = false;    //判断是否死亡
    public List<FallObject> falls = new List<FallObject>();
    [HideInInspector]public List<GameObject> enemies;
    [HideInInspector]public GameObject _enemy;

    public virtual void Start()
    {
        InitComponent();
        TransitionState(patrolState);

        GameManager.Instance.RegisterEnemies(this);
    }

    public virtual void Update() 
    {
        anim.SetBool("Dead", isDead);
        if(isDead) return;
        currentState.UpdateState(this);
    }

    public virtual void FixedUpdate()
    {
        if(isDead) return;
        Attack();
    }

    public void Movement()
    {
        if(target != null)
        {
            transform.position = Vector2.MoveTowards(transform.position, target.position, characterStats.CurrentSpeed * Time.deltaTime * 0.5f);
        }
        TurnAround();
    }

    public void Attack()
    {
        anim.SetBool("IsAttack", isAttack);
        if(Time.time - lastAttack <= attackCoolDown) return;

        if(isAttack)
        {
            AttackMode();
        }
    }

    public virtual void AttackMode()    //可以更改的攻击方式
    {
        switch(attackCount)
        {
            case 0:anim.Play("Attack1", 1);lastAttack = Time.time;attackCount++;break;
            case 1:anim.Play("Attack2", 1);lastAttack = Time.time;attackCount = 0;break;
        }
    }

    #region Tools Function
    public void InitComponent()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
        characterStats = GetComponent<CharacterStats>();
    }

    public void TransitionState(BaseFSM state)//切换状态类的函数
    {
        currentState = state;
        currentState.EnterState(this);//进入状态需要执行的函数
    }

    public void SwitchTargetPoint()//转换默认目标点,由状态机调用
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

    public void TurnAround()
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

    public void OnTriggerEnter2D(Collider2D other)
    {
        if(!targets.Contains(other.transform) && other.gameObject.CompareTag("Player") && !GameManager.Instance.gameOver)
        {
            targets.Add(other.transform);
        }
    } 

    public void OnTriggerStay2D(Collider2D other)
    {
        if(!targets.Contains(other.transform) && other.gameObject.CompareTag("Player") && !GameManager.Instance.gameOver)
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

    void ResetAttackAnimationData() //重置动画数据
    {
        anim.SetBool("Attack1", false);
        anim.SetBool("Attack2", false);
    }

    public void RegisterEnemies(List<GameObject> otherEnemies, GameObject enemy)
    {
        enemies = otherEnemies;
        _enemy = enemy;
    }

    public void GetHit(CharacterStats attacker) //当角色受伤时
    {
        if(!anim.GetCurrentAnimatorStateInfo(2).IsName("GetHit"))
        {
            anim.SetTrigger("GetHit");
            characterStats.TakeDamage(attacker);
            EventManager.Instance?.InvokeEvent<UI>(UI.UpdateBossHealth, characterStats.CurrentHealth, characterStats.MaxHealth);
            if(characterStats.CurrentHealth <= 0)   //当角色死亡时
            {
                //Reset Base Component
                GameManager.Instance.scenceEvent?.Invoke();
                isDead = true;
                coll.enabled = false;
                rb.constraints = RigidbodyConstraints2D.FreezeAll;

                if(enemies.Contains(_enemy))
                {
                    enemies.Remove(_enemy);
                }

                //控制是否掉落装备
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

                //更新任务
                if(QuestManager.IsInitialized)
                {
                    QuestManager.Instance.UpdateQuestProgress(transform.parent.name, 1);
                }
            }
        }
    }

    public void GameOverReset()
    {
        if(target != null)
        {
            target = null;
            TransitionState(patrolState);
        }
    }
    #endregion
}
