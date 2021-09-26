using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneSkip_GameRunner : MonoBehaviour
{

    public List<StoneSkip_State> states;
    private StoneSkip_State currentState;

    private void Start()
    {
        SetState("Charging");
    }

    //State update running functionality
    void FixedUpdate()
    {
        if (currentState.switchState)
        {
            SetState(currentState.switchStateName);
        }

        currentState.StateUpdate();
    }

    public void SetState(string stateName) {
        //Reset params in previous state
        if (currentState != null)
        {
            currentState.switchState = false;
            currentState.switchStateName = "";
            currentState.activeState = false;
        }

        foreach (var state in states) {
            if (stateName == state.stateName) {
                currentState = state;
                state.activeState = true;
                state.StateStart();
            }
        }
    }
}
