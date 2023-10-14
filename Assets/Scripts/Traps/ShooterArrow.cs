using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterArrow : MonoBehaviour
{
    public GameObject arrow;
    public Transform shootPoint;
    private Animator anim;
    public float ShootCoolDown;
    float ShootTime;
    bool canShoot;
    int diraction = 1;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        ShootTime = -ShootCoolDown;
    }

    // Update is called once per frame
    void Update()
    {
        if(Mathf.Abs(GameManager.Instance.player.transform.position.y - transform.position.y) < 0.5f)
        {
            canShoot = true;
        }
        else
        {
            canShoot = false;
        }

        if(GameManager.Instance.player.transform.position.x >= transform.position.x)
        {
            diraction = 1;
        }
        else
        {
            diraction = -1;
        }

        if(canShoot)
        {
            if(Time.time >= ShootCoolDown + ShootTime)
            {
                ShootTime = Time.time;
                anim.SetTrigger("Attack");
            }
        }
    }

    //Animation Event
    public void CreateArrow()
    {
        PortalEnvironment _bullet = ObjectPool.Instance.GetObjectButNoSetActive(arrow).GetComponent<PortalEnvironment>();
        _bullet.InitEnvironment(shootPoint.position, transform, GameManager.Instance.player.transform, null);
    }
}
