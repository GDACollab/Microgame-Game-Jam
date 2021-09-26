using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BirdFishing_HUDManager : MonoBehaviour
{
    [Header("Game Data")]
    public BirdFishing_GameData gameData;

    [Header("No. of Birds Caught Text")]
    public Text birdsCaughtText;
    public int birdsGoal;

    private void Start()
    {
        gameData.ScoreUpdate += UpdateText;

        UpdateText();
    }

    //---------------
    // UI Functions
    //---------------
    public void UpdateText()
    {
        int birdsGoal = 3;
        string newText = "<color=#b64199>" + gameData.birdsCaught + 
                        "/" + birdsGoal + "</color>";
        if( birdsCaughtText )
        {
            birdsCaughtText.text = newText;
        }
    }
}
