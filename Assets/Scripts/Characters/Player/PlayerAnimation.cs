using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [Header("Component")]
    private Animator anim;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private PlayerController playerController;

    [Header("Action Object")]
    public GameObject attack1;  //刀光1
    public GameObject attack2;  //刀光2
    public GameObject attackDown;  //刀光下

    void Start()
    {
        InitComponent();

        //使用事件总线优化，解开 PlayerController 与 PlayerAction 两者耦合
        //playerController.GetHitAction += GetHit;

        EventManager.Instance.AddListener(UI.UpdatePlayerHealth, GetHit);
    }

    void Update() 
    {
        SetAnimation();
        AnimCheck();
    }

    void InitComponent()    //初始化组件
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        playerController = GetComponent<PlayerController>();
    }

    
    void AnimCheck()
    {
        if(anim.GetCurrentAnimatorStateInfo(0).IsName("SlideWall"))
        {
            spriteRenderer.flipX = false;
        }
        else
        {
            spriteRenderer.flipX = true;
        }
    }

    //初始化动画
    void SetAnimation()
    {
        anim.SetFloat("Down",rb.velocity.y);
        anim.SetFloat("Run", Mathf.Abs(rb.velocity.x));
        anim.SetBool("Jump",playerController.isJump);
        anim.SetBool("Dead", playerController.isDead);
        anim.SetBool("IsMove", playerController.isMove);
        anim.SetBool("Attack1", playerController.attack1);
        anim.SetBool("Attack2", playerController.attack2);
        anim.SetBool("Attack3", playerController.attack3);
        anim.SetBool("AirAttack1", playerController.airAttack1);
        anim.SetBool("AirAttack2", playerController.airAttack2);
        anim.SetBool("AirAttack3", playerController.airAttack3);
        anim.SetBool("AttackDown", playerController.attackDown);
        //playerController.isHit = anim.GetCurrentAnimatorStateInfo(3).IsName("GetHit");
    }

    private void GetHit(object data)
    {
        anim.SetTrigger("GetHit");

        //将Model层数据更新，放置在PlayerController中执行，改脚本只负责管理Player动画

        //CharacterStats attacker = (CharacterStats)data;
        //var player = GameManager.Instance.player;
        //player.characterStats.TakeDamage(attacker);
    }

    //外部调用，获得玩家动画信息
    public bool GetCurrentAnimationInfo(int state, string name) 
    {
        return anim.GetCurrentAnimatorStateInfo(state).IsName(name);
    }

    #region Animation Event

    public void ResetAttack()
    {
        playerController.attack1 = false;
        playerController.attack2 = false;
        playerController.attack3 = false;
        playerController.airAttack1 = false;
        playerController.airAttack2 = false;
        playerController.airAttack3 = false;
        playerController.isAttack = false;
        playerController.attackDown = false;
    }
    #endregion 
}
