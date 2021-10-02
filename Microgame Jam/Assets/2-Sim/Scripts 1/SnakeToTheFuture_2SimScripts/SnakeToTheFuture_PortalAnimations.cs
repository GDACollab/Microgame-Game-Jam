using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(SpriteRenderer))]

public class SnakeToTheFuture_PortalAnimations : MonoBehaviour
{
    // Start is called before the first frame update

    SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        PortalVisible(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PortalVisible (bool visible)
    {
        spriteRenderer.enabled = visible;
    }
}
