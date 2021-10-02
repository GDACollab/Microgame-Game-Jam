using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SniperGolf_GolfHit : MonoBehaviour
{
    public GameObject player;
    public SniperGolf_Shake cameraShake;
    public SniperGolf_PowerMinigameController powerMinigame;
    public float ballHitSpeed = 10;
    public bool canHit;
    Rigidbody2D ballRigidbody;
    GameObject dir;
    // Start is called before the first frame update
    void Start()
    {
        dir = transform.GetChild(0).gameObject;
        dir.SetActive(false);
        powerMinigame.hitSpace.AddListener(BallHit);
        ballRigidbody = GetComponent<Rigidbody2D>();
        Physics2D.IgnoreCollision(player.GetComponent<Collider2D>(), GetComponent<Collider2D>());
    }

    // Update is called once per frame
    void Update()
    {
        canHit = Vector3.Distance(player.transform.position, this.transform.position) < 2;
        dir.SetActive(canHit);
        var targetRot = this.transform.position - player.transform.position;
        var angle = Mathf.Atan2(targetRot.y, targetRot.x) * Mathf.Rad2Deg - 90;
        dir.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        if (Input.GetKeyDown("space") && canHit) {
            powerMinigame.gameObject.SetActive(true);
            player.GetComponent<SniperGolf_PlayerController>().inputEnabled = false;
        }
    }

    void BallHit(float strength) {
        cameraShake.ShakeCamera(0.05f, 0.001f);
        ballRigidbody.AddForceAtPosition(strength * ballHitSpeed * (this.transform.position - player.transform.position).normalized, player.transform.position);
        player.GetComponent<SniperGolf_PlayerController>().inputEnabled = true;
    }
}
