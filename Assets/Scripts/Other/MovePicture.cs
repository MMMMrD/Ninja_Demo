using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePicture : MonoBehaviour
{
    private void Update() 
    {
        transform.position = new Vector3(GameManager.Instance.player.transform.position.x * 0.2f, GameManager.Instance.player.transform.position.y * 0.2f, 1);    
    }
}
