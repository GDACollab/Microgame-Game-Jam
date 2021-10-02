using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedBox_Sounds : MonoBehaviour
{
    [SerializeField] float pitchVariation;
    public enum SFX
    {
        Music,
        Laser,
        Crash,
        Change,
        Shatter
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Play(SFX sfx, float volume)
    {
        AudioSource source = transform.GetChild((int)sfx).GetComponent<AudioSource>();
        source.pitch = 1 + Random.Range(-pitchVariation, pitchVariation);
        source.volume = volume;
        source.Play();
    }
}
