using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PongJuggle_Ball2 : MonoBehaviour
{
    Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {  
        if(GameController.Instance.gameTime >= 0.1f)
        {
            rb.gravityScale = 1.0f;
        }
    }
}
