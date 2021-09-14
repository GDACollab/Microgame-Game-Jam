using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class ScoreTracker : MonoBehaviour
{
    public GameObject WinState;
    public GameObject LoseState;


    void OnDisable()
    {
        WinState.SetActive(false);
        LoseState.SetActive(false);
    }

    public void DidWin(bool win, UnityEvent canHideGame, UnityEvent canShowGame, UnityEvent canUnpause) {
        if (win)
        {
            WinState.GetComponent<TransitionAnimation>().SetEvents(canHideGame, canShowGame, canUnpause);
            WinState.SetActive(true);
        }
        else {
            LoseState.GetComponent<TransitionAnimation>().SetEvents(canHideGame, canShowGame, canUnpause);
            LoseState.SetActive(true);
        }
    }
}
