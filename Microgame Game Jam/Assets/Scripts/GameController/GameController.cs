using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class GameController : Singleton<GameController>
{
    ///Fields--------------------------------------------------------------------------------------
    //The amount of games that can be failed until the game is over
    //We might want to put this elsewhere but we can figure that out later
    public int maxFails { get; private set; } = 3;

    //The previous game that was played to make sure it doesn't get picked again
    protected int previousGame { get; set; } = 0;

    //The amount of microgames the player has failed
    public int gameFails { get; private set; } = 0;

    //The current Difficulty Rating. How this is calculated and when it updates is undecided
    public int gameDifficulty { get; protected set; } = 1;

    //How many seconds have passed since the game began
    public float gameTime { get; private set; } = 0f;

    // The maximum amount of time the player gets before the player loses.
    public float maxTime { get; private set; } = 20.0f;

    //How many games have been completed since the game began
    public int gameWins { get; private set; } = 0;

    //whether or not the game timer should be running
    public bool timerOn { get; private set; } = false;

    // Whether or not the SetTimer function has been called for this game.
    private bool timerSet = false;

    // How long to wait on a paused game screen before starting the game.
    protected float gameStartDelay = 2.0f;

    // How long to wait on a paused game before loading the next game.
    protected float endGameDelay = 2.0f;

    // Keeps track if WinGame or LoseGame has been called for this game.
    protected bool gameCanEnd = true;

    protected bool showGameObjects = true;

    protected Scene gameScene;


    ///Methods-------------------------------------------------------------------------------------
    // Start is called the frame before the scene begins
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (timerOn)
        {
            gameTime += Time.deltaTime;
            if (gameTime >= maxTime)
            {
                Debug.Log("Game time has exceeded 20 seconds! The game has been failed.");
                LoseGame();
            }
        }

        // Prevent any game objects from showing up if we don't want them to:
        if (!showGameObjects)
        {
            ActivateAllObjectsInScene(gameScene, showGameObjects);
        }
    }
    public void ActivateAllObjectsInScene(Scene scene, bool activate)
    {
        foreach (GameObject obj in scene.GetRootGameObjects())
        {
            obj.SetActive(activate);
        }
    }

    // Set all the objects in the scene to be active or not. If activate is false, the filterList will be filled with all the objects that are
    // currently active in the scene. If activate is true, the filterList will only activate objects in the provided filterList.
    public void ActivateAllObjectsInScene(Scene scene, bool activate, List<GameObject> filterList)
    {
        foreach (GameObject obj in scene.GetRootGameObjects())
        {
            if (activate == false && obj.activeSelf) {
                filterList.Add(obj);
            }
            
            if (activate == true && filterList.Contains(obj))
            {
                obj.SetActive(activate);
            } else if (activate == false) {
                obj.SetActive(activate);
            }
        }
    }

    //Called whenever a microgame is started
    protected void SceneInit()
    {
        // Make sure our next game can call WinGame or LoseGame:
        gameCanEnd = true;
        //turn on the game timer
        timerOn = true;
        gameTime = 0.0f;
    }

    //Starts the Game Conclusion after the game is won
    public void WinGame()
    {
        ConcludeGame(true);
    }

    //Starts the Game Conclusion after the game is lost
    public void LoseGame()
    {
        ConcludeGame(false);
    }

    /// <summary>
    /// Set the game's maximum amount of time before the player loses. Must be called BEFORE the game starts (call this in 
    /// a Start function somewhere), and can only be called ONCE.
    /// </summary>
    /// <param name="time">The time to set. The minimum amount of time you can set is 5 seconds, the maximum is 20 seconds.</param>
    public void SetMaxTimer(float time)
    {
        if (timerOn)
        {
            Debug.LogWarning("You called SetTimer(" + time + ") after the game started. Try calling SetTimer() during an active object's Start function.");
        }
        if (timerSet)
        {
            Debug.LogWarning("You called SetTimer(" + time + ") twice, after you already called it. Try calling SetTimer() only once.");
        }
        if (timerOn == false && timerSet == false)
        {
            timerSet = true;
            maxTime = Mathf.Clamp(time, 5.0f, 20.0f);
            Debug.Log("Maximum amount of time set to: " + time);
        }
    }

    void TearDownController(bool win)
    {
        //stop the game timer
        timerOn = false;

        if (gameCanEnd)
        {
            gameCanEnd = false;
            //calculate losses
            if (!win)
            {
                ++gameFails;
            }
            else
            {
                ++gameWins;
            }
        }
        else {
            Debug.LogWarning("You called " + ((win) ? "WinGame()" : "LoseGame()") + " multiple times. Try using GameController.Instance.timerOn to detect if the game is still running (if(timerOn){GameController.Instance.WinGame()}).");
        }
    }

    void ConcludeGame(bool win)
    {
        timerSet = false;
        TearDownController(win);
        gameDifficulty = Mathf.Clamp((gameWins - 1) % 5, 1, 3);
        LevelTransition();
    }

    protected abstract void LevelTransition();
}
