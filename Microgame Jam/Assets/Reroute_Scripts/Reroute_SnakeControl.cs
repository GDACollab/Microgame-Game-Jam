using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reroute_SnakeControl : MonoBehaviour
{
    public static Reroute_SnakeControl Instance;

    public List<GameObject> snakeSegments;
    public Rigidbody2D rb;
    public GameObject segmentPrefab;
    public Vector2 currentVelocity;
    public Vector2 lastFramePos;

    public bool isSpeedingUp;
    public float speed;
    public float baseSpeed;
    public float gapFactor;
    public int currentDirection;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }


        if (GameController.Instance.gameDifficulty == 2)
        {
            baseSpeed += 0.25f;
        }

        if (GameController.Instance.gameDifficulty == 3)
        {
            baseSpeed += 0.5f;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (currentDirection != 1)
            {
                currentDirection = 3;
                transform.eulerAngles = new Vector3(0, 0, 180);
            }
                

            
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if(currentDirection != 3)
            {
                currentDirection = 1;
                transform.eulerAngles = new Vector3(0, 0, 0);
            }

            
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if(currentDirection != 2)
            {
                currentDirection = 0;
                transform.eulerAngles = new Vector3(0, 0, 90);
            }
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if(currentDirection != 0)
            {
                currentDirection = 2;
                transform.eulerAngles = new Vector3(0, 0, -90);
            }
        }

        

    }

    private void FixedUpdate()
    {

        if (isSpeedingUp)
        {
            rb.velocity = transform.right * speed;

            for (int i = 0; i < snakeSegments.Count; i++)
            {
                if (i != 0)
                {
                    snakeSegments[i].transform.position = Vector2.Lerp(snakeSegments[i].transform.position, snakeSegments[i - 1].transform.GetComponent<Reroute_SnakeSegment>().lastFramePos, 0.1f + gapFactor);
                    snakeSegments[i].transform.right = snakeSegments[i].transform.position - snakeSegments[i - 1].transform.position;
                    //snakeSegments[i].transform.position = ((Vector2)snakeSegments[i-1].transform.position - snakeSegments[i-1].GetComponent<Reroute_SnakeSegment>().lastFramePos) + (Vector2)snakeSegments[i-1].transform.position;
                }


            }


            gapFactor += 0.004f;
            speed += 0.01f;
        }

       

        

    }

    private void LateUpdate()
    {
        lastFramePos = transform.position;
    }



    public void GrowSnake()
    {
        var newSegment = Instantiate(segmentPrefab, snakeSegments[snakeSegments.Count-2].transform.position, Quaternion.identity);
        snakeSegments.Insert(snakeSegments.Count-2, newSegment);
    }
}
