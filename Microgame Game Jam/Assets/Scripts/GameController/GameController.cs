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
    //Might need to be an int but we'll get there when we get there
    protected string previousGame { get; set; } = "";

    //The amount of microgames the player has failed
    public int gameFails { get; private set; } = 0;

    //The current Difficulty Rating. How this is calculated and when it updates is undecided
    public int gameDifficulty { get; protected set; } = 1;

    //How many points the player has
    public int gamePoints { get; set; } = 0;

    //How many seconds have passed since the game began
    public float gameTime { get; private set; } = 0f;

    //whether or not the game timer should be runnings
    private bool timerOn = false;

    ///Methods-------------------------------------------------------------------------------------
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(timerOn) gameTime += Time.deltaTime;
    }

    //Called whenever a microgame is started
    public void SceneInit()
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
    }

    void ConcludeGame(bool win)
    {
        TearDownController(win);
        LevelTransition();
    }

    protected abstract void LevelTransition();
}
