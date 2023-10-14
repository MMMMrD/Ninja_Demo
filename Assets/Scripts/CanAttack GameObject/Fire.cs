using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    public float speed;
    Transform targetTransform;
    CharacterStats characterStats;

    private void OnEnable() 
    {
        characterStats = GetComponent<CharacterStats>();
    }

    public void Init(Transform target, CharacterStats userCharacterStats)
    {
        targetTransform = target;
        characterStats.attackData = userCharacterStats.attackData;
    }

    private void FixedUpdate() 
    {
        if(targetTransform != null)
        {
            transform.position = Vector2.MoveTowards(transform.position, targetTransform.position, speed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.CompareTag("HitBox"))
        {
            if(other.transform.parent.CompareTag("Player"))
            {
                ObjectPool.Instance.PushObject(gameObject);
            }
        }    
    }
}
