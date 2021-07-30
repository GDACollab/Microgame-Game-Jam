using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreTracker : MonoBehaviour
{
    public bool showLives;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Text>().text = "You have\n"
            + (GameController.Instance.gameWins - 1) + " wins\n" + //Because we have to call .WinGame() to start transitions.
            (showLives? GameController.Instance.gameFails + " losses\n" : "") + 
            "and " + GameController.Instance.gamePoints + " points";
    }
}
