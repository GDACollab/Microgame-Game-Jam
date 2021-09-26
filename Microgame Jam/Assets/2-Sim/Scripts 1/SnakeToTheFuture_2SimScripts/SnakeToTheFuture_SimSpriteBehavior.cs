using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeToTheFuture_SimSpriteBehavior : MonoBehaviour
{
    public Sprite Deselected;
    public Sprite Selected;
    Sprite CurrentSprite;
    [HideInInspector]
    public SpriteRenderer SpriteRenderer;
    public bool Visible;
    
    void Awake()
    {
        SpriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        SpriteRenderer.enabled = false;
        Visible = false;
        CurrentSprite = Deselected;
    }    
    // Start is called before the first frame update

    // Update is called once per frame
    public void ToggleSelectedSprite()
    {
        if(CurrentSprite == Deselected)
        {
            CurrentSprite = Selected;
        }
        else if(CurrentSprite == Selected)
        {
            CurrentSprite = Deselected;
        }

        SpriteRenderer.sprite = CurrentSprite;
    }

    public void ToggleVisible(bool visibility)
    {
        SpriteRenderer.enabled = visibility;
        Visible = visibility;
    }
}
