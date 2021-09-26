using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StoneSkip_State : MonoBehaviour
{
    public string stateName;

    [HideInInspector]
    public bool activeState;
    [HideInInspector]
    public string switchStateName;
    [HideInInspector]
    public bool switchState;

    protected StoneSkip_State(string stateName_) {
        stateName = stateName_;
        switchStateName = "";
        switchState = false;
        activeState = false;
    }

    public abstract void StateStart();

    public abstract void StateUpdate();

    public void SwitchState(string name) {
        switchStateName = name;
        switchState = true;
    }
}
