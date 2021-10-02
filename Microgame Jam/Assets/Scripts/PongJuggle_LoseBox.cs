using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PongJuggle_LoseBox : MonoBehaviour
{
    AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Ball"))
        {
            audioSource.Play();
            GameController.Instance.LoseGame();
        }
    }
}
