using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControllerDevelop : GameController
{
    protected override void LevelTransition()
    {
        Debug.Log("Game done! This is where the game would transition to the next microgame.");
    }
}
