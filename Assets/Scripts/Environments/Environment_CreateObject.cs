using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Environment_CreateObject : MonoBehaviour
{
    bool canCreate;
    public float spawnCoolDown;
    float spawnTime = 0f;
    public GameObject fireFX;
    public Boss_AssassinCultist boss_AssassinCultist;
    public CharacterStats characterStats;

    public GameObject mainCamera;
    public GameObject childCamera;
    public Transform upPoint, downPoint, leftPoint, rightPoint;
    Transform originParent;

    private void OnEnable() 
    {
        characterStats = GetComponent<CharacterStats>();
        upPoint.parent = null;
        downPoint.parent = null;
        leftPoint.parent = null;
        rightPoint.parent = null;
    }

    private void Update()
    {
        if(!GameManager.Instance.enemies.Contains(boss_AssassinCultist))
        {
            canCreate = false;
            mainCamera.SetActive(true);
            childCamera.SetActive(false);
        }
        
        SpawnObject();
    }

    void SpawnObject()
    {
        if(!canCreate) return;

        if(Time.time >= spawnCoolDown + spawnTime)
        {
            spawnTime = Time.time;
            GameObject _object = ObjectPool.Instance.GetObjectButNoSetActive(fireFX);

            originParent = _object.transform.parent;
            _object.transform.parent = null;
            float fireX = Random.Range(leftPoint.position.x, rightPoint.position.x);
            float fireY = Random.Range(downPoint.position.y, upPoint.position.y);

            _object.transform.position = new Vector3(fireX, fireY, 0f);
            _object.transform.parent = originParent;

            _object.GetComponent<Fire>().Init(GameManager.Instance.player.transform, characterStats);
            _object.SetActive(true);
        }
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.CompareTag("Player"))
        {
            canCreate = true;
            mainCamera.SetActive(false);
            childCamera.SetActive(true);
        }    
    }
}
