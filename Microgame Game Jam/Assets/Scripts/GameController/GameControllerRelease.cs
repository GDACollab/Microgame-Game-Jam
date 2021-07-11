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
    private AsyncOperation transitionLoad;

    //Picks a random level in the build order then transitions to it
    protected override void LevelTransition()
    {
        StartCoroutine(SyncTransitionTiming());
    }

    IEnumerator SyncTransitionTiming()
    {
        int destinationScene = this.previousGame;

        //Step 0: If this scene is the previous scene... uhhh... pick again?
        Debug.Log($"Picking a scene between {this.minSceneIndex} and {SceneManager.sceneCountInBuildSettings - 1}");
        while(destinationScene == this.previousGame && this.minSceneIndex != SceneManager.sceneCountInBuildSettings - 1)
        {
            destinationScene = Random.Range(this.minSceneIndex, SceneManager.sceneCountInBuildSettings);
        }
        Debug.Log($"Selecting scene #{destinationScene}");

        //And if the game's over, actually go to the end.
        destinationScene = this.gameFails >= this.maxFails ? gameoverSceneIndex : destinationScene;
        Debug.Log($"Transitioning to scene #{destinationScene}");

        //Step 1: Go to the transition
        SceneManager.LoadScene(transitionSceneIndex);
        Debug.Log($"Current Stats: {this.gameFails}/{this.maxFails} fails, {this.gameWins} wins.");

        //Step 2: Wait a bit
        yield return new WaitForSeconds(0.5f);

        //Step 3: Go to destination
        SceneManager.LoadScene(destinationScene);
    }

    //The timing of the actual scene transition
    //It's a coroutine to handle timing of effects because I doubt this transition 
    //should be instant
    //Doesn't quite work yet, but that has more to do with Thomas not understanding how to do
    //asynchronous scene managment correctly
    IEnumerator AsyncTransitionTiming()
    {
        int destinationScene = this.previousGame;

        //Step 0: If this scene is the previous scene... uhhh... pick again?
        while(destinationScene == this.previousGame && minSceneIndex != SceneManager.sceneCount - 1)
        {
            destinationScene = Random.Range(this.minSceneIndex, SceneManager.sceneCount);
        }
        Debug.Log($"Selecting scene #{destinationScene}");

        //And if the game's over, actually go to the end.
        destinationScene = this.gameFails >= this.maxFails ? gameoverSceneIndex : destinationScene;
        Debug.Log($"Transitioning to scene #{destinationScene}");

        //Step 1: Transition to the Transition Scene, which is always loaded
        if(transitionLoad != null)
        {
            transitionLoad.allowSceneActivation = true;
            transitionLoad = null;
        }
        //If it isn't loaded for some reason, just raw transition right now
        else
        {
            SceneManager.LoadScene(transitionSceneIndex);
        }
        //SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(transitionSceneIndex));

        //Step 2: Unload the previous scene AND load the next scene
        if(this.previousGame >= minSceneIndex) SceneManager.UnloadSceneAsync(this.previousGame);
        //this picks between game over and the next game depending on the comparison
        this.previousGame = destinationScene;
        var nextSceneLoad = SceneManager.LoadSceneAsync(destinationScene);
        nextSceneLoad.allowSceneActivation = false;

        //Step 3: Delays
        //If we want to manufacture any delays, the yields for them should go here
        yield return null;

        //Step 4: Transition
        nextSceneLoad.allowSceneActivation = true;

        //Step 5: Load the transition scene
        transitionLoad = SceneManager.LoadSceneAsync(transitionSceneIndex);
        transitionLoad.allowSceneActivation = false;
        

        //Transition done!
    }
}
