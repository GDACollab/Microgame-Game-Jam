using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarpSpeed_Player : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] private AudioSource playerAudio;
    public float speed;

    public WarpSpeed_MicrogameManager manager;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        //x = 0;
        float y = Input.GetAxis("Vertical");

        Vector2 dir = new Vector2(x, y);

        if (manager.gameOver == false)
        {
            rb.velocity = dir * speed;
        } else {
            rb.velocity = new Vector2(0, 0);
        }
        
    }
    void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.tag == "Collectible")
        {
            playerAudio.Play();
            manager.collected++;
        }
    }
}
