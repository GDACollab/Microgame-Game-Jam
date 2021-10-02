using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlippedOff_collectible : MonoBehaviour
{
    public Vector3 growthValue;
    public float sizeRequired;
    public float pointsGiven;
    public bool winningCollectible;
    public FlippedOff_playerMovement player;
    private float jumpAmount;

    // Start is called before the first frame update
    void Start()
    {
        if (winningCollectible)
        {
            sizeRequired = 15 + (GameController.Instance.gameDifficulty * 2);
            Vector3 difficultyScale = new Vector3(.7f, .7f, .7f) * (GameController.Instance.gameDifficulty - 1);
            transform.localScale += difficultyScale;
            transform.position += new Vector3(0, difficultyScale.y, 0);
        }

        if (growthValue == null)
        {
            growthValue = new Vector3(.2f, .2f, .2f);
        }
        player = GameObject.Find("Player").GetComponent<FlippedOff_playerMovement>();
        jumpAmount = 1.5f;
    }

    // Update is called once per frame
    void Update()
    {
            
    }
    public void TriggerJump()
    {
        gameObject.GetComponent<Rigidbody>().velocity = new Vector3(0, jumpAmount, 0);
    }
}
