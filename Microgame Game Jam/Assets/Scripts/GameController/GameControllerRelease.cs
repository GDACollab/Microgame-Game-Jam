using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameControllerRelease : GameController
{
    [Tooltip("The lowest scene index that is an actual microgame. It is assumed" + 
        " that all scenes minSceneIndex through SceneManager.sceneCount-1 are microgames.")]
    public int minSceneIndex;

    [Tooltip("The index of the transition scene.")]
    public int transitionSceneIndex;

    [Tooltip("The index of the Game Over scene.")]
    public int gameoverSceneIndex;

    //Picks a random level in the build order then transitions to it
    protected override void LevelTransition()
    {
        StartCoroutine(TransitionTiming(Random.Range(minSceneIndex, SceneManager.sceneCount)));
    }

    //The timing of the actual scene transition
    //It's a coroutine to handle timing of effects because I doubt this transition 
    //should be instant
    IEnumerator TransitionTiming(int destinationScene)
    {
        //Step 0: If this scene is the previous scene... uhhh... pick again?
        while(destinationScene == this.previousGame && minSceneIndex != SceneManager.sceneCount - 1)
        {
            destinationScene = Random.Range(minSceneIndex, SceneManager.sceneCount);
        }

        //Step 1: Transition to the Transition Scene, which is always loaded
        SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(transitionSceneIndex));

        //Step 2: Unload the previous scene AND load the next scene
        SceneManager.UnloadSceneAsync(this.previousGame);
        //this picks between game over and the next game depending on the comparison
        var nextSceneLoad = SceneManager.LoadSceneAsync(
            this.gameFails >= this.maxFails ? gameoverSceneIndex : destinationScene,
            LoadSceneMode.Additive
        );

        //Step 3: Wait for the scene to finish loading
        //Replace this step with any fun visuals idk
        while(!nextSceneLoad.isDone) yield return null;

        //Step 4: Switch scenes and make the destination the previous game
        SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(destinationScene));
        this.previousGame = destinationScene;

        //Transition done!
    }
}
