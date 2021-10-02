using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameControllerDevelop : GameController
{
    [Range(1, 3)]
    [Tooltip("The current difficulty to test your game at.")]
    public int gameDifficultySlider = 1;

    public Image fadeOutSimulation;

    bool isSimulatingFadeOut = false;

    float fadeOutDir = -1;

    float opacity = 1;

    float fadeOutSpeed = 0.05f;

    private void Awake()
    {
        if (Application.isEditor)
        {
            Application.targetFrameRate = 60;
        }
        if (FindObjectsOfType(typeof(GameController)).Length > 1)
        {
            Destroy(this);
        }
        else
        {
            gameDifficulty = gameDifficultySlider;
            //=v=v=v=v=v=v=v=v=v=v=v=[ONLY FOR PLAYABLE WEBGL]=v=v=v=v=v=v=v=v=v=v=v=
            //gameDifficulty = PlayerPrefs.GetInt("SpeedBox_Difficulty", 1);
            //Debug.Log("This Snippet of Code Should Only be Running for the WebGL Build");
            //=^=^=^=^=^=^=^=^=^=^=^=[ONLY FOR PLAYABLE WEBGL]=^=^=^=^=^=^=^=^=^=^=^=
        }
    }

    private void Start()
    {
        // This will be localized to one scene, so we don't want any DontDestroyOnLoads.
        // We also don't want anything to be set up if there's already a GameController out there.
        // So if FindObjectsOfType finds both itself and any other GameControllers, this won't get called.
        if (FindObjectsOfType(typeof(GameController)).Length <= 1)
        {
            SimulatePause();
        }
    }

    void SimulatePause()
    {
        fadeOutSimulation.gameObject.SetActive(true);
        isSimulatingFadeOut = true;
        opacity = 1;
        fadeOutDir = -1;
        Time.timeScale = 0;
    }

    void SimulateEnd()
    {
        // TODO: Replace this with a transition.
        // Pausing is no longer feasible.
        Debug.Log("Running a *rough* simulation of what the transition will look like (the actual one will be somewhat different).");
        Time.timeScale = 0;
        fadeOutDir *= -1;
        isSimulatingFadeOut = true;
        fadeOutSimulation.gameObject.SetActive(true);
    }

    //Would normally cause a scene transition here, but because this is just for development,
    //it only prints out some debug messages
    protected override void LevelTransition(bool didWin)
    {
        Debug.Log("Game done! This is where the game would transition to the next microgame.");
        Debug.Log($"The game controller has recorded {this.gameWins} and {this.gameFails} loses");
        SimulateEnd();
    }

    // We don't want to override Update from GameController.
    private void LateUpdate()
    {
        if (isSimulatingFadeOut)
        {
            fadeOutSimulation.color = new Color(fadeOutSimulation.color.r, fadeOutSimulation.color.g, fadeOutSimulation.color.b, opacity);
            opacity += fadeOutDir * fadeOutSpeed;
            if (opacity <= 0)
            {
                isSimulatingFadeOut = false;
                Time.timeScale = 1;
                fadeOutSimulation.gameObject.SetActive(false);
                this.SceneInit();
            }
            else if (opacity >= 1)
            {
                isSimulatingFadeOut = false;
                // Don't ask me how this reloads the scene, but it somehow does.
                var newScene = SceneManager.CreateScene("SampleLoadingScene");
                var currScene = SceneManager.GetActiveScene();
                SceneManager.UnloadSceneAsync(currScene);
                SceneManager.LoadScene(currScene.name);
            }
        }
    }

    private void OnDestroy()
    {

    }
}
