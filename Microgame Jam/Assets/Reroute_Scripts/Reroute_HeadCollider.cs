using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reroute_HeadCollider : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "SnakeSegment")
        {
            Reroute_LevelController.Instance.LoseViaCollision();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Terrain")
        {
            Reroute_LevelController.Instance.LoseViaWall();

        }
    }
}
