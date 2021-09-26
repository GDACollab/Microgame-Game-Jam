using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reroute_AppleScript : MonoBehaviour
{
    // Start is called before the first frame update

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            Reroute_SnakeControl.Instance.GrowSnake();
            Reroute_LevelController.Instance.EatApple();

            Destroy(gameObject);
        }
    }
}
