using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DDC_RobotController : MonoBehaviour
{
    public Rigidbody2D rb;

    public float speed;
    public float rotationSpeed;

    float horizontal;
    float vertical;

    public float timer = 3f;
    public float cooldown;
    public bool isDashing;
    public bool canDash;
    public SpriteRenderer sr;
    public Sprite[] sprite;
    

    

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");
        if (!isDashing && timer >= 3)
        {
            sr.sprite = sprite[0];
        }
        

        if (Input.GetKeyDown("space"))
        {
            if (!isDashing && timer >= 3)
            {
                StartCoroutine(Dash());
            }
        }

        if (!isDashing && timer < 3)
        {
            timer = timer + Time.deltaTime;
            sr.sprite = sprite[1];
        }


    }

    private void FixedUpdate()
    {
        rb.velocity = transform.right * speed;
        float rotation = -horizontal * rotationSpeed;
        transform.Rotate(Vector3.forward * rotation);
    }

    IEnumerator Dash()
    {
        isDashing = true;
        speed = speed + 10;
        yield return new WaitForSeconds(0.2f);
        isDashing = false;
        speed = speed - 10;
        timer = 0;
    }
}
