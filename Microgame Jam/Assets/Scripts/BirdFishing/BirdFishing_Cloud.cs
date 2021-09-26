using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BirdFishing_Cloud : MonoBehaviour
{
    public Image image;

    private float heightThreshold = 0;

    private void Awake()
    {
        image.color = new Color( 1, 1, 1, 0 );
    }

    private void Update()
    {
        float height = transform.position.y;
        if( height < heightThreshold )
        {
            float alpha = BirdFishing_Math.LinearMap( height, -60, heightThreshold - 2, .1f, 1 );
            image.color = new Color( 1,1,1, alpha );
        }
    }
}
