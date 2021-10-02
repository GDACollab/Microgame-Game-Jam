using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LowBandwidth_Movement : MonoBehaviour
{
	
	public Rigidbody2D rb;
	public Animator animator;
	bool moveLeft;
	bool moveRight;
	bool moveUp;
	bool moveDown;
	
	bool isInIntersection = false;
	bool canTurnDown;
	bool canTurnUp;
	bool canTurnLeft;
	bool canTurnRight;
	
	float setSpeed = 30f;
	
	string facing = "right";
	
	[SerializeField] private Transform downCheck;							//these check if you're near a wire
	[SerializeField] private Transform upCheck;
	[SerializeField] private Transform leftCheck;
	[SerializeField] private Transform rightCheck;
	[SerializeField] private LayerMask m_WhatIsWire;
	[SerializeField] private Transform inIntersection;								//checks if you're in an intersection
	[SerializeField] private LayerMask m_WhatIsIntersection;
	[SerializeField] private Transform wireCheckLeft;
	[SerializeField] private Transform wireCheckRight;
	[SerializeField] private Transform wireCheckUp;
	[SerializeField] private Transform wireCheckDown;
	[SerializeField] private LayerMask m_WhatIsWinZone;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
		animator.SetInteger("FaceDirection", 1);
    }

    // Update is called once per frame
    void Update()
    {
		if (Physics2D.OverlapCircle(inIntersection.position, 0.2f, m_WhatIsWinZone) == true)
		{
			rb.velocity = new Vector2(0f, 0f);
			moveLeft = false;
			moveRight = false;
			moveUp = false;
			moveDown = false;
			animator.SetBool("isTouchingWin", true);
			animator.SetBool("isTouchingWin2", true);
			animator.SetBool("isTouchingWin3", true);
			if (GameController.Instance.timerOn)
			{
				GameController.Instance.WinGame();
			}
		}
		
		if (Physics2D.OverlapCircle(inIntersection.position, 0.2f, m_WhatIsWire) == false)
		{
			rb.velocity = new Vector2(0f, 0f);
			moveLeft = false;
			moveRight = false;
			moveUp = false;
			moveDown = false;
		}
		
		isInIntersection = Physics2D.OverlapCircle(inIntersection.position, 0.3f, m_WhatIsIntersection);
		
		if (rb.velocity.x == 0 && rb.velocity.y == 0)
		{
			animator.SetBool("isMoving", false);
		}
		else
		{
			animator.SetBool("isMoving", true);
		}
		
		if (isInIntersection)
		{
			//since you continue moving for as long as a boolean is true, I gotta turn em off when you switch direction (why I'm using bools explained below)
			if (Input.GetAxisRaw("Horizontal") == -1f)
			{
				moveLeft = true;
				moveRight = false;
				moveUp = false;
				moveDown = false;
			}
			if (Input.GetAxisRaw("Horizontal") == 1f)
			{
				moveLeft = false;
				moveRight = true;
				moveUp = false;
				moveDown = false;
			}
			if (Input.GetAxisRaw("Vertical") == 1f)
			{
				moveLeft = false;
				moveRight = false;
				moveUp = true;
				moveDown = false;
			}
			if (Input.GetAxisRaw("Vertical") == -1f)
			{
				moveLeft = false;
				moveRight = false;
				moveUp = false;
				moveDown = true;
			}
		}
		if (isInIntersection && (moveDown || moveLeft || moveRight || moveUp))
		{
			Move();
		}
		if (!Physics2D.OverlapCircle(wireCheckRight.position, 0.2f, m_WhatIsWire) && rb.velocity.x > 10f && Physics2D.OverlapCircle(inIntersection.position, 0.2f, m_WhatIsWire) && facing == "right")
		{
			rb.velocity = new Vector2(0f, 0f);
		}
		if (!Physics2D.OverlapCircle(wireCheckLeft.position, 0.2f, m_WhatIsWire) && rb.velocity.x < -10f && Physics2D.OverlapCircle(inIntersection.position, 0.2f, m_WhatIsWire) && facing == "left")
		{
			rb.velocity = new Vector2(0f, 0f);
		}
		if (!Physics2D.OverlapCircle(wireCheckUp.position, 0.2f, m_WhatIsWire) && rb.velocity.y > 10f && Physics2D.OverlapCircle(inIntersection.position, 0.2f, m_WhatIsWire) && facing == "up")
		{
			rb.velocity = new Vector2(0f, 0f);
		}
		if (!Physics2D.OverlapCircle(wireCheckDown.position, 0.2f, m_WhatIsWire) && rb.velocity.y < -10f && Physics2D.OverlapCircle(inIntersection.position, 0.2f, m_WhatIsWire) && facing == "down")
		{
			rb.velocity = new Vector2(0f, 0f);
		}
	void Rotate()
	{
		if (facing == "left")
		{
			if (moveRight)
			{
				facing = "right";
				animator.SetInteger("FaceDirection", 1);
			}
			if (moveUp)
			{
				facing = "up";
				animator.SetInteger("FaceDirection", 2);
			}
			if (moveDown)
			{
				facing = "down";
				animator.SetInteger("FaceDirection", 4);
			}
		}
		else if (facing == "right")
		{
			if (moveLeft)
			{
				facing = "left";
				animator.SetInteger("FaceDirection", 3);
			}
			if (moveUp)
			{
				facing = "up";
				animator.SetInteger("FaceDirection", 2);
			}
			if (moveDown)
			{
				facing = "down";
				animator.SetInteger("FaceDirection", 4);
			}
		}
		else if (facing == "up")
		{
			if (moveLeft)
			{
				facing = "left";
				animator.SetInteger("FaceDirection", 3);
			}
			if (moveRight)
			{
				facing = "right";
				animator.SetInteger("FaceDirection", 1);
			}
			if (moveDown)
			{
				facing = "down";
				animator.SetInteger("FaceDirection", 4);
			}
		}
		else if (facing == "down")
		{
			if (moveLeft)
			{
				facing = "left";
				animator.SetInteger("FaceDirection", 3);
			}
			if (moveRight)
			{
				facing = "right";
				animator.SetInteger("FaceDirection", 1);
			}
			if (moveUp)
			{
				facing = "up";
				animator.SetInteger("FaceDirection", 2);
			}
		}
	}
	
	void Move()
	{
		canTurnDown = Physics2D.OverlapCircle(downCheck.position, 0.2f, m_WhatIsWire);
		canTurnUp = Physics2D.OverlapCircle(upCheck.position, 0.2f, m_WhatIsWire);
		canTurnLeft = Physics2D.OverlapCircle(leftCheck.position, 0.2f, m_WhatIsWire);
		canTurnRight = Physics2D.OverlapCircle(rightCheck.position, 0.2f, m_WhatIsWire);
		if (canTurnLeft || canTurnRight || canTurnUp || canTurnDown)
		{
		
			if (facing == "left" && moveRight && canTurnRight)
			{
				Rotate();
				rb.velocity = new Vector2(setSpeed, 0f);
			}
			else if (facing == "left" && moveUp && canTurnUp)
			{
				Rotate();
				rb.velocity = new Vector2(0f, setSpeed);
			}
			else if (facing == "left" && moveDown && canTurnDown)
			{
				Rotate();
				rb.velocity = new Vector2(0f, -setSpeed);
			}
			else if (facing == "right" && moveLeft && canTurnLeft)
			{
				Rotate();
				rb.velocity = new Vector2(-setSpeed, 0f);
			}
			else if (facing == "right" && moveUp && canTurnUp)
			{
				Rotate();
				rb.velocity = new Vector2(0f, setSpeed);
			}
			else if (facing == "right" && moveDown && canTurnDown)
			{
				Rotate();
				rb.velocity = new Vector2(0f, -setSpeed);
			}
			else if (facing == "down" && moveUp && canTurnUp)
			{
				Rotate();
				rb.velocity = new Vector2(0f, setSpeed);
			}
			else if (facing == "down" && moveLeft && canTurnLeft)
			{
				Rotate();
				rb.velocity = new Vector2(-setSpeed, 0f);
			}
			else if (facing == "down" && moveRight && canTurnRight)
			{
				Rotate();
				rb.velocity = new Vector2(setSpeed, 0f);
			}
			else if (facing == "up" && moveDown && canTurnDown)
			{
				Rotate();
				rb.velocity = new Vector2(0f, -setSpeed);
			}
			else if (facing == "up" && moveLeft && canTurnLeft)
			{
				Rotate();
				rb.velocity = new Vector2(-setSpeed, 0f);
			}
			else if (facing == "up" && moveRight && canTurnRight)
			{
				Rotate();
				rb.velocity = new Vector2(setSpeed, 0f);
			}
			
		}
	}
}
}
