using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuNavigation : MonoBehaviour
{
    /// <summary>
    /// The lowest scene index that is an actual microgame. It is assumed
    /// that all scenes minSceneIndex through SceneManager.sceneCountInBuildSettings-1 
    /// are microgames.
    /// This is used to set the GameController's Singleton.
    /// </summary>
    [Tooltip("The lowest scene index that is an actual microgame. It is assumed" +
        " that all scenes minSceneIndex through SceneManager.sceneCountInBuildSettings-1 are microgames.")]
    public int minSceneIndex;

    /// <summary>
    /// The scene for transitions between levels.
    /// </summary>
    [Tooltip("The index of the transition scene.")]
    public int transitionSceneIndex;

    /// <summary>
    /// The index for the game over scene.
    /// </summary>
    [Tooltip("The index of the Game Over scene.")]
    public int gameOverSceneIndex;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // NOTE: This will NOT work unless you change line 5 of GameController.cs to:
    // public abstract class GameController : Singleton<GameControllerRelease>
    // Which is a weird chain of dependencies, I know.
    public void StartGame() {
        var randomScene = Random.Range(minSceneIndex, SceneManager.sceneCountInBuildSettings - 1);
        // We make sure to set the minSceneIndex so that GameController knows which scenes to look at for games.
        GameController.Instance.minSceneIndex = minSceneIndex;

        // We also have to set the scene for game over:
        GameController.Instance.gameoverSceneIndex = gameOverSceneIndex;

        // And the scene for transitions:
        GameController.Instance.transitionSceneIndex = transitionSceneIndex;
        // As soon as we load the next scene, GameController should reset.
        SceneManager.LoadScene(randomScene);
    }

    public void LoadCredits()
    {
        SceneManager.LoadScene("Credits");
    }

    public void LoadMainMenu() {
        SceneManager.LoadScene("TitleScreen");
    }

    public void QuitGame() {
        Application.Quit();
    }
}
