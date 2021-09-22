using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TransitionAnimation : MonoBehaviour
{
    // The reason we have this class set up is because animation events will only trigger attached component functions.
    // So, we pass the events that GameControllerRelease has to here, and invoke them when the animation events happen.

    UnityEvent hideGame;
    UnityEvent showGame;
    UnityEvent startGame;
    public MaskController maskController;

    public void SetEvents(UnityEvent hide, UnityEvent show, UnityEvent start) {
        hideGame = hide;
        showGame = show;
        startGame = start;
    }

    public void OnStartTransition() {
        maskController.ToggleMaskDirection();
    }

    public void OnEndTransitionStart() {
        maskController.ToggleMaskDirection();
    }

    // These are functions called by the Transition_Screen_Win and Transition_Screen_Lose sprites when their animations
    // hit certain points.

    // When the current game on screen can be safely hidden.
    public void OnGameHide() {
        hideGame.Invoke();
    }

    // When the next game can be shown on screen (we try to wait as long as possible for this in the animation, since sounds
    // can also start playing when this happens).
    public void OnGameShow() {
        showGame.Invoke();
    }

    // When the game can finally be started.
    public void OnGameStart() {
        startGame.Invoke();
    }
}
