using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordLight : MonoBehaviour
{
    //Animation Event
    public void EndAnimation()
    {
        gameObject.SetActive(false);
    }
}
