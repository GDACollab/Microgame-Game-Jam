using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdFishing_AudioManager : MonoBehaviour
{
    [Header("Game Data")]
    public BirdFishing_GameData gameData;

    [Header("Wind Layers")]
    public List<AudioSource> windLayers = new List<AudioSource>();
    private int layersActive = 0;

    [Header("Bird SFX")]
    public AudioSource birdScream;
    public List<AudioClip> birdScreamClips = new List<AudioClip>();
    public AudioSource birdExplosion;

    //For ending the game
    private bool beginEnding = false;

    private void Start()
    {
        gameData.CaughtBird += AddWindLayer;
        gameData.CaughtBird += BirdScream;

        gameData.EndSound += EndSound;
    }

    private void Update()
    {
        if (GameController.Instance.gameTime >= gameData.maxTime - 2f && !beginEnding)
        {
            EndSound();
        }
    }

    //-----------------------
    // Wind Layer Functions
    //-----------------------
    public void AddWindLayer()
    {
        //If a layer is active, fade it in
        layersActive = Mathf.Clamp(layersActive++, 0, windLayers.Count);
        if (GameController.Instance.gameTime < gameData.maxTime - 2f)
        {
            StartCoroutine(FadeInLayer(layersActive));
        }
    }

    public IEnumerator FadeInLayer(int layer)
    {
        while (windLayers[layer].volume != 0.75f)
        {
            windLayers[layer].volume += 0.0375f;
            yield return new WaitForSeconds(0.1f);
        }
    }

    public IEnumerator FadeOutLayer(int layer)
    {
        while (windLayers[layer].volume > 0)
        {
            windLayers[layer].volume -= 0.07875f;
            yield return new WaitForSeconds(0.1f);
        }
    }

    //------------------------
    // Bird Scream Functions
    //------------------------
    public void BirdScream()
    {
        AudioClip toScream = birdScreamClips[Random.Range(0, birdScreamClips.Count)];
        
        birdScream.pitch = Mathf.Clamp(0.65f + 0.35f * layersActive, 1f, 1.5f);
        birdScream.PlayOneShot(toScream);
        birdExplosion.Play();
    }

    //----------------------
    // End Sound Functions
    //----------------------
    public void EndSound()
    {
        //Fade out wind layers near the end
        StopAllCoroutines();
        for (int i = 0; i <= layersActive; i++)
        {
            StartCoroutine(FadeOutLayer(i));
        }

        beginEnding = true;
    }
}
