using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChompyDino_PlayerMovement : MonoBehaviour
{
    public float accelRate;
    public float maxSpeed;
    public float slowRate;
    public Vector2 jumpVector;
	public ChompyDino_GameController controller;
	
    public float horizontalJumpForce;
    public float verticalJumpForce;
    public float maxJumpTime;

    [SerializeField] private ChompyDino_GroundCheck groundCheck; 
    public Rigidbody2D rb;
    private SpriteRenderer playerSprite;

    private float inputX;
    private Vector2 moveVector;
    private Vector2 horizontalMovement;
    private Vector2 moveDirection;
	private GameObject player;
	
	[SerializeField] private AudioSource sound;

    private bool isJumping;
    private float jumpTime;
	private bool playJump;
	

    private float speedIncrease;

    private void Awake()
    {
		playJump = true;
        rb = gameObject.GetComponent<Rigidbody2D>();
        playerSprite = gameObject.GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        speedIncrease = GameController.Instance.gameDifficulty
            + GameController.Instance.gameTime;

        inputX = Input.GetAxisRaw("Horizontal");
        moveDirection = new Vector2(rb.velocity.x, 0);
        moveDirection.Normalize();

        if (groundCheck.isGrounded)
        {
            jumpTime = 0;
        }

        if (Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.UpArrow))
        {
            Jump();
        }

        if (Input.GetKeyUp(KeyCode.Space) || Input.GetKeyUp(KeyCode.UpArrow))
        {
            isJumping = false;
        }

        if (moveDirection.x > 0)
        {
            playerSprite.flipX = true;
        }
        else
        {
            playerSprite.flipX = false;
        }
    }

    private void FixedUpdate()
    {
        if (inputX != 0)
        {
            Move(inputX);
        }
        else
        {
            moveVector = Vector2.zero;
            Slow();
        }
    }

    private void Move(float direction)
    {
        if (Mathf.Abs(rb.velocity.x) > maxSpeed)
        {
            float newMaxSpeed = speedIncrease * 0.2f * maxSpeed;
            rb.velocity = new Vector2(newMaxSpeed   * moveDirection.x, rb.velocity.y);
        }
        else
        {
            float newAccelRate = speedIncrease * 1.0f * accelRate;
            horizontalMovement = new Vector2(direction * newAccelRate, 0);
            moveVector += horizontalMovement * Time.fixedDeltaTime;

            rb.velocity += moveVector;
        }
    }

    private void Slow()
    {
        Vector2 slowVector = moveDirection * slowRate * Time.fixedDeltaTime;

        if (Mathf.Abs(rb.velocity.x) < 1)
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
        }
        else
        {
            rb.velocity -= slowVector;
        }
    }

    private void Jump()
    {
		if (playJump == true)
		{
           sound.Play();
           playJump = false;		   
		}
        if(jumpTime < maxJumpTime)
        {
            isJumping = true;
            rb.velocity += jumpVector;
            jumpTime += Time.deltaTime;
        }
        else
        {
            isJumping = false;
        }

        if(groundCheck.isGrounded)
        {
            Vector2 horizontalJump = new Vector2(horizontalJumpForce * inputX, 0);
            Vector2 totalJump = horizontalJump + verticalJumpForce * Vector2.up;
            rb.AddForce(totalJump, ForceMode2D.Force);
			playJump = true;
        }
    }
	
	private void OnTriggerEnter2D (Collider2D col)
	{
		if (col.tag == "Chompy")
	    {
			controller.lives--;
		}
	}
}
