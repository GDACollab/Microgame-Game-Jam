using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopstarPoser_GameManager : MonoBehaviour
{
    [SerializeField] private PopstarPoser_PoseConfig Dummy;
    [SerializeField] private SpriteRenderer playerHeadSprite;
    [SerializeField] private SpriteRenderer dummyHeadSprite;
    [SerializeField] private Sprite spr_player_sad;
    [SerializeField] private Sprite spr_player_happy;
    [SerializeField] private Sprite spr_dummy_sad;
    [SerializeField] private Sprite spr_dummy_happy;
    [SerializeField] private AudioSource sfx_crowd_success;
    [SerializeField] private AudioSource sfx_crowd_fail;
    [SerializeField] private SpriteRenderer spr_success;
    [SerializeField] private SpriteRenderer spr_fail;


    public bool success = true;
    public int leniency;

    // Start is called before the first frame update
    void Start()
    {
        spr_success.enabled = false;
        spr_fail.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameController.Instance.gameTime >= 20f)
        {
            success = false;
        }

    }

    public void checkLimb(int limbNumber, float limbRotation)
    {

        float playerRotation = limbRotation;
        float dummyRotation = Dummy.selectedPose[limbNumber];

        //Convert limb offset back to original rotation
        if (limbNumber % 2 != 0)
        {
            dummyRotation = Dummy.selectedPose[limbNumber] + Dummy.selectedPose[limbNumber - 1];
        }

        while (playerRotation < 0)
        {
            playerRotation += 360;
        }

        while (dummyRotation < 0)
        {
            dummyRotation += 360;
        }

        //Debug.Log("Player: " + playerRotation);
        //Debug.Log("Dummy: " + dummyRotation);
        
        if (playerRotation > dummyRotation - leniency && playerRotation < dummyRotation + leniency)
        {
            return;
        }
        else
        {
            //Catch edge cases (this is without a doubt the worst block of code I've ever written)
            if (playerRotation + leniency > 360)
            {
                playerRotation -= 360;
            }
            else if (playerRotation - leniency < 0)
            {
                playerRotation += 360;
            }

            //Debug.Log("EDGE Player: " + playerRotation);
            //Debug.Log("EDGE Dummy: " + dummyRotation);

            if (playerRotation > dummyRotation - leniency && playerRotation < dummyRotation + leniency)
            {
                return;
            }
            else
            {
                success = false;
                showResults();
            }

        }
    }

    public void showResults()
    {
        if (success)
        {
            //End game early if animation won't have time to play
            if (GameController.Instance.gameTime >= 17.5f)
            {
                GameController.Instance.WinGame();
            }

            StartCoroutine(WinAnimation());
        }
        else
        {
            StartCoroutine(LoseAnimation());
        }
    }

    IEnumerator WinAnimation()
    {
        sfx_crowd_success.Play();
        spr_success.enabled = true;
        playerHeadSprite.sprite = spr_player_happy;
        dummyHeadSprite.sprite = spr_dummy_happy;
        yield return new WaitForSeconds(2);
        
        GameController.Instance.WinGame();
    }

    IEnumerator LoseAnimation()
    {
        sfx_crowd_fail.Play();
        spr_fail.enabled = true;
        playerHeadSprite.sprite = spr_player_sad;
        dummyHeadSprite.sprite = spr_dummy_sad;
        yield return new WaitForSeconds(2);
        GameController.Instance.LoseGame();
    }
}
