using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDropThatDungDung_DungControl : MonoBehaviour
{
    public Rigidbody2D rb;
    public int level = 0;
    public static float moveSpeed = 6f;

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
            // limit movement speed diagonally, so you move at 70% speed
            horizontal *= moveLimit;
            vertical *= moveLimit;
        } 

        rb.velocity = new Vector2(horizontal * moveSpeed, vertical * moveSpeed);
    }
}
