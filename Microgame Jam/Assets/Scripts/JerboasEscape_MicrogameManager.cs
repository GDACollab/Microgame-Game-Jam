using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JerboasEscape_MicrogameManager : MonoBehaviour
{
    public int collected;

    public AudioClip Portalsound;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (collected >= 1)
        {
            GetComponent<AudioSource>().Play();
            collected = 0;
            AudioSource.PlayClipAtPoint(Portalsound, transform.position);
            GameController.Instance.WinGame();
        }
    }
}
