using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Reroute_LevelController : MonoBehaviour
{
    [Header("Preset Values")]
    public static Reroute_LevelController Instance;
    public GameObject audioOneshotPrefab;
    public BoxCollider2D appleSpawnBox;
    public GameObject applePrefab;
    public Image collectOxygenWarningImage;
    public TextMesh applesEatenText;
    public TextMesh timeRemainingText;
    public GameObject gameCamera;
    private Animator gameCameraAnim;
    public Animator handMasterAnim;
    public Animator asteroidAnim;

    [Header("Countdown Values")]

    public SpriteRenderer countdownNumber;
    public Sprite[] countdownSprites = new Sprite[3];

    [Header("Timers")]

    public float startInvincibilityTime;
    public bool isVulnerable;
    public float currentTurnInvincibleTime;
    public float baseInvincibleTime;
    public float snakeTimeRemaining;


    [Header("Apple State")]

    public int requiredApples;

    public int applesEaten;

    [Header("Apple State")]

    public List<AudioClip> soundList;

    [Header("Game States")]
    public bool canWin;
    public bool hasLost;


    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }



    // Start is called before the first frame update
    void Start()
    {
        gameCameraAnim = gameCamera.GetComponent<Animator>();
        StartCoroutine(StartingRoutine());
        snakeTimeRemaining = 15;
        canWin = true;

    }

    public IEnumerator StartingRoutine()
    {
        isVulnerable = false;
        currentTurnInvincibleTime = baseInvincibleTime;
        
        yield return new WaitForSeconds(0.3f);
        collectOxygenWarningImage.color = new Color(1, 1, 1, 1);
        countdownNumber.sprite = countdownSprites[1];
        yield return new WaitForSeconds(0.3f);
        collectOxygenWarningImage.color = new Color(1, 1, 1, 0);
        yield return new WaitForSeconds(0.3f);
        collectOxygenWarningImage.color = new Color(1, 1, 1, 1);
        countdownNumber.sprite = countdownSprites[2];
        yield return new WaitForSeconds(0.3f);
        collectOxygenWarningImage.color = new Color(1, 1, 1, 0);
        countdownNumber.color = new Color(1,1,1,0);
        Reroute_SnakeControl.Instance.speed = Reroute_SnakeControl.Instance.baseSpeed;
        Reroute_SnakeControl.Instance.isSpeedingUp = true;
        gameCameraAnim.Play("reroute_gameCamAnimate");

        yield return new WaitForSeconds(2f);
        handMasterAnim.Play("shift_left");

        /*
         * 
         * var pauseSound = Instantiate(audioOneshotPrefab, transform.position, Quaternion.identity);
            pauseSound.GetComponent<AudioSource>().clip = Resources.Load<AudioClip>("Audio/SFX/miscSound");
         * */

    }

    // Update is called once per frame
    void Update()
    {
        if(currentTurnInvincibleTime > 0)
        {
            currentTurnInvincibleTime -= Time.deltaTime;
            isVulnerable = false;
        }
        else
        {
            isVulnerable = true;
        }






    }

    private void FixedUpdate()
    {
        if (!hasLost && Reroute_SnakeControl.Instance.isSpeedingUp)
        {
            if (snakeTimeRemaining > 0)
            {
                snakeTimeRemaining -= Time.deltaTime;
            }

            if (snakeTimeRemaining > 9)
            {
                timeRemainingText.text = "00:" + (int)snakeTimeRemaining;
            }
            else
            {
                timeRemainingText.text = "00:0" + (int)snakeTimeRemaining;
            }
        }

        if(snakeTimeRemaining <= 0)
        {
            if (!hasLost)
            {
                hasLost = true;
                StartCoroutine(LoseRoutine());
            }
        
        }

    }

    public void EatApple()
    {
        applesEaten += 1;

        var appleSound = Instantiate(audioOneshotPrefab, transform.position, Quaternion.identity);
        appleSound.GetComponent<AudioSource>().clip = soundList[2];
        appleSound.GetComponent<AudioSource>().volume = 0.05f;

        applesEatenText.text = applesEaten + "/" + requiredApples;
        if(applesEaten < requiredApples)
        {
            SpawnNewApple();
        }
        else
        {
            if (canWin)
            {
                StartCoroutine(WinRoutine());
            }
            
        }


        

    }

    public IEnumerator WinRoutine()
    {

        gameCameraAnim.SetBool("isWin", true);
        yield return new WaitForSeconds(1.5f);

        asteroidAnim.Play("reroute_asteroid_move");
        //yield return new WaitForSeconds(1f);


        var appleSound = Instantiate(audioOneshotPrefab, transform.position, Quaternion.identity);
        appleSound.GetComponent<AudioSource>().clip = soundList[3];

        yield return new WaitForSeconds(0.3f);

        //var glassBreak = Instantiate(audioOneshotPrefab, transform.position, Quaternion.identity);
        //glassBreak.GetComponent<AudioSource>().clip = soundList[4];
        yield return new WaitForSeconds(0.55f);

        GameController.Instance.WinGame();
    }

    public IEnumerator LoseRoutine()
    {
        canWin = false;

        var loseSound = Instantiate(audioOneshotPrefab, transform.position, Quaternion.identity);
        loseSound.GetComponent<AudioSource>().clip = soundList[0];

        gameCameraAnim.SetBool("isLose", true);
        yield return new WaitForSeconds(2.75f);

        var thumpSound = Instantiate(audioOneshotPrefab, transform.position, Quaternion.identity);
        thumpSound.GetComponent<AudioSource>().clip = soundList[1];

        yield return new WaitForSeconds(1.5f);

        GameController.Instance.LoseGame();
    }

    public void SpawnNewApple()
    {
        var newApple = Instantiate(applePrefab, new Vector3(Random.Range(appleSpawnBox.bounds.min.x, appleSpawnBox.bounds.max.x), Random.Range(appleSpawnBox.bounds.min.y, appleSpawnBox.bounds.max.y), 0), Quaternion.identity);
     

    }

    public void LoseViaCollision()
    {
        if (isVulnerable )
        {
            print("lost via collision");

            if (!hasLost)
            {
                hasLost = true;
                StartCoroutine(LoseRoutine());
            }
        }

        
    }

    public void LoseViaWall()
    {
        if (isVulnerable)
        {
            print("lost via wall");
            if (!hasLost)
            {
                hasLost = true;
                StartCoroutine(LoseRoutine());
            }
        }


    }
}
