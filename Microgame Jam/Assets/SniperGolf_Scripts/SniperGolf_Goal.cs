using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SniperGolf_Goal : MonoBehaviour
{
    [SerializeField] private AudioSource winSound;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent<SniperGolf_GolfHit>(out SniperGolf_GolfHit ball) && GameController.Instance.timerOn){
            winSound.Play();
            GameController.Instance.WinGame();
        }
    }
}
