using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    private bool timerOn = false;

    // Whether or not the SetTimer function has been called for this game.
    private bool timerSet = false;

    // How long to wait on a paused game screen before starting the game.
    protected float gameStartDelay = 2.0f;

    // How long to wait on a paused game before loading the next game.
    protected float endGameDelay = 2.0f;


    ///Methods-------------------------------------------------------------------------------------
    // Start is called the frame before the scene begins
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if(timerOn) gameTime += Time.deltaTime;
        if(gameTime >= maxTime){
            Debug.Log("Game time has exceeded 20 seconds! The game has been failed.");
            LoseGame();
        }
    }

    //Called whenever a microgame is started
    protected void SceneInit()
    {
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
    public void SetMaxTimer(float time) {
        if (timerOn == false && timerSet == false)
        {
            timerSet = true;
            maxTime = Mathf.Clamp(time, 5.0f, 20.0f);
            Debug.Log("Maximum amount of time set to: " + time);
        }
        if (timerOn) {
            Debug.LogError("You called SetTimer(" + time + ") after the game started. Try calling SetTimer() during an active object's Start function.");
        }

        if (timerSet) {
            Debug.LogError("You called SetTimer(" + time + ") twice, after you already called it. Try calling SetTimer() only once.");
        }
    }

    void TearDownController(bool win)
    {
        //stop the game timer
        timerOn = false;

        //calculate losses
        if(!win)
        {
            ++gameFails;
        }
        else
        {
            ++gameWins;
        }
    }

    void ConcludeGame(bool win)
    {
        timerSet = false;
        TearDownController(win);
        gameDifficulty = Mathf.Clamp(gameWins % 5, 1, 3);
        LevelTransition();
    }

    protected abstract void LevelTransition();
}
