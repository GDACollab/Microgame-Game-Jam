using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChompyDino_GroundCheck : MonoBehaviour
{
    public bool isGrounded = true;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            isGrounded = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            isGrounded = false;
        }
    }
}
