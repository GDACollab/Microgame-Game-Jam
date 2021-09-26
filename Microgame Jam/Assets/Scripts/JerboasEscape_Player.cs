using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JerboasEscape_Player : MonoBehaviour
{
    private Rigidbody2D rb;
    
    public float speed;

    public float jumpSpeed;

    public JerboasEscape_MicrogameManager manager;

    public Animator animator;

    public bool HoldLeft = false;

    public int tokencnt = 0;

    public AudioClip Filesound;

    public Text FileText;
    
    //public bool Jump;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        float x = Input.GetAxis("Horizontal");

        
        animator.SetFloat("Speed", x);
        if (x < 0)
        {
            HoldLeft = true;
            //GetComponent<AudioSource>().volume = Random.Range(0.8f, 1);
            //GetComponent<AudioSource>().pitch = Random.Range(0.8f, 1.1f);
            //GetComponent<AudioSource>().Play();
        }else if (x > 0){
            HoldLeft = false;
        }
        animator.SetBool("Left",HoldLeft);
        
        
        transform.position += new Vector3(x, 0, 0) * Time.deltaTime * speed;

        if (Input.GetKey (KeyCode.D) || Input.GetKey (KeyCode.A))
        {
            //GetComponent<AudioSource>().volume = 0.4f;
            GetComponent<AudioSource>().UnPause();
        }else{
            GetComponent<AudioSource>().Pause();
        }

        if (Input.GetButtonDown("Jump") && Mathf.Abs(rb.velocity.y) < 0.001f)
        {
            rb.AddForce(new Vector2(0, jumpSpeed), ForceMode2D.Impulse);
            //Jump = true;
            animator.SetBool("IsJumping", true);
        }else{
            animator.SetBool("IsJumping",false);
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Collectible")
        {
            manager.collected++;
            Destroy(col.gameObject);
        }

        if (col.gameObject.tag == "Tokens")
        {
            tokencnt = tokencnt + 1;
            Debug.Log("YOU HAVE " + tokencnt + " TOKENS");
            FileText.text = tokencnt + " / 7";
            AudioSource.PlayClipAtPoint(Filesound, transform.position);
            Destroy(col.gameObject);
        }
    }

}
