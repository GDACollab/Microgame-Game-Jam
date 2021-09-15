using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class ScoreTracker : MonoBehaviour
{
    public GameObject WinState;
    public GameObject LoseState;

    public List<GameObject> liveObjects;

    private void OnEnable()
    {
        // This is to prevent any one animation from starting when TransitionScene is reloaded.
        // Since these objects are not root level, their active states are saved, so we have to specifically make sure
        // that they're set to be active.
        WinState.SetActive(false);
        LoseState.SetActive(false);

        int lives = GameController.Instance.maxFails - GameController.Instance.gameFails;
        foreach (GameObject life in liveObjects) {
            // Iterate through all the displays of lives, set the ones that are unused to be not active.
            if (lives <= 0) {
                life.SetActive(false);
            }
            lives--;
        }
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
