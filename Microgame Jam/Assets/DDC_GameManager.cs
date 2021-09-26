using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DDC_GameManager : MonoBehaviour
{
    
    public GameObject prefab;
    public GameObject squareWall;
    public GameObject rectWall;
    DDC_DustControl dustControl;
    

    public int diffScore;
    private Vector3 pos;
    public static int total;
    public int random;
    public Quaternion rot;

    public List<Transform> tablePositions;
    public List<Transform> chairPositions;

    void Start()
    {
        dustControl = GameObject.Find("DDC_Dust").GetComponent<DDC_DustControl>();

        // Set the number of dust patches based on difficulty level
        if (GameController.Instance.gameDifficulty == 1)
        {
            diffScore = 10;
        }
        else if (GameController.Instance.gameDifficulty == 2)
        {
            diffScore = 15;
        }
        else if (GameController.Instance.gameDifficulty == 3)
        {
            diffScore = 20;
        }

        bool placedTable = false;

        // Spawn the walls in different spots around the room
        for (int i = 1; i <= diffScore/5; i++)
        {
            

            // Pick a random rotation
            random = Random.Range(1, 3);
            rot = Quaternion.Euler(0, 0, Random.Range(0, 360));

            // Choose square or rect
            if (random == 1 || placedTable)
            {
                // Pick a position from one of the set positions
    
                int randomPos = Random.Range(0, chairPositions.Count);
                pos = chairPositions[randomPos].position;
                chairPositions.RemoveAt(randomPos);            

                Instantiate(squareWall, pos, rot, transform);
            }
            else
            {
                // Pick a position from one of the set positions
                int randomPos = Random.Range(0, tablePositions.Count);
                pos = tablePositions[randomPos].position;
                tablePositions.RemoveAt(randomPos);

                Instantiate(rectWall, pos, rot, transform);
                placedTable = true;
            }
        }

        // Spawn the dust in random spots around the room
        for (int i = 1; i < diffScore; i++)
        {
            rot = Quaternion.Euler(0, 0, Random.Range(0, 360));
            pos = new Vector3(Random.Range(-8.3f, 8.3f), Random.Range(-4.5f, 4.5f), -.1f);
            Instantiate(prefab, pos, rot, transform);
        }

        

    }

    // Update is called once per frame
    void Update()
    {

        total = GameObject.FindGameObjectsWithTag("Dust").Length;

        // If we collected all the dust, win the game
        if(total <= 1 && GameController.Instance.timerOn && Time.timeSinceLevelLoad > 5f)
        {
            GameController.Instance.WinGame();
        }
    }
}
