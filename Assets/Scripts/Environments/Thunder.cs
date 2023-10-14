using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thunder : Bullet
{
    public override void OnEnable()
    {
        base.OnEnable();
    }
    public override void InitEnvironment(Vector2 point, Transform userTransform, Transform targetTransform, CharacterStats attacker)
    {
        characterStats = attacker;
        transform.position = point;
        transform.rotation = userTransform.rotation;

        gameObject.SetActive(true);
    }

    public override void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {

            GameManager.Instance.player.characterStats.TakeDamage(characterStats);
        }
    }
}
