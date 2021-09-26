using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DontDropThatDungDung_Score : MonoBehaviour
{
    public GameController gc;
    public static int scoreAmount;
    private Text scoreText;

    // Start is called before the first frame update
    void Start()
    {
        scoreText = GetComponent<Text>();
        scoreAmount = 0;
    }

    void Update()
    {
        if(scoreAmount == 1600)
        {
            gc.WinGame();
            Debug.Log("Game Won");
            scoreAmount = 0;
        }
    }

}
