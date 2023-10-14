using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cultist : Enemy
{
    public GameObject fire; //攻击时释放火焰
    public Transform hitPoint;  //释放火球的位置
    public override void Start()
    {
        base.Start();
    }

    public override void Update()
    {
        base.Update();
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }
    
    //Animation Event
    public void ReleaseFire()   //释放火焰
    {
        GameObject _object = ObjectPool.Instance.GetObject(fire);
        _object.transform.position = hitPoint.position;
        _object.GetComponent<Bullet>().Init(transform, characterStats);
    }
}
