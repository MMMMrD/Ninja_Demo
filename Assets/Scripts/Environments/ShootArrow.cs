using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootArrow : Bullet
{
    public override void InitEnvironment(Vector2 point, Transform userTransform, Transform targetTransform, CharacterStats attacker)
    {
        transform.position = point;
        if(targetTransform.position.x >= userTransform.position.x)
        {
            diraction = 1;
        }
        else
        {
            diraction = -1;
        }

        transform.localScale = new Vector3(transform.localScale.x, -diraction, transform.localScale.z);
        
        gameObject.SetActive(true);
    }
    public override void Movement()
    {
        rb.velocity = new Vector2(currentSpeed * diraction, rb.velocity.y);
    }

    public override void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            GameManager.Instance.player.characterStats.TakeDamage(characterStats);
        }

        if(other.CompareTag("HitBox"))
        {
            diraction = -diraction;
            transform.localScale = new Vector3(transform.localScale.x, -diraction, transform.localScale.z);
        }

        if(other.CompareTag("Ground"))
        {
            currentSpeed = 0f;
            ObjectPool.Instance.PushObject(gameObject);
        }
    }
}
