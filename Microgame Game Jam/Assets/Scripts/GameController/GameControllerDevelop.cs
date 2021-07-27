using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControllerDevelop : GameController
{
    ///Methods-------------------------------------------------------------------------------------
    //Called on first frame automatically
    void Start()
    {
        // This will be localized to one scene, so we don't want any DontDestroyOnLoads.
        // We also don't want anything to be set up if there's already a GameController out there.  
        if ((GameController)FindObjectOfType(typeof(GameController)) == null)
        {
            this.SceneInit();
        }
    }

    //Would normally cause a scene transition here, but because this is just for development,
    //it only prints out some debug messages
    protected override void LevelTransition()
    {
        Debug.Log("Game done! This is where the game would transition to the next microgame.");
        Debug.Log($"The game controller has recorded {this.gameWins} and {this.gameFails} loses");
    }
    private void OnDestroy()
    {
        
    }
}
