using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlippedOff_collisionManager : MonoBehaviour
{
    [SerializeField] private float currentSize = 0; 
    [SerializeField] private AudioSource collectSFX;
    [SerializeField] private AudioSource boingSFX;
    public BoxCollider playerCol;
    public Transform pivotLeft;
    public Transform pivotRight;
    public Transform pivotForward;
    public Transform pivotBackward;
    public FlippedOff_cameraShake shakeController;
    public Animator animator;
    public Animator animator1;
    public Animator animator2;
    public Animator animator3;
    public Animator animator4;
    public Animator animator5;
    private Vector3 growthValue;

    //private gameObject face;

    // Start is called before the first frame update
    void Start()
    {
        playerCol = gameObject.GetComponent<BoxCollider>();

        //face = Transform.Find("Plane")
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Collectible")
        {
            if (currentSize >= col.gameObject.GetComponent<FlippedOff_collectible>().sizeRequired)
            {
                collectSFX.Play();
                
                if (col.gameObject.GetComponent<FlippedOff_collectible>().winningCollectible)
                    GameController.Instance.WinGame();
                //Box grows larger
                growthValue = col.gameObject.GetComponent<FlippedOff_collectible>().growthValue;
                transform.localScale += growthValue;
                shakeController.shakeMagnitude += 0.05f;

                Destroy(col.gameObject);
                currentSize += col.gameObject.GetComponent<FlippedOff_collectible>().pointsGiven;
                animator.SetBool("isIdle", false);
                animator.SetBool("isEating", true);

                animator1.SetBool("isIdle", false);
                animator1.SetBool("isEating", true);

                animator2.SetBool("isIdle", false);
                animator2.SetBool("isEating", true);

                animator3.SetBool("isIdle", false);
                animator3.SetBool("isEating", true);

                animator4.SetBool("isIdle", false);
                animator4.SetBool("isEating", true);

                animator5.SetBool("isIdle", false);
                animator5.SetBool("isEating", true);
                pivotReposition();
            }
            else
            {
                //boingSFX.Play();
                reverseDirection();

                Debug.Log("Can't be eaten!");
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Boundary")
            reverseDirection();
    }

    void reverseDirection()
    {
        Debug.Log("I am bouncing");
        animator.SetBool("isIdle", false);
        animator.SetBool("isBouncing", true);

        animator1.SetBool("isIdle", false);
        animator1.SetBool("isBouncing", true);

        animator2.SetBool("isIdle", false);
        animator2.SetBool("isBouncing", true);

        animator3.SetBool("isIdle", false);
        animator3.SetBool("isBouncing", true);

        animator4.SetBool("isIdle", false);
        animator4.SetBool("isBouncing", true);

        animator5.SetBool("isIdle", false);
        animator5.SetBool("isBouncing", true);
        print("reversing direction");
        FlippedOff_playerMovement player = playerCol.gameObject.GetComponent<FlippedOff_playerMovement>();

        //reverse direction of rotation
        if (player.rotateAxis == Vector3.right || player.rotateAxis == Vector3.left)
            player.direction.x = -player.direction.x;
        else
            player.direction.z = -player.direction.z;

        //reverse axis of rotation
        player.rotateAxis = -player.rotateAxis;

        //set rotation amount to the remained of the original rotation
        player.totalRotation = 90f - player.totalRotation;
    }

    void pivotReposition()
    {
        pivotLeft.localPosition = new Vector3(-playerCol.transform.localScale.x / 2, -playerCol.transform.localScale.x / 2, 0);

        pivotBackward.localPosition = new Vector3(0, -playerCol.transform.localScale.x / 2, -playerCol.transform.localScale.x / 2);

        pivotForward.localPosition = new Vector3(0, -playerCol.transform.localScale.x / 2, playerCol.transform.localScale.x / 2);

        pivotRight.localPosition = new Vector3(playerCol.transform.localScale.x / 2, -playerCol.transform.localScale.x / 2, 0);
    }
}
