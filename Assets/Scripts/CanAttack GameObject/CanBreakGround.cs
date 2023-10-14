using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CanBreakGround : MonoBehaviour
{
    private Collider2D coll;
    public bool left;
    public bool right;
    private int attackCount = 0;

    private void Start() 
    {
        coll = GetComponent<Collider2D>();    
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("HitBox"))
        {
            if(right)
            {
                if(other.transform.position.x >= transform.position.x)
                {
                    attackCount ++;
                    if(attackCount > 2)
                    {
                        Destroy(gameObject);
                    }
                }
            }

            if(left)
            {
                if(right) return;

                if(other.transform.position.x <= transform.position.x)
                {
                    attackCount ++;
                    if(attackCount > 2)
                    {
                        Destroy(gameObject);
                    }
                }
            }
        }
    }
}
