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

    public void StartGame() {

        var gameControllerCreator = new GameObject();
        gameControllerCreator.AddComponent<GameControllerRelease>();
        var controllerComponent = gameControllerCreator.GetComponent<GameControllerRelease>();
        // We make sure to set the minSceneIndex so that GameController knows which scenes to look at for games.
        controllerComponent.minSceneIndex = minSceneIndex;

        // We also have to set the scene for game over:
        controllerComponent.gameoverSceneIndex = gameOverSceneIndex;

        // And the scene for transitions:
        controllerComponent.transitionSceneIndex = transitionSceneIndex;
        // As soon as we load the next scene, GameController should reset.

        // We do this by .Instance so that we ensure the instance that's created is derived from GameControllerRelease.
        GameControllerRelease.Instance.WinGame();
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
