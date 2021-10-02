using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class nikorb : MonoBehaviour
{
    private Rigidbody2D rb;
    private float YCoord;
    private TextMeshProUGUI TextPro;
    private bool NikoDead = false;
    // Start is called before the first frame update
    void Start()
    {

        TextPro = GameObject.Find("Tutorial").GetComponent<TextMeshProUGUI>();
        float xCord = Random.Range(2.6f, 3.5f);

        if (Random.Range(0,10) % 2 == 0) {
            xCord = xCord * -1;
        }

        transform.position = new Vector3(xCord, 5, 0);


    }

    // Update is called once per frame
    void Update()
    {
        YCoord = transform.position.y;
        if (YCoord <= -6.5) {
            TextPro.text = "Ur bad";
            if (!NikoDead){
                GameController.Instance.LoseGame(); //remind Naman and co abt this
                NikoDead = true;
            }
        }
    }
}
