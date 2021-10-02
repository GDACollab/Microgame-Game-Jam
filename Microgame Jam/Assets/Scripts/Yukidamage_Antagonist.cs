using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Yukidamage_Antagonist : MonoBehaviour {

    public Yukidamage_Manager manager;
    private Rigidbody2D antagonistMovement;
    [SerializeField] float minSpawnHeight;
    [SerializeField] float maxSpawnHeight;

    // Start is called before the first frame update
    void Start() {
        GetComponent<SpriteRenderer>().enabled = false;
        antagonistMovement = GetComponent<Rigidbody2D>();
        antagonistMovement.transform.position = new Vector2(10, Random.Range(minSpawnHeight, maxSpawnHeight));
    }

    // Update is called once per frame
    void Update() {
        if (!manager.gameOver) {
            if (GameController.Instance.gameTime >= manager.timeToSpawnAntagonist) {
                GetComponent<SpriteRenderer>().enabled = true;
                antagonistMovement.velocity = new Vector2(-manager.speed, 0);
            }
        } else {
            antagonistMovement.velocity = new Vector2(0, 0);
        }
    }
}
