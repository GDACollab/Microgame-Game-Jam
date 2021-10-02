using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DDC_ShowInstructions : MonoBehaviour
{

    public float timeLeft = 1.5f;
    public Image background;
    public Image[] icons;
    private Text text;

    private Color backgroundColor;
    private Color textColor;

    // Start is called before the first frame update
    void Start()
    {
        text = GetComponentInChildren<Text>();
        backgroundColor = background.color;
        textColor = text.color;
    }

    // Update is called once per frame
    void Update()
    {
        timeLeft -= Time.deltaTime;

        background.color =
            new Color(background.color.r,
                background.color.g,
                background.color.b,
                Mathf.Clamp(timeLeft, 0 , background.color.a)
                );

        for (int i = 0; i < icons.Length; i++)
        {
            icons[i].color =
                new Color(icons[i].color.r,
                    icons[i].color.g,
                    icons[i].color.b,
                    Mathf.Clamp(timeLeft, 0, icons[i].color.a)
                    );
        }
        

        text.color =
            new Color(text.color.r,
                text.color.g,
                text.color.b,
                Mathf.Clamp(timeLeft, 0, text.color.a)
                );
    }
}
