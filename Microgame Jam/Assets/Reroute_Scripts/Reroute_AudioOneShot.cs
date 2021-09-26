using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reroute_AudioOneShot : MonoBehaviour
{

    protected AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.Play();
        float audioClipLength = audioSource.clip.length;
        StartCoroutine(DestroyAudio(audioClipLength));
    }

    public IEnumerator DestroyAudio(float audioClipLength)
    {
        yield return new WaitForSecondsRealtime(audioClipLength);
        Destroy(gameObject);
    }
    
}
