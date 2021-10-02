using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeToTheFuture_CharacterAnimator : MonoBehaviour
{
    public Sprite MainSprite;
    public Sprite AltSprite;
    
    public void ChangeSprite (string state)
    {
        if (state == "main")
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = MainSprite;
        }
        if (state == "alt")
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = AltSprite;
        }
    }
}
