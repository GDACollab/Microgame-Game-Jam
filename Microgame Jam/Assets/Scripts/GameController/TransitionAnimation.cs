using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionAnimation : MonoBehaviour
{
    // These are functions called by the Transition_Screen_Win and Transition_Screen_Lose sprites when their animations
    // hit certain points.

    // When the current game on screen can be safely hidden.
    public void OnGameHide() { 
    
    }

    // When the next game can be shown on screen (we try to wait as long as possible for this in the animation, since sounds
    // can also start playing when this happens).
    public void OnGameShow() { 
        
    }

    // When the game can finally be started.
    public void OnGameStart() { 
        
    }
}
