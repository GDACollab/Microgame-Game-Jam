using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class ScoreTracker : MonoBehaviour
{
    public GameObject WinState;
    public GameObject LoseState;

    private void OnEnable()
    {
        // This is to prevent any one animation from starting when TransitionScene is reloaded.
        WinState.SetActive(false);
        LoseState.SetActive(false);
    }

    public void DidWin(bool win, UnityEvent canHideGame, UnityEvent canShowGame, UnityEvent canUnpause) {
        if (win)
        {
            WinState.SetActive(true);
            WinState.GetComponent<TransitionAnimation>().SetEvents(canHideGame, canShowGame, canUnpause);
        }
        else {
            LoseState.SetActive(true);
            LoseState.GetComponent<TransitionAnimation>().SetEvents(canHideGame, canShowGame, canUnpause);
        }
    }
}
