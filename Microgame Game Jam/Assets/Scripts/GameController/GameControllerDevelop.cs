using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControllerDevelop : GameController
{
    ///Methods-------------------------------------------------------------------------------------
    //Called on first frame automatically
    void Start()
    {
        this.SceneInit();
    }

    //Would normally cause a scene transition here, but because this is just for development,
    //it only prints out some debug messages
    protected override void LevelTransition()
    {
        Debug.Log("Game done! This is where the game would transition to the next microgame.");
        Debug.Log($"The game controller has recorded {this.gameWins} and {this.gameFails} loses");
    }
}
