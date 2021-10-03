using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WinningDetector : MonoBehaviour
{
    private SpriteRenderer sprite;
    private Text TextPro;
    private bool onEnter = false;
    private bool NikoWin = false;
    private float dtotal = 0.0f;
    private int difficile = 1;
    // Start is called before the first frame update
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        TextPro = GameObject.Find("Tutorial").GetComponent<Text>();
        switch(GameController.Instance.gameDifficulty){
            case 1:
                difficile = 3;
                break;
            case 2:
                difficile = 5;
                break;
            case 3:
                difficile = 10;
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (onEnter) {
            TextPro.text = (GameController.Instance.gameTime - dtotal).ToString("0.000");
        } else {
            TextPro.text = "Keep Niko Centered!";
        }
    }

    void OnTriggerEnter2D(Collider2D col) {

        if (col.gameObject.name == "Circle") {
            sprite.color = new Color(0.0f, 1.0f, 0.0f, 1.0f);
            onEnter = true;
            dtotal = GameController.Instance.gameTime;
        }

    }

    void OnTriggerStay2D(Collider2D col){
        if (onEnter){
            if ((GameController.Instance.gameTime - dtotal) >= difficile ) {
                TextPro.text = "You Did It, Parmpreet!";
                if (!NikoWin) {
                    GameController.Instance.WinGame();
                    NikoWin = true;
                }
            }
        }
    }

    void OnTriggerExit2D(Collider2D col) {

        if (col.gameObject.name == "Circle") {
            sprite.color = new Color(1.0f, 0.0f, 0.0f, 1.0f);
            onEnter = false;
        }

    }
}
