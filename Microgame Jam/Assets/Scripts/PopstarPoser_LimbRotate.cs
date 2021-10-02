using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopstarPoser_LimbRotate : MonoBehaviour
{
    public float speed;

    [SerializeField] private GameObject arrows;
    [SerializeField] Color highlightColor;
    private SpriteRenderer activeLimbSprite ;

    [SerializeField] private int turnCount = 8;
    [SerializeField] private AudioSource sfx_select;
    [SerializeField] private AudioSource sfx_crank;
    [SerializeField] private int limbNumber = 0;
    [SerializeField] private GameObject [] limbsToRotate;

    [SerializeField] private Transform[] arrowSpawns;

    [SerializeField] private PopstarPoser_GameManager manager;

    private GameObject activeLimb;

    void Start()
    {
        turnCount = 2 + (2 * GameController.Instance.gameDifficulty);
        activeLimb = limbsToRotate[limbNumber];
        highlightArm(activeLimb);
        arrowFollow(activeLimb);
    }

    void Update()
    {
        if (limbNumber < turnCount && manager.success)
        {
            if (Input.GetButtonDown("Jump"))
            {
                //Reset the set limb to its original color
                activeLimbSprite.color = Color.white;

                //Check to see if the limb was placed correctly
                manager.checkLimb(limbNumber, activeLimb.transform.eulerAngles.z);

                //End loop early if failed early
                if (!manager.success)
                {
                    return;
                }

                sfx_select.Play();

                //Move to the next limb
                limbNumber++;

                //Destroy arrows if done, and end the game loop
                if (limbNumber >= turnCount)
                {
                    Destroy(arrows);
                    manager.showResults();
                    return;
                }

                activeLimb = limbsToRotate[limbNumber];

                highlightArm(activeLimb);

                arrowFollow(activeLimb);
            }

            if (arrows != null)
            {
                arrows.transform.rotation = activeLimb.transform.rotation;

                //Rotate arrow sprite if on a mirrored side
                if (limbNumber % 4 == 2 || limbNumber % 4 == 3)
                {
                    arrows.GetComponent<SpriteRenderer>().flipY = true;
                }
                else
                {
                    arrows.GetComponent<SpriteRenderer>().flipY = false;
                }
            }

            float limbMovement = -Input.GetAxis("Horizontal");

            //Reverse rotation for limbs
            if (limbNumber > 3)
            {
                limbMovement *= -1;
            }

            activeLimb.transform.Rotate(0, 0, limbMovement * speed * Time.deltaTime);

            if (!sfx_crank.isPlaying && Input.GetAxis("Horizontal") != 0)
            {
                sfx_crank.Play();
            }
        }
        else
        {
            activeLimbSprite.color = Color.white;
            Destroy(arrows);
        }
    }

    //Highlights the active limb in red
    void highlightArm(GameObject limb)
    {
        activeLimbSprite = limb.GetComponent<SpriteRenderer>();
        activeLimbSprite.color = highlightColor;
    }

    //Sets the position of the arrow on the active limb
    void arrowFollow(GameObject limb)
    {
        arrows.transform.position = (Vector2)arrowSpawns[limbNumber].position;

        //Hack to get arrows in front of player body
        arrows.transform.position = new Vector3(arrows.transform.position.x, arrows.transform.position.y, -5);

        arrows.transform.SetParent(limb.transform);
    }
}
