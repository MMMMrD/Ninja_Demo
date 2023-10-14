using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseFSM
{
    public abstract void EnterState(Enemy enemy);
    public abstract void UpdateState(Enemy enemy);
}
