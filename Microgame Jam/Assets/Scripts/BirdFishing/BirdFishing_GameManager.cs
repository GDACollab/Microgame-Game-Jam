using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BirdFishing_GameManager : MonoBehaviour
{
    [Header("Game Data + Misc. Managers")]
    public BirdFishing_GameData gameData;

    private void Awake()
    {
        gameData.ResetScore();
        gameData.ResetEvents();
    }

    private void OnEnable()
    {
        //Define game difficulty
        gameData.SetDifficulty( GameController.Instance.gameDifficulty );

        //Define game events
        gameData.CaughtBird += CheckIfWin;
    }

    //------------------------
    // Bird Caught Functions
    //------------------------

    public void CheckIfWin()
    {
        if (gameData.birdsCaught >= gameData.birdsGoal)
        {
            //play sfx

            gameData.OnWinGame();
            gameData.ResetEvents();

            Debug.Log("You won the Bird Fishing Game! :pochi:");
            GameController.Instance.WinGame();
        }
    }
}
