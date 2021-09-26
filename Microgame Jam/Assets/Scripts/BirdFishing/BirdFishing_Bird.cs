using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BirdFishing_Bird : MonoBehaviour
{
    public Image image;
    public new ParticleSystem particleSystem;

    private new Collider collider;
    private Animator animator;

    private float heightThreshold = 0;
    private bool inDeath = false;

    private void Awake()
    {
        collider = GetComponent<Collider>();
        animator = GetComponent<Animator>();

        image.color = new Color( 1, 1, 1, 0 );
    }

    private void Update()
    {
        if( !inDeath )
        {
            float height = transform.parent.transform.position.y;
            // Before it reaches the player
            if( height < heightThreshold )
            {
                
                float alpha = BirdFishing_Math.LinearMap( height, -60, heightThreshold - 2, .1f, 1 );
                image.color = new Color( 1,1,1, alpha );
            }
            // After it reaches the player
            else
            {
                float alpha = BirdFishing_Math.LinearMap( height, heightThreshold + 2, heightThreshold + 7, 1, 0 );
                image.color = new Color( 1,1,1, alpha );
            }
        }
    }

    public void Caught()
    {
        // So opacity no longer changes
        inDeath = true;

        particleSystem.Play();
        float totalDuration = particleSystem.main.duration + particleSystem.main.startLifetimeMultiplier;

        Destroy( collider );

        animator.SetTrigger("Death");
    }
}
