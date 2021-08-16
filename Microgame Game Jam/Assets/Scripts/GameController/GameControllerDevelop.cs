using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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
            canvasObject = new GameObject();
            canvasObject.AddComponent<Canvas>();
            var tmp = canvasObject.AddComponent<TextMeshProUGUI>();
            tmp.alignment = TextAlignmentOptions.Center;
            tmp.fontSize = 1;
            tmp.enabled = false;
            StartCoroutine("SimulatePause");
        }
    }

    IEnumerator SimulatePause()
    {
        Time.timeScale = 0;
        DisplayHelpText(true);
        yield return new WaitForSecondsRealtime(gameStartDelay);
        DisplayHelpText(false);
        Time.timeScale = 1;
        gameDifficulty = gameDifficultySlider;
        this.SceneInit();
    }

    //Would normally cause a scene transition here, but because this is just for development,
    //it only prints out some debug messages
    protected override void LevelTransition()
    {
        Debug.Log("Game done! This is where the game would transition to the next microgame.");
        Debug.Log($"The game controller has recorded {this.gameWins} and {this.gameFails} loses");
        Debug.Log("Pausing the game now to simulate what the transition would look like on your game's end...");
        Time.timeScale = 0;
    }
    private void OnDestroy()
    {

    }
}
