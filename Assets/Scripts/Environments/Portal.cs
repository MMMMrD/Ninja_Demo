using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    Animator anim;
    public float castTime;
    float portalStarTime;
    bool canCast;   //判断是否可以发射
    GameObject prefab;
    Vector3 potralPoint;
    Transform targetPoint;
    CharacterStats characterStats;
    public Transform createPrefabPoint;
    [HideInInspector]public float diraction;  //方向

    void OnEnable() 
    {
        Debug.Log("传送门被打开");

        if(anim == null)
            anim = GetComponent<Animator>();
        
        transform.localScale = new Vector3(0, 0, 1);

        if(potralPoint == null) return;

        canCast = true;
        portalStarTime = Time.time;
        transform.position = potralPoint;
        //更改传送门角度

    }

    public void Init(GameObject _object, Vector3 _point, Transform _target, CharacterStats attacker)
    {
        prefab = _object;
        potralPoint = _point;
        targetPoint = _target;
        characterStats = attacker;
        if(targetPoint.position.x >= potralPoint.x)
        {
            diraction = 1;
        }
        else
        {
            diraction = -1;
        }
    }

    public void Init(GameObject _object, Vector3 _point, Transform _target, CharacterStats attacker, Vector3 rotation)
    {
        prefab = _object;
        potralPoint = _point;
        targetPoint = _target;
        characterStats = attacker;
        transform.rotation = Quaternion.Euler(rotation);

        if(targetPoint.position.x >= potralPoint.x)
        {
            diraction = 1;
        }
        else
        {
            diraction = -1;
        }
    }

    void Update() 
    {
        if(canCast)
        {
            if(prefab != null)
            {
                if(Time.time >= castTime + portalStarTime)
                {
                    canCast = false;
                    anim.SetTrigger("Cast Attack");
                    PortalEnvironment _object = ObjectPool.Instance.GetObjectButNoSetActive(prefab).GetComponent<PortalEnvironment>();
                    _object.InitEnvironment(createPrefabPoint.position, transform, targetPoint, characterStats);
                }
            }
            else
            {
                canCast = false;
                anim.SetTrigger("Base Attack");
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.CompareTag("Player"))
        {
            GameManager.Instance.player.GetHit(characterStats);
        }    
    }

    //Animation Event
    public void ReturnObjectPool()
    {
        ObjectPool.Instance.PushObject(gameObject);
    }
}
