using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDropThatDungDung_SignSpectacle : MonoBehaviour
{
    public List<Sprite> PoolOfSprite;
    public SpriteRenderer digitOne;
    public SpriteRenderer digitTwo;
    public int dungCount=0;

    public void DungDungSignIncrease(){
        // Debug.Log(dungCount);
        // Debug.Log(dungCount%10);
        // Debug.Log(dungCount/10);
        dungCount += 1;
        digitOne.sprite = PoolOfSprite[dungCount%10];
        digitTwo.sprite = PoolOfSprite[dungCount/10];
    }
}
