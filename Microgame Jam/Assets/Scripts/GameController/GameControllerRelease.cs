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

    //the load of the transition scene
    private bool transitionLoaded;

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
    }

    //The timing of the actual scene transition
    //It's a coroutine to handle timing of effects because I doubt this transition 
    //should be instant
    //Doesn't quite work yet, but that has more to do with Thomas not understanding how to do
    //asynchronous scene managment correctly
    IEnumerator StartGameTransition(bool didWin)
    {

        // We want these listeners to only be active when the game is transitioning, so we'll remove these listeners in UnpauseGame:

        canHideGame.AddListener(UnloadPrevGame);
        canShowGame.AddListener(ShowGame);
        canUnpause.AddListener(UnpauseGame);

        destinationScene = this.previousGame;
        // We're done with this scene, so as long as it's not the game over scene, we should add it to the list of places we don't want to go:
        if (this.previousGame == gameoverSceneIndex)
        {
            SceneManager.UnloadSceneAsync(gameoverSceneIndex);
        }
        else {
            previousGames.Enqueue(destinationScene);
        }

        //Step 0: If this scene is from the queue of games recently played... uhhh... pick again?
        while (previousGames.Contains(destinationScene) && minSceneIndex != SceneManager.sceneCountInBuildSettings - 1)
        {
            destinationScene = Random.Range(this.minSceneIndex, SceneManager.sceneCountInBuildSettings);
        }

        Debug.Log($"Selecting scene #{destinationScene}");

        //And if the game's over, actually go to the end.
        destinationScene = this.gameFails >= this.maxFails ? gameoverSceneIndex : destinationScene;
        Debug.Log($"Transitioning to scene #{destinationScene}");

        // Step 1: Tyler takes over writing the comments now. Let's see what we can fix.

        // Step 2: If the transition scene isn't loaded, load it now.
        // Once this step is done, the transition scene will always be loaded.
        if (!transitionLoaded)
        {
            // We want the previous screen to be shown, if possible.
            var transitionLoading = SceneManager.LoadSceneAsync(transitionSceneIndex, LoadSceneMode.Additive);
            transitionLoaded = true;
            transitionScene = SceneManager.GetSceneByBuildIndex(transitionSceneIndex);
            while (!transitionScene.isLoaded) {
                yield return null;
            }
            SceneManager.SetActiveScene(transitionScene);
        }
        else {
            // Otherwise, our scene is already loaded, so we just set it to be active.
            ActivateAllObjectsInScene(transitionScene, true);
            SceneManager.SetActiveScene(transitionScene);
        }

        while (!transitionScene.isLoaded) {
            yield return null;
        }
        foreach (GameObject obj in transitionScene.GetRootGameObjects()) {
            if (obj.name == "PointTracker") {
                // Now select the appropriate animation for whether or not we've won or lost, and we allow it to carry our unity events to later trigger
                // at various animation points:
                obj.GetComponent<ScoreTracker>().DidWin(didWin, canHideGame, canShowGame, canUnpause);
            }
        }

        // Because we're about to start loading the next scene, we need to make sure everything in the level
        // will be paused.
        // NOTE: BECAUSE OF THIS, WHENEVER INTRODUCING DELAYS, YOU MUST USE WaitForSecondsRealtime.
        // The animations for Transition_Screen_Win and Transition_Screen_Lose run on Unscaled Time because of this.
        Time.timeScale = 0;

        var loading = SceneManager.LoadSceneAsync(destinationScene, LoadSceneMode.Additive);

        // Wait until we're done loading to start deactivating stuff.
        while (!loading.isDone) {
            yield return null;
        }

        // Make sure any new objects are not going to show up while we do loading:
        gameScene = SceneManager.GetSceneByBuildIndex(destinationScene);

        gameObjectsToActivate = new List<GameObject>();

        // Step 3: Hide everything in the loaded scene so we don't get two scenes on top of one another
        ActivateAllObjectsInScene(gameScene, false, gameObjectsToActivate);

        showGameObjects = false;

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
        //Step 4: Transition:
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
        }
    }
}
