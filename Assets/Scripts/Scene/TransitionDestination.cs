using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionDestination : MonoBehaviour
{
    public enum DestinationTag { Enter, Quit };  //记录传送门类型

    public DestinationTag destinationTag;   // 记录传送类型
}
