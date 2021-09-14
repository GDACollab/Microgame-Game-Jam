using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PongJuggle_Ball : MonoBehaviour
{
    Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        transform.position = new Vector3(Random.Range(-10.0f, -6.0f), 4.3f, 0f);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(GameController.Instance.gameTime >= 1)
        {
            rb.gravityScale = 1.1f;
        }
    }
}
