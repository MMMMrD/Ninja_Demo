using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ChildStateMachine
{
    public abstract void EnterState(Boss boss);
    public abstract void UpdateState(Boss boss);
    public abstract void ExitState(Boss boss);
}
