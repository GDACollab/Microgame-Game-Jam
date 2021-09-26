using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HappyHunting_Citizen : MonoBehaviour
{
    public float _speed = 1f;
    public bool _target = false;
    public Sprite _kissingSprite;

    //wall bouncing variables
    //referencing video for characters bouncing on walls
    //https://www.youtube.com/watch?v=RoZG5RARGF0
    private Rigidbody2D _rigidBody;
    private Vector3 _lastVelocity;

    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody2D>();    
    }

    // Start is called before the first frame update
    void Start()
    {
        Vector3 direction = new Vector3(Random.value, Random.value, 0);
        direction = direction.normalized;
        _rigidBody.velocity = direction * _speed;

        //citizens get smaller and faster at higher difficulties
        if (GameController.Instance.gameDifficulty == 1)
        {
            _speed = 5f;
            transform.localScale = new Vector3(1, 1, 1) * Random.Range(0.6f, 2.5f);
        }
        else if (GameController.Instance.gameDifficulty == 2)
        {
            _speed = 7f;
            transform.localScale = new Vector3(1, 1, 1) * Random.Range(0.6f, 1.75f);
        }
        else if (GameController.Instance.gameDifficulty == 3)
        {
            _speed = 9f;
            transform.localScale = new Vector3(1, 1, 1) * Random.Range(0.6f, 1f);
        } 
        
    }

    // Update is called once per frame
    void Update()
    {
        _lastVelocity = _rigidBody.velocity;  
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall") || collision.gameObject.CompareTag("Citizen"))
        {
            //referencing video for characters bouncing on walls
            //https://www.youtube.com/watch?v=RoZG5RARGF0
            float speed = _lastVelocity.magnitude;
            Vector3 direction = Vector3.Reflect(_lastVelocity.normalized, collision.contacts[0].normal);

            _rigidBody.velocity = direction * Mathf.Max(speed, 0f);

        }
        
    }
}
