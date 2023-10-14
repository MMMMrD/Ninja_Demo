using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirWall : MonoBehaviour
{
    bool airActive = false;
    public GameObject airWallLeft;
    public GameObject airWallRight;
    
    public GameObject mainCamera;
    public GameObject childCamera;

    private void Start()
    {
        GameManager.Instance.scenceEvent += SetAirWallActive;
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.CompareTag("Player"))
        {
            mainCamera.SetActive(false);
            childCamera.SetActive(true);
        }
    }

    void SetAirWallActive()
    {
        airActive = !airActive;
        airWallLeft?.SetActive(airActive);
        airWallRight?.SetActive(airActive);
    }
}
