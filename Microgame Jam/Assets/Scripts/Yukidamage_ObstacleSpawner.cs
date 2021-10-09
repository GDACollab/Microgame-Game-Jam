using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Yukidamage_ObstacleSpawner : MonoBehaviour {

    [SerializeField] GameObject tree;
    [SerializeField] GameObject rock;
    public Yukidamage_Manager manager;

    [SerializeField] float minSpawnHeight;
    [SerializeField] float maxSpawnHeight;
    float minSpawnTime;
    float maxSpawnTime;

    private bool waitingOnSpawn = false;
    private float timeToSpawn;
    private bool death = false;

    // Start is called before the first frame update
    void Start() {
        minSpawnTime = 0.75f - (0.055f * GameController.Instance.gameDifficulty);
        maxSpawnTime = 1.5f - (0.11f * GameController.Instance.gameDifficulty);
    }

    // Update is called once per frame
    void Update() {
        if (!waitingOnSpawn) {
            timeToSpawn = GameController.Instance.gameTime + RandomizeTime();
            waitingOnSpawn = true;
        }
        if (GameController.Instance.gameTime >= timeToSpawn && GameController.Instance.gameTime <= manager.timeToSpawnAntagonist - 0.5f) {
            SpawnObstacle();
            waitingOnSpawn = false;
        }
        if (GameController.Instance.gameTime >= manager.timeToSpawnAntagonist + 1 && !death) {
            SpawnDeath();
            death = true;
        }
    }

    private void SpawnObstacle() {
        Vector2 spawnLocation = RandomizeLocation();
        bool isTree = (Random.Range(0f, 1f) < 0.5);
        if (isTree) {
            Instantiate(tree, spawnLocation, Quaternion.identity, this.transform);
        } else {
            Instantiate(rock, spawnLocation, Quaternion.identity, this.transform);
        }
    }

    private Vector2 RandomizeLocation() {
        return new Vector2(10f, Random.Range(minSpawnHeight, maxSpawnHeight)); // x-value of 10 is slightly offscreen
    }

    private float RandomizeTime() {
        return Random.Range(minSpawnTime, maxSpawnTime);
    }

    private void SpawnDeath() {
        for (float i = 4.5f; i >= -4.5f; i -= 2f) {
            Vector2 treeLocation = new Vector2(10f, i);
            Instantiate(rock, treeLocation, Quaternion.identity, this.transform);
        }
    }
}
