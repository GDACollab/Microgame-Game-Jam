using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeeSaw : MonoBehaviour
{
    public bool nikoBallOnSeeSaw = false; //whether the niko ball is on the seesaw
    public GameObject nikoBall;
    public float velocity;
    public float Velocity
    {
        get { return velocity; }
        set
        {
            if (value <= -5)
                velocity = -5;
            else if (value >= 5)
                velocity = 5;
            else
                velocity = value;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.localEulerAngles.z > 45 && transform.localEulerAngles.z < 90)
        {
            velocity = 0;
            transform.localEulerAngles = new Vector3(0, 0, 45);
        }
        else if (transform.localEulerAngles.z < 315 && transform.localEulerAngles.z > 270)
        {
            velocity = 0;
           transform.localEulerAngles = new Vector3(0, 0, 315);
        }
    }

    private void FixedUpdate()
    {
        Velocity += Input.GetAxisRaw("Horizontal") * Time.deltaTime * 2f;
        if(Input.GetAxisRaw("Horizontal") == 0)
        {
            if (Velocity <= 0.1 && Velocity >= -0.1)
                Velocity = 0;
            else if (Velocity < 0)
                Velocity += 0.03f;
            else if (Velocity > 0)
                Velocity -= 0.03f;
        }

        if(nikoBallOnSeeSaw)
        {
            transform.localEulerAngles = new Vector3(0, 0, transform.localEulerAngles.z - Velocity
                - nikoBall.transform.position.x / 10);
        }
        else
        {
            transform.localEulerAngles = new Vector3(0, 0, transform.localEulerAngles.z - Velocity);
        }

        

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.name == "Circle")
        {
            nikoBallOnSeeSaw = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if(collision.gameObject.name == "Circle")
        {
            nikoBallOnSeeSaw = false;
        }
    }

}
