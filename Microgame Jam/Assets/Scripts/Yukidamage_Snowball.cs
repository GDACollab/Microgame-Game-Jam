using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Yukidamage_Snowball : MonoBehaviour
{
    private Rigidbody2D rb;
    public Yukidamage_Manager manager;
    [SerializeField] private float speed;
    [SerializeField] private float size;
    [SerializeField] Sprite impactSprite;
    [SerializeField] AudioClip impactSound;

    private void OnEnable() {
        rb = GetComponent<Rigidbody2D>();
        speed += 0.5f * (GameController.Instance.gameDifficulty - 1f);
    }
    // Update is called once per frame
    void Update() {
        if (GameController.Instance.timerOn)
        {
            float y = Input.GetAxis("Vertical");
            Vector2 dir = new Vector2(gameObject.transform.position.x, y);
            rb.velocity = dir * speed;
            transform.localScale = new Vector2((size * GameController.Instance.gameTime * (GameController.Instance.gameDifficulty * 0.25f + 0.75f) + 0.5f) / 2f, (size * GameController.Instance.gameTime * (GameController.Instance.gameDifficulty * 0.25f + 0.75f) + 0.5f) / 2f);
        }
    }

    private void OnCollisionEnter2D(Collision2D col) {
        if (SceneManager.GetActiveScene().name == "Yukidamage")
        {
            if (col.gameObject.tag == "Target" || col.gameObject.tag == "Obstacle")
            {
                GetComponent<SpriteRenderer>().sprite = impactSprite;
                GetComponent<AudioSource>().PlayOneShot(impactSound);
                manager.EndGame();
                rb.velocity = new Vector2(0, 0);
                if (col.gameObject.tag == "Target")
                {
                    GameController.Instance.WinGame();
                }
                else if (col.gameObject.tag == "Obstacle" && GameController.Instance.timerOn)
                {
                    GameController.Instance.LoseGame();
                }
            }
        }
    }

}
