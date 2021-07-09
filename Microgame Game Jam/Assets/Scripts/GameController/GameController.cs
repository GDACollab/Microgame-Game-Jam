using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameController : Singleton<GameController>
{
    ///Fields--------------------------------------------------------------------------------------
    //The amount of games that can be failed until the game is over
    //We might want to put this elsewhere but we can figure that out later
    protected int maxFails { get; private set; } = 3;

    //The previous game that was played to make sure it doesn't get picked again
    protected int previousGame { get; set; } = 0;

    //The amount of microgames the player has failed
    public int gameFails { get; private set; } = 0;

    //The current Difficulty Rating. How this is calculated and when it updates is undecided
    public int gameDifficulty { get; protected set; } = 1;

    //How many points the player has
    public int gamePoints { get; set; } = 0;

    //How many seconds have passed since the game began
    public float gameTime { get; private set; } = 0f;

    //How many games have been completed since the game began
    public int gameWins { get; private set; } = 0;

    //whether or not the game timer should be runnings
    private bool timerOn = false;

    ///Methods-------------------------------------------------------------------------------------
    // Update is called once per frame
    void Update()
    {
        if(timerOn) gameTime += Time.deltaTime;
    }

    //Called whenever a microgame is started
    protected void SceneInit()
    {
        //turn on the game timer
        timerOn = true;
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
        TearDownController(win);
        LevelTransition();
    }

    protected abstract void LevelTransition();
}
