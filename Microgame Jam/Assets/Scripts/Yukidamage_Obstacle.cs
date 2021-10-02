using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Yukidamage_Obstacle : MonoBehaviour {

    public Yukidamage_Manager manager;
    private Rigidbody2D obstacleMovement;

    // Start is called before the first frame update
    void Start() {
        obstacleMovement = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update() {
        if (!manager.gameOver) {
            obstacleMovement.velocity = new Vector2(-manager.speed, 0);
            if (Offscreen()) {
                Destroy(gameObject);
            }
        } else {
            obstacleMovement.velocity = new Vector2(0, 0);
        }
    }

    private bool Offscreen() {
        return (gameObject.transform.position.x < -10);
    }
}
