using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControllerDevelop : GameController
{
    [Range(1, 3)]
    [Tooltip("The current difficulty to test your game at.")]
    public int gameDifficultySlider = 1;

    ///Methods-------------------------------------------------------------------------------------
    //Called on first frame automatically
    void Start()
    {
        // This will be localized to one scene, so we don't want any DontDestroyOnLoads.
        // We also don't want anything to be set up if there's already a GameController out there.
        // So if FindObjectsOfType finds both itself and any other GameControllers, this won't get called.
        if (FindObjectsOfType(typeof(GameController)).Length <= 1)
        {
            gameDifficulty = gameDifficultySlider;
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
