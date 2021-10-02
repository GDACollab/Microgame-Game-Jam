using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungControl : MonoBehaviour
{
    public Rigidbody2D rb;
    public float moveSpeed = 5f;

    float horizontal;
    float vertical;
    float moveLimit = 0.7f;

    // Update is called once per frame
    void Update()
    {
        // Input
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");
    }

    void FixedUpdate()
    {
        // Movement
        if (horizontal != 0 && vertical != 0) // Check for diagonal movement
        {
            // limit movement speed diagonally, so you move at ##% speed
            horizontal *= moveLimit;
            vertical *= moveLimit;
        } 

        rb.velocity = new Vector2(horizontal * moveSpeed, vertical * moveSpeed);
    }

    // void OnTriggerEnter2D(Collider2D col)
    // {
    //     if (col.gameObject.tag == "Collectible") 
    //     {
    //         switch (col.name)
    //         {
    //             case "Poop1":
    //                 Debug.Log(col.gameObject.name);
    //                 DontDropThatDungDung_Score.scoreAmount += 100;
    //                 Destroy(col.gameObject);
    //                 break;
    //             case "Poop2":
    //                 Debug.Log(col.gameObject.name);
    //                 DontDropThatDungDung_Score.scoreAmount += 200;
    //                 Destroy(col.gameObject);
    //                 break;
    //         }
    //     }
    // }
}
