using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameControllerDevelop : GameController
{
    [Range(1, 3)]
    [Tooltip("The current difficulty to test your game at.")]
    public int gameDifficultySlider = 1;

    private void Awake()
    {
        if (Application.isEditor)
        {
            Application.targetFrameRate = 60;
        }
        if (FindObjectsOfType(typeof(GameController)).Length > 1)
        {
            gameDifficulty = gameDifficultySlider;
            Destroy(this);
        }
    }

    private void Start()
    {
        // This will be localized to one scene, so we don't want any DontDestroyOnLoads.
        // We also don't want anything to be set up if there's already a GameController out there.
        // So if FindObjectsOfType finds both itself and any other GameControllers, this won't get called.
        if (FindObjectsOfType(typeof(GameController)).Length <= 1)
        {
            StartCoroutine("SimulatePause");
        }
    }

    IEnumerator SimulatePause()
    {
        Time.timeScale = 0;
        yield return new WaitForSecondsRealtime(gameStartDelay);
        Time.timeScale = 1;
        this.SceneInit();
    }

    IEnumerator SimulateEnd()
    {
        Debug.Log("Pausing the game now to simulate what the transition would look like on your game's end...");
        Time.timeScale = 0;
        yield return new WaitForSecondsRealtime(endGameDelay);
        Debug.Log("Pause over, unloading scene to simulate what the transition would look like.");
        SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().buildIndex);
    }

    //Would normally cause a scene transition here, but because this is just for development,
    //it only prints out some debug messages
    protected override void LevelTransition(bool didWin)
    {
        Debug.Log("Game done! This is where the game would transition to the next microgame.");
        Debug.Log($"The game controller has recorded {this.gameWins} and {this.gameFails} loses");
        StartCoroutine("SimulateEnd");
    }
    private void OnDestroy()
    {

    }
}
