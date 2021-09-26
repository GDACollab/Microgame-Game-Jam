using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reroute_AppleScript : MonoBehaviour
{
    public Reroute_LevelController levelController;
    public Reroute_SnakeControl snakeController;
    // Start is called before the first frame update

    private void OnEnable()
    {
        levelController = GameObject.Find("LevelController").GetComponent<Reroute_LevelController>();
        snakeController = GameObject.Find("SnakeHeadControl").GetComponent<Reroute_SnakeControl>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            snakeController.GrowSnake();
            levelController.EatApple();

            Destroy(gameObject);
        }
    }
}
