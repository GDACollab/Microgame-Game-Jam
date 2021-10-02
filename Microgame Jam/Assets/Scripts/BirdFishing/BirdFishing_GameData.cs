using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "BirdFishing_GameData", menuName = "ScriptableObjects/BirdFishing_GameData", order = 1)]
public class BirdFishing_GameData : ScriptableObject
{
    public int birdsCaught = 0;
    public int birdsGoal = 3;
    public float maxTime = 15;

    public List<int> birdsDifficultyGoalsList = new List<int>();

    public Action CaughtBird;
    public Action ScoreUpdate;
    public Action EndSound;

    public void OnCaughtBird()
    {
        birdsCaught++;
        CaughtBird?.Invoke();
        ScoreUpdate?.Invoke();
    }

    public void SetDifficulty( int gameDifficulty )
    {
        birdsGoal = birdsDifficultyGoalsList[gameDifficulty - 1];
        GameController.Instance.SetMaxTimer(maxTime);
    }

    public void ResetScore()
    {
        birdsCaught = 0;
        ScoreUpdate?.Invoke();
    }

    public void ResetEvents()
    {
        CaughtBird = null;
        ScoreUpdate = null;
        EndSound = null;
    }

    public void OnWinGame()
    {
        EndSound?.Invoke();
    }
}
