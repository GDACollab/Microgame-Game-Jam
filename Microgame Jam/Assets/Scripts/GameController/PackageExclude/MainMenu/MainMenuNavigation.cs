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

    public bool isDebug = false;

    public UnityEngine.EventSystems.EventSystem eventSystem;

    // Start is called before the first frame update
    void OnEnable()
    {
        eventSystem.enabled = true;
        // Only do this if there's only one MainMenuNavigation up (to prevent this code from being run multiple times):
        if (FindObjectsOfType(typeof(MainMenuNavigation)).Length <= 1)
        {
            GameControllerRelease controllerComponent = null;
            // Make sure GameController is set up:
            if (FindObjectsOfType(typeof(GameController)).Length == 0)
            {
                var gameControllerCreator = new GameObject();
                gameControllerCreator.AddComponent<GameControllerRelease>();
                controllerComponent = gameControllerCreator.GetComponent<GameControllerRelease>();
                // We make sure to set the minSceneIndex so that GameController knows which scenes to look at for games.
                controllerComponent.minSceneIndex = minSceneIndex;

                // We also have to set the scene for game over:
                controllerComponent.gameoverSceneIndex = gameOverSceneIndex;

                // And the scene for transitions:
                controllerComponent.transitionSceneIndex = transitionSceneIndex;

                controllerComponent.isDebug = isDebug;
            }
            else if (FindObjectsOfType(typeof(GameController)).Length == 1)
            {
                controllerComponent = (GameControllerRelease)FindObjectOfType(typeof(GameControllerRelease));
            }

            bool transitionSceneExists = false;
            bool gameOverSceneExists = false;
            bool nextGameExists = false;

            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                Scene j = SceneManager.GetSceneAt(i);
                Debug.Log(j.buildIndex);
                if (j.buildIndex == transitionSceneIndex)
                {
                    transitionSceneExists = true;
                }
                else if (j.buildIndex == gameOverSceneIndex)
                {
                    gameOverSceneExists = true;
                }
                else if (j.buildIndex >= this.minSceneIndex) {
                    nextGameExists = true;
                }
                if (transitionSceneExists && gameOverSceneExists)
                {
                    break;
                }
            }

            if (!transitionSceneExists)
            {
                StartCoroutine(PreloadScene(transitionSceneIndex));
            }

            if (!gameOverSceneExists)
            {
                // And make sure to have the gameOverScene in handy, in case anything goes wrong.
                StartCoroutine(PreloadScene(gameOverSceneIndex));
            }

            if (controllerComponent != null && !nextGameExists)
            {
                // Then, tell the gameController to start the next game.
                // We want this to be as fast as possible.
                StartCoroutine(controllerComponent.GetNextGame(ThreadPriority.High));
            }
        }
    }

    IEnumerator PreloadScene(int index) {
        var loading = SceneManager.LoadSceneAsync(index, LoadSceneMode.Additive);
        while (!loading.isDone) {
            yield return null;
        }
        // This method is static, so we can call it when we need to:
        GameController.ActivateAllObjectsInScene(SceneManager.GetSceneByBuildIndex(index), false);
    }

    public void StartGame() {
        Debug.Log("Start Game");
        // Make sure the canvas for this scene can't be tampered with any further:
        eventSystem.enabled = false;

        Debug.Log("Event system disabled...");

        // As soon as we load the next scene, GameController should reset.
        // We do this by .Instance so that we ensure the instance that's created is derived from GameControllerRelease.
        GameController.Instance.WinGame();
    }

    public void LoadCredits()
    {
        var credits = SceneManager.LoadSceneAsync("Credits", LoadSceneMode.Additive);
        SetActiveWhenDone(credits, SceneManager.GetSceneByName("Credits"));
        SceneManager.UnloadSceneAsync("TitleScreen");
    }

    IEnumerator SetActiveWhenDone(AsyncOperation op, Scene newScene) {
        while (!op.isDone)
        {
            yield return null;
        }
        SceneManager.SetActiveScene(newScene);
    }

    public void LoadMainMenu() {
        var controller = (GameControllerRelease)FindObjectOfType(typeof(GameControllerRelease));
        controller.ResetPrevGame();
        var currScene = this.gameObject.scene;
        var load = SceneManager.LoadSceneAsync("TitleScreen", LoadSceneMode.Additive);
        SceneManager.UnloadSceneAsync(currScene);
        SetActiveWhenDone(load, SceneManager.GetSceneByName("TitleScreen"));
    }

    public void QuitGame() {
        Application.Quit();
    }
}
