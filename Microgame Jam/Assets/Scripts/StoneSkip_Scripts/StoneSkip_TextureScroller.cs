using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneSkip_TextureScroller : MonoBehaviour
{
    public bool active = false;
    public float speed;
    private float offsetV = 0;
    public Vector2 direction;

    private SpriteRenderer spRenderer;


    void Start()
    {
        //direction = direction.normalized;
        spRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        
        if (active) {
            offsetV += Time.deltaTime * speed;
            spRenderer.material.mainTextureOffset = direction * offsetV;
        }
    }
}
