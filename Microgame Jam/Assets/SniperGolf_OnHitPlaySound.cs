using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SniperGolf_OnHitPlaySound : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D other) {
        GetComponent<AudioSource>().Play();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        GetComponent<AudioSource>().Play();
    }
}
