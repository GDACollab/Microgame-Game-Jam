using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameControllerRelease : GameController
{
    [Tooltip("The lowest scene index that is an actual microgame. It is assumed" +
        " that all scenes minSceneIndex through SceneManager.sceneCountInBuildSettings-1 are microgames.")]
    public int minSceneIndex;

    [Tooltip("The index of the transition scene.")]
    public int transitionSceneIndex;

    [Tooltip("The index of the Game Over scene.")]
    public int gameoverSceneIndex;

    [Tooltip("How much of all the games should a player typically see before we start showing repeats?")]
    public float gameCoveragePercentage = 0.33f;

    [Tooltip("If there are say... 1000 games, how many should we push to the queue of previously played games before we start removing memory of having played those games?")]
    public int maxQueueLength = 10;

    private Queue<int> previousGames = new Queue<int>();

    // The scene that's used to transition between levels.
    private Scene transitionScene;

    // UnityEvents passed through ScoreTracker and TransitionAnimation, Invoked by animation events when...
    // It's safe to hide the previous game
    private UnityEvent canHideGame = new UnityEvent();
    // It's safe to show the next game
    private UnityEvent canShowGame = new UnityEvent();
    // It's safe to unpause the next game
    private UnityEvent canUnpause = new UnityEvent();

    // A list of game objects in the next game that we need to unpause. Set by ActivateAllObjectsInScene
    private List<GameObject> gameObjectsToActivate;

    // The scene we want to go to next.
    private int destinationScene;

    // Where we want to go to after destinationScene is done.
    private int nextDestinationScene;

    // The default transition camera, so we can restore its settings after mirroring previous games' cameras.
    private Camera transitionCameraDefault;


    //Picks a random level in the build order then transitions to it
    protected override void LevelTransition(bool didWin)
    {
        StartCoroutine(StartGameTransition(didWin));
    }

    // Called when the animation covers the previous game.
    protected void UnloadPrevGame() {
        //Unload the previous scene:
        SceneManager.UnloadSceneAsync(this.previousGame);
        //this picks between game over and the next game depending on the comparison
        this.previousGame = destinationScene;

        // We make sure the queue remains at a fixed size once it reaches capacity. A game should not be played until a player has already
        // played gameCoveragePercentage% of all available games.
        if (previousGames.Count > Mathf.Clamp((SceneManager.sceneCountInBuildSettings - minSceneIndex) * gameCoveragePercentage, 0, maxQueueLength))
        {
            previousGames.Dequeue();
        }

        // Now that we're no longer copying from the previous game's camera, we can switch to the default scene settings:
        var potentialCameras = GameObject.FindGameObjectsWithTag("MainCamera");
        foreach (GameObject camera in potentialCameras)
        {
            if (camera.scene.buildIndex == transitionSceneIndex)
            {
                camera.GetComponent<Camera>().CopyFrom(transitionCameraDefault);
            }
        }

    }

    IEnumerator LoadTransitionScene(bool didWin) {
        // The transition scene should have already been loaded by MainMenuNavigation.cs.
        // We need to get a reference to the transitionScene if we haven't already:
        if (transitionScene.name == null)
        {
            transitionScene = SceneManager.GetSceneByBuildIndex(transitionSceneIndex);
        }

        // Now we set everything in the transition scene to be active:
        ActivateAllObjectsInScene(transitionScene, true);
        SceneManager.SetActiveScene(transitionScene);

        // In case the transition scene still needs to load some stuff, we wait.
        while (!transitionScene.isLoaded)
        {
            yield return null;
        }

        // Mirror the main camera settings from the previous game scene, since we don't want to unload things weirdly.
        // This is a hacky fix, but unless people start experimenting with their unity camera setups (and changing their camera's tags),
        // this should hopefully work fine for most games.
        Camera gameMainCamera = null;
        GameObject transitionCamera = null;
        var potentialCameras = GameObject.FindGameObjectsWithTag("MainCamera");
        foreach (GameObject camera in potentialCameras)
        {
            if (camera.scene.buildIndex == previousGame)
            {
                gameMainCamera = camera.GetComponent<Camera>();
            }
            if (camera.scene.buildIndex == transitionSceneIndex)
            {
                transitionCamera = camera;
                transitionCameraDefault = transitionCamera.GetComponent<Camera>();
            }
        }

        // Copy the settings from the game's camera, if we found one:
        if (gameMainCamera != null && transitionCamera != null)
        {
            transitionCamera.GetComponent<Camera>().CopyFrom(gameMainCamera);
        }


        // Now that this scene is loaded, mirror the camera settings from the previous game to match this one.

        foreach (GameObject obj in transitionScene.GetRootGameObjects())
        {
            if (obj.name == "PointTracker")
            {
                // Now select the appropriate animation for whether or not we've won or lost, and we allow it to carry our unity events to later trigger
                // at various animation points:
                obj.GetComponent<ScoreTracker>().DidWin(didWin, canHideGame, canShowGame, canUnpause);
            }
        }
    }

    public IEnumerator GetNextGame() {
        // Okay, now we can start figuring out what we're loading for next time:
        nextDestinationScene = this.previousGame;
        // We're done with this scene, so as long as it's not the game over scene, we should add it to the list of places we don't want to go:
        if (this.previousGame != gameoverSceneIndex)
        {
            previousGames.Enqueue(nextDestinationScene);
        }


        while (previousGames.Contains(nextDestinationScene))
        {
            nextDestinationScene = Random.Range(this.minSceneIndex, SceneManager.sceneCountInBuildSettings);
        }
        Debug.Log("Loading " + nextDestinationScene + " next.");

        var loading = SceneManager.LoadSceneAsync(nextDestinationScene, LoadSceneMode.Additive);

        // Wait until we're done loading to start deactivating stuff.
        while (!loading.isDone)
        {
            yield return null;
        }

        nextGameScene = SceneManager.GetSceneByBuildIndex(nextDestinationScene);

        // Tell GameController.cs to continually hide everything that shows up in nextGameScene:
        showGameObjects = false;
    }

    //Starts setting everything up to allow for the transition animations from the TransitionScene (see the MasterScenes/Transition scene)
    IEnumerator StartGameTransition(bool didWin)
    {
        // We want these listeners to only be active when the game is transitioning, so we'll remove these listeners in UnpauseGame.
        // But for now, we activate them so that the animations triggered by transitionScene can invoke these events:

        canHideGame.AddListener(UnloadPrevGame);
        canShowGame.AddListener(ShowGame);
        canUnpause.AddListener(UnpauseGame);

        // Wait until the next game scene is loaded:
        while (nextGameScene != null && !nextGameScene.isLoaded) {
            yield return null;
        }

        // Since we've transitioned over, we can now set destinationScene to nextDestinationScene:
        destinationScene = nextDestinationScene;

        Debug.Log($"Selecting scene #{destinationScene}");

        // Activate the transitioning scene, to show that we're in a transition.
        StartCoroutine(LoadTransitionScene(didWin));

        // Begin setting the relevant things so that ShowGame and UnpauseGame work as intended.
        // If the game's over, actually go to the end.
        destinationScene = this.gameFails >= this.maxFails ? gameoverSceneIndex : destinationScene;

        // Make sure any new objects are not going to show up while we do loading:
        gameScene = SceneManager.GetSceneByBuildIndex(destinationScene);

        // Because we're about to start activating everything in the next scene, we need to make sure everything in the level
        // will be paused.
        // NOTE: BECAUSE OF THIS, WHENEVER INTRODUCING DELAYS, YOU MUST USE WaitForSecondsRealtime.
        // The animations for Transition_Screen_Win and Transition_Screen_Lose run on Unscaled Time because of this.
        Time.timeScale = 0;

        // We do this before the scene is loaded, because a player might click the restart button when the scene is loaded, but before
        // we've reset all variables.
        if (destinationScene == gameoverSceneIndex)
        {
            // The game is over, so reset all variables.
            this.gameWins = 0;
            this.gameDifficulty = 0;
            this.gameFails = 0;
        }
    }

    // Called when it's safe to start loading everything in the game's scene (dictated by animation events from the TransitionScene by Transition_Scene_Win or Transition_Scene_Lose).
    protected void ShowGame() {
        //Step 5: Transition:
        // We're counting on the games being small enough to be quickly loaded by the time this event gets called.
        // If that doesn't happen, well, some changes will have to be made to the game that's being loaded.
        // A good thing TODO would be to make a looping transition screen, and to wait until the game has finished loading before
        // starting the animation to transition to the next game.
        showGameObjects = true;
        ActivateAllObjectsInScene(gameScene, true, gameObjectsToActivate);
    }

    // Called when it's safe to unpause the game.
    protected void UnpauseGame() {
        ActivateAllObjectsInScene(transitionScene, false);
        // And now the scene is loaded in, so we can resume time:
        Time.timeScale = 1;

        //Transition done!
        // Remove our listeners:
        canHideGame.RemoveAllListeners();
        canShowGame.RemoveAllListeners();
        canUnpause.RemoveAllListeners();

        SceneManager.SetActiveScene(gameScene);

        if (destinationScene != gameoverSceneIndex)
        {
            Debug.Log("Scene Activated.");
            this.SceneInit();

            // And now we can start loading the next game:
            StartCoroutine(GetNextGame());
        }
    }
}
