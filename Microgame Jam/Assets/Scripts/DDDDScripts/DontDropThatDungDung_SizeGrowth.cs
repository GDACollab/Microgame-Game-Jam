using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDropThatDungDung_SizeGrowth : MonoBehaviour
{
    public CapsuleCollider2D col;
    public SpriteRenderer spriteRenderer;
    public List<Sprite> SpritePool;

    public int dungLvl;
    public int speedDecrease;

    void Start()
    {
        dungLvl = 0;
        spriteRenderer.sprite = SpritePool[dungLvl];
        DontDropThatDungDung_DungControl.moveSpeed = 6f;
    }

    // Update is called once per frame
    void Update()
    {
        // Every 1500 point, the ball gets one size bigger. In addition, the player becomes slower
        int temp = (int)(Mathf.Floor(DontDropThatDungDung_Score.scoreAmount / 500));
        if(temp != dungLvl) 
        {
            dungLvl = temp;
            DontDropThatDungDung_DungControl.moveSpeed -= 0.4f;
            if(dungLvl < 3)
            {
                ChangeSprite();
            }
        }
        // Debug.Log(DontDropThatDungDung_DungControl.moveSpeed + ": " + SpritePool[dungLvl]);

    }

    void ChangeSprite()
    {
        transform.localScale = new Vector3(0.4f * (dungLvl + 2), 0.4f * (dungLvl + 2), 1);
        spriteRenderer.sprite = SpritePool[dungLvl];
    }
}
