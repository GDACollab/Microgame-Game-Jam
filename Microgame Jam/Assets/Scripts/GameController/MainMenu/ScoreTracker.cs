using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreTracker : MonoBehaviour
{
    public GameObject WinState;
    public GameObject LoseState;


    void OnDisable()
    {
        WinState.SetActive(false);
        LoseState.SetActive(false);
    }

    public void DidWin(bool win) {
        if (win)
        {
            WinState.SetActive(true);
        }
        else {
            LoseState.SetActive(true);
        }
    }
}
