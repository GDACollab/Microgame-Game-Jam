using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class MeltyMeltyRunRun_Player : MonoBehaviour
{
    private Rigidbody2D rb;
    private Vector3 meltThreshold = new Vector3(0.5f, 0.5f, 0.5f);
    private bool melting = false;
    private bool facingLeft = false;
    public bool lost = false;
    private bool playerControl = false;
    private bool OOGABOOGA = false;

    public AudioClip die;
    
    public float speed;
    public Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerControl = true;
    }

    // Update is called once per frame
    void Update()
    {
        //Movement
        if (playerControl) {
            float x = Input.GetAxis("Horizontal");
            Vector2 dir = new Vector2(x,0);
            rb.velocity = dir * speed;

            if ((Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow)) && !GetComponent<AudioSource>().isPlaying) {
                GetComponent<AudioSource>().Play();
            } else if ((rb.velocity.x < 0.2 && rb.velocity.x > -0.2) && !(Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow))) {
                GetComponent<AudioSource>().Pause();
            }

            if (rb.velocity.x < 0 && !facingLeft) {
                transform.Rotate(new Vector3(0, 180, 0));
                facingLeft = true;
            } else if (facingLeft && rb.velocity.x > 0) {
                transform.Rotate(new Vector3(0, 180, 0));
                facingLeft = false;
            }
        }

        //Animation
        anim.SetFloat("Speed", rb.velocity.x);
        anim.SetBool("Lost", lost);

        //Lose Case 1 (melted)
        if (transform.localScale.x < meltThreshold.x && !lost) {
            lost = true;
            GetComponent<AudioSource>().clip = die;
            GetComponent<AudioSource>().loop = false;
            GetComponent<AudioSource>().Play();
            playerControl = false;
            melting = false;
            OOGABOOGA = true;
            meltThreshold.x -= 0.1f;
        }

        //if die animation is finished
        if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 2 && OOGABOOGA) {
            OOGABOOGA = false;
            GameController.Instance.LoseGame();
        }

        //Melting
        if (melting) {
            speed += transform.localScale.x / 400;
            float temp;
            if (GameController.Instance.gameDifficulty == 1) {
                temp = 1;
            } else if (GameController.Instance.gameDifficulty == 2) {
                temp = 0.75f;
            } else {
                temp = 0.6f;
            }
            transform.localScale -= transform.localScale / 450 * temp; 
            //difficulty = 1 stay the same, difficulty = 2 * 0.75, difficulty = 3 * 0.6 
        }
        
    }

    public void setMelt(bool melt) {
            melting = melt;
    }
}
