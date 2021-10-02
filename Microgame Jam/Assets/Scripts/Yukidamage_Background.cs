using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Yukidamage_Background : MonoBehaviour {

    public Yukidamage_Manager manager;
    private Rigidbody2D bgMovement;

    // Start is called before the first frame update
    void Start() {
        bgMovement = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update() {
        if (!manager.gameOver) {
            bgMovement.velocity = new Vector2(-manager.speed, 0);
            if (gameObject.transform.position.x <= (-17.5f - (GameController.Instance.gameDifficulty - 1f))) {
                gameObject.transform.position = new Vector3((17.5f - (GameController.Instance.gameDifficulty - 1f)), gameObject.transform.position.y, gameObject.transform.position.z);
            }
        } else {
            bgMovement.velocity = new Vector2(0, 0);
        }
    }
}
