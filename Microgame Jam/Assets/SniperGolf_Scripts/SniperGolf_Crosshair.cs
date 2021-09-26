using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SniperGolf_Crosshair : MonoBehaviour
{
    public GameObject player;
    public AudioSource aimSound;
    public AudioSource fireSound;
    public AudioSource loseSound;
    public SniperGolf_Shake cameraShake;
    public float sniperDistance = 0.5f;
    Vector3 startScale;
    bool zoomIn = false;

    float moveSpeed;
    float aimSpeed;

    private Vector3 playerPos;
    private bool isAiming;

    // Start is called before the first frame update
    void Start()
    {
        switch (GameController.Instance.gameDifficulty) {
            default:
                moveSpeed = 1;
                aimSpeed = 1;
                break;
            case 2:
                moveSpeed = 1.25f;
                aimSpeed = 1.5f;
                break;
            case 3:
                moveSpeed = 1.5f;
                aimSpeed = 2f;
                break;
        }
        aimSound.pitch = aimSpeed;
        isAiming = true;
        startScale = this.transform.localScale;
    }

    IEnumerator Aim() {
        zoomIn = true;
        aimSound.Play();
        while (aimSound.isPlaying)
        {
            yield return null;
        }
        zoomIn = false;
        Shoot();
    }

    // Update is called once per frame
    void Update()
    {
        if (isAiming)
        {
            playerPos = new Vector3(player.transform.position.x, player.transform.position.y, this.transform.position.z);
            this.transform.position = Vector3.MoveTowards(this.transform.position, playerPos, moveSpeed * Time.deltaTime);
            if (Vector2.Distance(player.transform.position, this.transform.position) < sniperDistance && !zoomIn) {
                StartCoroutine(Aim());
            }
        }
        if (zoomIn)
        {
            this.transform.localScale -= Vector3.one * Time.deltaTime * 0.1f;
        }
        else {
            if (this.transform.localScale.magnitude < startScale.magnitude)
            {
                this.transform.localScale += Vector3.one * Time.deltaTime;
            }
        }
    }

    void Shoot()
    {
        fireSound.Play();
        cameraShake.ShakeCamera(0.05f, 0.001f);
        if (Vector2.Distance(player.transform.position, this.transform.position) < sniperDistance)
        {
            loseSound.Play();
            player.GetComponent<SniperGolf_PlayerController>().inputEnabled = false;
            cameraShake.ShakeCamera(5, 5);
            isAiming = false;
            player.GetComponent<SniperGolf_PlayerController>().EndGame();
        }
        else {
            isAiming = true;
        }
    }
}
