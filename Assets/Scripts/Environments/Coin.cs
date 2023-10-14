using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    public int containMoney;
    Animator anim;
    Rigidbody2D rb;
    CircleCollider2D coll;
    bool canAddMoney = false;

    private void Start() 
    {
        anim = GetComponent<Animator>();   
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<CircleCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.CompareTag("Player"))
        {
            if(!canAddMoney)
            {
                canAddMoney = true;
                anim.SetBool("End", canAddMoney);
                rb.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionX;
                coll.enabled = false;
                GameManager.Instance.player.characterStats.Money += containMoney;
                EventManager.Instance?.InvokeEvent<UI>(UI.UpdateMoney);
            }
        }
    }

    //Animation Event
    public void DestoryGameObject()
    {
        Destroy(gameObject);
    }

}
