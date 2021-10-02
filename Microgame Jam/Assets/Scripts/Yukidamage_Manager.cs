using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Yukidamage_Manager : MonoBehaviour {

    [SerializeField] public float speed;
    [SerializeField] public float timeToSpawnAntagonist;
    public bool gameOver = false;

    void Awake() {
        GameController.Instance.SetMaxTimer(20f);
    }

    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        speed += 0.005f * GameController.Instance.gameDifficulty;
    }

    public void EndGame() {
        gameOver = true;
    }
}
