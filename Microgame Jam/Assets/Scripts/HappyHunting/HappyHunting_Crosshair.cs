using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HappyHunting_Crosshair : MonoBehaviour
{
    //crosshair movement
    public float _speed = 1;
    public float _cooldown = 0.5f;

    //timer
    private float _maxTime = 15f;

    //audio
    public AudioSource _captureSound;
    public AudioSource _BGM;
    public AudioSource _Victory;

    //sprites and collision
    public GameObject[] _citizens;
    private bool _shot = false;
    private bool _found = false;
    private float _timer = 0;

    // Start is called before the first frame update
    void OnEnable()
    {
        //gathering citizens and setting the target
        _citizens = GameObject.FindGameObjectsWithTag("Citizen");
        int target = (int) Random.Range(0f, _citizens.Length - 1);
        _citizens[target].GetComponent<HappyHunting_Citizen>()._target = true;
        
        //difficulty
        if (GameController.Instance.gameDifficulty == 1)
        {
            _maxTime = 15f;
            GameController.Instance.SetMaxTimer(_maxTime);
        }
        else if (GameController.Instance.gameDifficulty == 2)
        {
            _maxTime = 10f;
            GameController.Instance.SetMaxTimer(_maxTime);
        }
        else if (GameController.Instance.gameDifficulty == 3)
        {
            _maxTime = 5f;
            GameController.Instance.SetMaxTimer(_maxTime);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_shot)
        {
            _timer -= Time.deltaTime;
            if (_timer < 0)
            {
                _shot = false;
            }
        }

        //moves the crosshair
        if (!_shot && !_found)
        {
            Vector3 direction = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);
            Vector3 finalPos = transform.position + direction;
            transform.position = Vector3.Lerp(transform.position, finalPos, Time.deltaTime * _speed);
        }

        //keeps in the boundary
        if (transform.position.x < -2.8f)
        {
            transform.position = new Vector3(-2.8f, transform.position.y, transform.position.z);
        }
        if (transform.position.x > 8.8f)
        {
            transform.position = new Vector3(8.8f, transform.position.y, transform.position.z);
        }
        if (transform.position.y < -5f)
        {
            transform.position = new Vector3(transform.position.x, -5f, transform.position.z);
        }
        if (transform.position.y > 5f)
        {
            transform.position = new Vector3(transform.position.x, 5, transform.position.z);
        }

        //taking the shot
        if (Input.GetKeyDown("space") && !_shot && !_found)
        {
            CheckForTarget();
            _timer = _cooldown;
            _shot = true;
            //Camera Sound
            _captureSound.Play();
        }

        //freeze everything if the timer runs out
        if (GameController.Instance.gameTime >= _maxTime - 0.5)
        {
            Debug.Log("Here");
            Freeze();
            _timer = _cooldown;
            _shot = true;
        }

    }

    //win game after a pause for a photo
    IEnumerator WinDelay(float time)
    {
        yield return new WaitForSeconds(time);
        GameController.Instance.WinGame();
    }

    //finding out if the target was correct
    void CheckForTarget()
    {
        foreach (GameObject citizen in _citizens)
        {
            if (citizen.GetComponent<HappyHunting_Citizen>()._target)
            {
                if(Mathf.Abs(citizen.transform.position.x - transform.position.x) < 1.15f && Mathf.Abs(citizen.transform.position.y - transform.position.y) < 1.15f)
                {
                    citizen.GetComponent<SpriteRenderer>().sprite = citizen.GetComponent<HappyHunting_Citizen>()._kissingSprite;
                    _found = true;
                    Freeze();
                    //Win Condition Here
                    _BGM.Stop();
                    _Victory.Play();
                    StartCoroutine(WinDelay(2)); // Change to any delay amount before the game ends!
                }
            }
        }
    }

    //stopping all citizens
    void Freeze()
    {
        foreach (GameObject citizen in _citizens)
        {
            //stops citizen
            citizen.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
            
        }
    }
}
