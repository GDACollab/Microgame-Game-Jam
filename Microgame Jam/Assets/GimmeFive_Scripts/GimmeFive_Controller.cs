using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GimmeFive_Controller : MonoBehaviour
{
    public GameObject frontHand;
    public GameObject backHand;
    public GameObject pressSpace;

    Rigidbody2D backHandRigidbody;
    Rigidbody2D frontHandRigidbody;
    GimmeFive_Fill progressFill;

    float targetVelocity = 0;

    public float gainProgressSpeed = 0.5f;
    public float loseProgressSpeed = 0.25f;

    public float requiredAngle = 20f;

    bool animateHand = false;

    // Start is called before the first frame update
    void OnEnable()
    {
        float time;
        switch (GameController.Instance.gameDifficulty) {
            case 1:
            default:
                time = 20.0f;
            break;
            case 2:
                time = 15.0f;
            break;
            case 3:
                time = 10.0f;
            break;
        }
        GameController.Instance.SetMaxTimer(time);

        var color = new Color(Random.Range(0.2f, 1), Random.Range(0.2f, 1), Random.Range(0.2f, 1));
        frontHand.transform.GetChild(0).GetComponent<SpriteRenderer>().color = color;
        backHand.transform.GetChild(0).GetComponent<SpriteRenderer>().color = color;
        frontHand.transform.Rotate(new Vector3(0, 0, Random.Range(90, 270)));
        backHandRigidbody = backHand.GetComponent<Rigidbody2D>();
        frontHandRigidbody = frontHand.GetComponent<Rigidbody2D>();
        backHandRigidbody.angularDrag = 1 / GameController.Instance.gameDifficulty;

        progressFill = backHand.transform.GetChild(1).GetComponent<GimmeFive_Fill>();
        progressFill.SetProgressColor(color);
        pressSpace.SetActive(false);
    }

    private void FixedUpdate()
    {
        if (Input.GetKey("up") || Input.GetKey("left"))
        {
            backHandRigidbody.angularVelocity += 10f;
        }
        if (Input.GetKey("down") || Input.GetKey("right"))
        {
            backHandRigidbody.angularVelocity -= 10f;
        }
        frontHandRigidbody.angularVelocity = Mathf.Lerp(frontHandRigidbody.angularVelocity, targetVelocity, Time.deltaTime);
        if (Mathf.Abs(targetVelocity - frontHandRigidbody.angularVelocity) <= 1)
        {
            targetVelocity = Random.Range(-1, 1) * Random.Range(50, 100) * GameController.Instance.gameDifficulty;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector2.Angle(backHand.transform.up, frontHand.transform.up) <= requiredAngle)
        {
            if (progressFill.progress < 1)
            {
                progressFill.progress += gainProgressSpeed * Time.deltaTime;
            }
            else if (progressFill.progress >= 1) {
                pressSpace.SetActive(true);
            }
        }
        else {
            if (progressFill.progress > 0)
            {
                progressFill.progress -= loseProgressSpeed * Time.deltaTime;
            }
        }
        if (progressFill.progress < 1 && pressSpace.activeInHierarchy)
        {
            pressSpace.SetActive(false);
        }
        else if (progressFill.progress >= 1 && pressSpace.activeInHierarchy && Input.GetKeyDown("space") && animateHand == false) {
            pressSpace.SetActive(false);
            GameController.Instance.WinGame();
            animateHand = true;
            backHand.transform.position = frontHand.transform.position + new Vector3(0.1f, 0.1f, -0.1f);
            GetComponent<AudioSource>().Play();
            GameObject.Find("DrumRoller").GetComponent<AudioSource>().Stop();
            progressFill.HideSelf();
        }
        if (animateHand) {
            backHand.transform.Rotate(new Vector3(1, 1, 1));
            frontHand.transform.Rotate(new Vector3(-1, -1, -1));
        }
    }
}
