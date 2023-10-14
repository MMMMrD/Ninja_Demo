using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NeedEnemies
{
    public List<GameObject> enemies;
}

public class CreateEnemies : MonoBehaviour
{
    int i = 0;
    bool airActive = false;
    bool isStart = false;
    bool canCreateEnemies;
    public GameObject airWallLeft;
    public GameObject airWallRight;

    public GameObject mainCamera;
    public GameObject childCamera;

    public List<NeedEnemies> needEnemies = new List<NeedEnemies>();

    private void FixedUpdate() 
    {
        if(i < needEnemies.Count)
        {
            if(needEnemies[i].enemies.Count > 0)
            {
                if(canCreateEnemies)
                {
                    canCreateEnemies = !canCreateEnemies;
                    for(int j = 0; j < needEnemies[i].enemies.Count; j++)
                    {   
                        GameObject _object = Instantiate(needEnemies[i].enemies[j], new Vector3(transform.position.x + Random.Range(-10f,10f), transform.position.y, transform.position.z), transform.rotation);
                        _object.transform.GetChild(0).GetComponent<Enemy>()?.RegisterEnemies(needEnemies[i].enemies, needEnemies[i].enemies[j]);
                        _object.transform.GetChild(0).GetComponent<Boss>()?.RegisterEnemies(needEnemies[i].enemies, needEnemies[i].enemies[j]);
                    }
                }
            }
            else
            {
                canCreateEnemies = !canCreateEnemies;
                i++;
            }
        }

        if(i >= needEnemies.Count)
        {
            if(isStart)
            {
                isStart = false;
                canCreateEnemies = false;
                airActive = false;
                airWallLeft.SetActive(airActive);
                airWallRight.SetActive(airActive);
                mainCamera.SetActive(true);
                childCamera.SetActive(false);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.CompareTag("Player"))
        {
            if(!isStart)
            {
                isStart = true;
                SetAirWallActive();
                canCreateEnemies = true;
                mainCamera.SetActive(false);
                childCamera.SetActive(true);
            }
        }
    }

    void SetAirWallActive()
    {
        airActive = !airActive;
        airWallLeft.SetActive(airActive);
        airWallRight.SetActive(airActive);
    }
}
