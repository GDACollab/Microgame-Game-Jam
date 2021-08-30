using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

    //the load of the transition scene
    private bool transitionLoaded;

    private Scene transitionScene;


    //Picks a random level in the build order then transitions to it
    protected override void LevelTransition()
    {
        StartCoroutine(AsyncTransitionTiming());
    }

    /*IEnumerator SyncTransitionTiming()
    {
        int destinationScene = this.previousGame;

        //Step 0: If this scene is the previous scene... uhhh... pick again?
        Debug.Log($"Picking a scene between {this.minSceneIndex} and {SceneManager.sceneCountInBuildSettings - 1}");
        while (destinationScene == this.previousGame && this.minSceneIndex != SceneManager.sceneCountInBuildSettings - 1)
        {
            destinationScene = Random.Range(this.minSceneIndex, SceneManager.sceneCountInBuildSettings);
        }
        Debug.Log($"Selecting scene #{destinationScene}");

        //And if the game's over, actually go to the end.
        destinationScene = this.gameFails >= this.maxFails ? gameoverSceneIndex : destinationScene;
        Debug.Log($"Transitioning to scene #{destinationScene}");

        //Step 1: Go to the transition
        SceneManager.LoadScene(transitionSceneIndex);
        Debug.Log($"Current Stats: {this.gameFails}/{this.maxFails} fails, {this.gameWins} wins. Time: {this.gameTime}");

        //Step 2: Wait a bit
        yield return new WaitForSeconds(100f);

        //Step 3: Go to destination
        SceneManager.LoadScene(destinationScene);

        //Step 4: Initialize the game controller for the next scene.
        this.SceneInit();
    }
    */

    //The timing of the actual scene transition
    //It's a coroutine to handle timing of effects because I doubt this transition 
    //should be instant
    //Doesn't quite work yet, but that has more to do with Thomas not understanding how to do
    //asynchronous scene managment correctly
    IEnumerator AsyncTransitionTiming()
    {
        // We wait 2 seconds to make the transition to the loading screen smoother. We pause the current time scale to
        // make sure stuff like animations remain paused:
        if (this.previousGame >= minSceneIndex)
        {
            Time.timeScale = 0;
            yield return new WaitForSecondsRealtime(endGameDelay);
            Time.timeScale = 1;
        }

        int destinationScene = this.previousGame;

        //Step 0: If this scene is the previous scene... uhhh... pick again?
        while (destinationScene == this.previousGame && minSceneIndex != SceneManager.sceneCountInBuildSettings - 1)
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
            // We don't want the scene to load async because we want it to be displaying
            // while everything else is going on.
            SceneManager.LoadScene(transitionSceneIndex);
            transitionLoaded = true;
            transitionScene = SceneManager.GetSceneByBuildIndex(transitionSceneIndex);
        }
        else {
            // Otherwise, our scene is already loaded, so we just set it to be active.
            ActivateAllObjectsInScene(transitionScene, true);
            SceneManager.SetActiveScene(transitionScene);
        }
        
        //SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(transitionSceneIndex));

        //Step 3: Unload the previous scene (if the previous scene was a game) AND load the next scene
        if(this.previousGame >= minSceneIndex) SceneManager.UnloadSceneAsync(this.previousGame);
        //this picks between game over and the next game depending on the comparison
        this.previousGame = destinationScene;

        // Because we're about to start loading the next scene, we need to make sure everything in the level
        // will be paused.
        // NOTE: BECAUSE OF THIS, WHENEVER INTRODUCING DELAYS, YOU MUST USE WaitForSecondsRealtime.
        Time.timeScale = 0;

        var nextSceneLoad = SceneManager.LoadSceneAsync(destinationScene, LoadSceneMode.Additive);
        var startTime = Time.time;

        // Make sure any new objects are not going to show up while we do loading:
        gameScene = SceneManager.GetSceneByBuildIndex(destinationScene);

        //Step 3: Delays
        // First, we wait for the scene to load.
        while (!nextSceneLoad.isDone)
        {
            yield return null;
        }

        List<GameObject> gameObjectsToActivate = new List<GameObject>();

        // Step 4: Hide everything in the loaded scene so we don't get two scenes on top of one another
        ActivateAllObjectsInScene(gameScene, false, gameObjectsToActivate);

        showGameObjects = false;

        var totalTimeLoading = Time.time - startTime;
        // If the loading time takes less time than we want the transition scene to show for,
        // we compensate for that here.
        if (totalTimeLoading <= 2.0f) {
            //If we want to manufacture any delays, the yields for them should go here
            yield return new WaitForSecondsRealtime(2.0f - totalTimeLoading);
        }

        //Step 4: Transition:
        showGameObjects = true;
        ActivateAllObjectsInScene(transitionScene, false);
        ActivateAllObjectsInScene(gameScene, true, gameObjectsToActivate);
        SceneManager.SetActiveScene(gameScene);

        // If we want a grace period for jammers to show instructions or something, we add a delay here:
        yield return new WaitForSecondsRealtime(gameStartDelay);

        // And now the scene is loaded in, so we can resume time:
        Time.timeScale = 1;

        //Transition done!

        if (destinationScene != gameoverSceneIndex)
        {
            Debug.Log("Scene Activated.");
            this.SceneInit();
        }
    }
}
