!!Warning!!
Neither of GameController's versions have been tested yet! Things may not behave correctly!

#Game Controllers and How to Use Them

###Used Classes
- Singleton.cs
- GameController.cs
- GameControllerDebug.cs
- GameControllerRelease.cs

###Singleton.cs

Copied directly from the [Unity Community Wiki](http://wiki.unity3d.com/index.php/Singleton). Documentation on function can be found there.

Tl;dr: Maintains a unique instance of a Monobehaviour that can be statically accessed at [ClassNameType].Instance. This instance will be attached to a Game Object that will persist across scenes.

###GameController.cs

GameController inherits from Singleton\<GameController\>, but is itself abstract, and cannot be instantiated. All of the data required by jammers is stored in this abstract class as properties of various accessibility level.

WinGame and LoseGame are called by the jammers when the microgame is won or lost. They both call ConcludeGame, which updates end-of-microgame data depending on if the microgame was won or lost. ConcludeGame then calls LevelTransition, which is abstract.

LevelTransition is implemented by GameControllerDebug and GameControllerRelease to transition to the next scene after a microgame is won.

###GameControllerDebug

Inherits from GameController. Implements LevelTransition to print a debug message indicating that the game is over and whether or not it was won or lost.

###GameControllerRelease

Inherits from GameController. Implements LevelTransition to pick a number from minSceneIndex to SceneManager.sceneCountInBuild-1 (inclusive) and then transitions to the scene at transitionSceneIndex and then to the selected scene. The transitionSceneIndex stop is for anything we want to put between microgames

///The below is for the async version, which doesnt work yet

start a transition coroutine; TransitionTiming.

TransitionTiming first transitions to the Transition Scene at transitionSceneIndex (which is assumed to always be loaded). It then asynchronously unloads the previous microgame and loads the next one. Once the new microgame is loaded, its build index is saved as the previous microgame and it is immediately transitioned to. However, if too many games have been failed, the scene at gameoverSceneIndex will be loaded and transitioned to instead.

Any special things that happen during the transition should happen while the next scene is loading.
