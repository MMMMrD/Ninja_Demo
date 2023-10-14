using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThunderShield : MonoBehaviour
{
    //Animation Event
    public void CloseGameObject()
    {
        ObjectPool.Instance.PushObject(gameObject);
    }
}
