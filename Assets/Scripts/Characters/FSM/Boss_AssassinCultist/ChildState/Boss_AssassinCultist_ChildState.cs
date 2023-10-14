using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Boss_AssassinCultist_ChildState
{
    public abstract void EnterState(Boss_AssassinCultist boss);
    public abstract void UpdateState(Boss_AssassinCultist boss);
    public abstract void ExitState(Boss_AssassinCultist boss);
}
