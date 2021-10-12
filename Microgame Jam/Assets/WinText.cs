using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WinText : MonoBehaviour
{
    // Start is called before the first frame update
    void OnEnable()
    {
        var wins = GameController.Instance.gameWins;
        var text = "You won " + wins + " times!";
        if (wins >= 9 && wins <= 10)
        {
            text += "\nWow!";
        }
        else if (wins > 10 && wins <= 20)
        {
            text += "\nAmazing!";
        }
        else if (wins > 20 && wins <= 30)
        {
            text += "\nFantastic!";
        }
        else if (wins > 30 && wins <= 50)
        {
            text += "\nI don't believe it!";
        }
        else if (wins > 50 && wins <= 70)
        {
            text += "\nI don't believe it! No, seriously. I don't believe it. What did you <b>do</b>?";
        }
        else if (wins > 70 && wins <= 100)
        {
            text += "\nLet's call this a win for everyone. Now go. Leave. Be free!";
        }
        GetComponent<Text>().text = text;
        transform.GetChild(0).GetComponent<Text>().text = text;
    }
}
