using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SniperGolf_PlayerController : MonoBehaviour
{
    public float speed = 10.0f;
    public bool inputEnabled = true;
    public GameObject eyes;
    Rigidbody2D playerRigidbody;
    Animator currAnimator;
    // Start is called before the first frame update
    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody2D>();
        currAnimator = GetComponent<Animator>();
    }


    void FixedUpdate()
    {
        if (inputEnabled)
        {
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                playerRigidbody.AddForce(Vector2.left * speed);
            }
            if (Input.GetKey(KeyCode.RightArrow))
            {
                playerRigidbody.AddForce(-Vector2.left * speed);
            }
            if (Input.GetKey(KeyCode.UpArrow))
            {
                playerRigidbody.AddForce(Vector2.up * speed);
            }
            if (Input.GetKey(KeyCode.DownArrow))
            {
                playerRigidbody.AddForce(-Vector2.up * speed);
            }

            // Start idling if we're barely moving:
            currAnimator.SetBool("IsMoving", playerRigidbody.velocity.magnitude > 0.05f);

            var normalVelocity = playerRigidbody.velocity.normalized;
            // Get a percentage of the velocity.
            var outsideRest = playerRigidbody.velocity / new Vector2(5, 5);
            // If the magnitude of that percentage is greater than 100%, we should reset it to 100%.
            if (outsideRest.magnitude > 1)
            {
                outsideRest = new Vector2(0.9f, 0.9f);
            }
            // Now move the eyes in the direction of the velocity, up to +/- 0.5 units away, letting outsideRest tween us from resting.
            eyes.transform.localPosition = new Vector3(normalVelocity.x * 0.5f * Mathf.Abs(outsideRest.x), normalVelocity.y * 0.5f * Mathf.Abs(outsideRest.y), 0);
        }
    }

    public void EndGame() {
        GetComponent<ParticleSystem>().Play();
        StartCoroutine(WaitForEnd());
    }

    IEnumerator WaitForEnd() {
        yield return new WaitForSeconds(1);
        if (GameController.Instance.timerOn)
        {
            GameController.Instance.LoseGame();
        }
    }
}
