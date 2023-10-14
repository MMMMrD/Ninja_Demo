using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    public bool X;
    public bool Y;
    private bool canMove = false;
    public Transform pointA;
    public Transform pointB;
    public Transform currentTarget;
    public float waitTime;
    public float moveSpeed;
    [HideInInspector]public float currentWaitTime;
    [HideInInspector]public Transform originParentTransform;

    private void Start()
    {
        pointA.parent = null;
        pointB.parent = null;
        if(pointA != null)
            currentTarget = pointA;
    }

    private void Update()
    {
        if(currentWaitTime > 0f)
        {
            currentWaitTime -= Time.deltaTime;
        }
    }

    private void FixedUpdate() 
    {
        paltformMovement();
    }

    private void paltformMovement()
    {
        if(!canMove) return;

        if(X)
        {
            if(Mathf.Abs(transform.position.x - currentTarget.position.x) < 0.1f)
            {
                if(currentTarget == pointA)
                {
                    currentTarget = pointB;
                }
                else if(currentTarget == pointB)
                {
                    currentTarget = pointA;
                }
                currentWaitTime = waitTime;
            }
        }

        if(Y)
        {
            if(X) return;

            if(Mathf.Abs(transform.position.y - currentTarget.position.y) < 0.1f)
            {
                if(currentTarget == pointA)
                {
                    currentTarget = pointB;
                }
                else if(currentTarget == pointB)
                {
                    currentTarget = pointA;
                }
                currentWaitTime = waitTime;
            }
        }

        if(currentWaitTime <= 0f)
        {
            if(currentTarget != null)
            {
                transform.position = Vector2.MoveTowards(transform.position, currentTarget.position, moveSpeed * Time.deltaTime);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        canMove = true;
        if(other.CompareTag("Player"))
        {
            originParentTransform = other.transform.parent;
            other.transform.parent = transform;
        }    
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            other.transform.parent = originParentTransform;
        }
    }
}
