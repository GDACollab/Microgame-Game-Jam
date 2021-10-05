using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class PongJuggle_Paddle : MonoBehaviour
{
    public Vector3 easyScale;
    public Vector3 mediumScale;
    public Vector3 hardScale;
    public float speed;
    AudioSource audioSource;
    Rigidbody2D rb;
    PongJuggle_Tween tween;
    public GameObject yo;
    public GameObject hand1;
    public GameObject hand2;
    public GameObject hand3;
    public GameObject hand4;
    public GameObject loseBox;

    private void OnEnable()
    {
        GameController.Instance.SetMaxTimer(20);

        tween = GetComponent<PongJuggle_Tween>();
        audioSource = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody2D>();

        if(GameController.Instance.gameDifficulty == 1)
        {
            transform.localScale = easyScale;
        } else if(GameController.Instance.gameDifficulty == 2)
        {
            transform.localScale = mediumScale;
        } else if(GameController.Instance.gameDifficulty == 3)
        {
            transform.localScale = hardScale;
        }
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        rb.velocity = (new Vector2(Input.GetAxis("Horizontal") * speed, 0f));
        
        if(GameController.Instance.gameTime >= (3 + (GameController.Instance.gameDifficulty * 2)))
        {
            yo.SetActive(true);
            hand1.SetActive(true);
            hand2.SetActive(true);
            hand3.SetActive(true);
            hand4.SetActive(true);
            loseBox.SetActive(false);
            tween.enabled = true;
        }

        if(GameController.Instance.gameTime >= 7 + (GameController.Instance.gameDifficulty * 2))
        {
            GameController.Instance.WinGame();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        audioSource.Play();
    }
}
