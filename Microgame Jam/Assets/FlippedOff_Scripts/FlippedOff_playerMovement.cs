using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlippedOff_playerMovement : MonoBehaviour
{
    [SerializeField] private GameObject pivotPoints;
    [SerializeField] private AudioSource slamSFX;
    private bool moving = false;
    public GameObject[] collectibles;
    public FlippedOff_cameraShake shakeController;
    [HideInInspector] public Vector3 direction;
    [HideInInspector] public float totalRotation = 0f;
    [HideInInspector] public Vector3 rotateAxis;
    public Animator animator;
    public Animator animator1;
    public Animator animator2;
    public Animator animator3;
    public Animator animator4;
    public Animator animator5;

    public int rotateSpeed = 180;
    // Start is called before the first frame update
    void Start()
    {
        collectibles = GameObject.FindGameObjectsWithTag("Collectible");
    }

    // Update is called once per frame
    void Update()
    {
        GetMoveDirection();
        if (moving) 
        {
            totalRotation += rotateSpeed * Time.deltaTime;
            if(totalRotation > 90f)
            {
                transform.RotateAround(direction, rotateAxis, rotateSpeed * Time.deltaTime - (totalRotation - 90f));
            }    
            else
            {
                transform.RotateAround(direction, rotateAxis, rotateSpeed * Time.deltaTime);
            } 
        }
        if(totalRotation >= 90f)
        {
            moving = false;
            totalRotation = 0f;
            transform.position = new Vector3(transform.position.x, transform.lossyScale.y / 2, transform.position.z);
            shakeController.TriggerShake();
            animator.SetBool("isEating", false);
            animator.SetBool("isBouncing", false);
            animator.SetBool("isIdle", true);

            animator1.SetBool("isEating", false);
            animator1.SetBool("isBouncing", false);
            animator1.SetBool("isIdle", true);

            animator2.SetBool("isEating", false);
            animator2.SetBool("isBouncing", false);
            animator2.SetBool("isIdle", true);

            animator3.SetBool("isEating", false);
            animator3.SetBool("isBouncing", false);
            animator3.SetBool("isIdle", true);

            animator4.SetBool("isEating", false);
            animator4.SetBool("isBouncing", false);
            animator4.SetBool("isIdle", true);

            animator5.SetBool("isEating", false);
            animator5.SetBool("isBouncing", false);
            animator5.SetBool("isIdle", true);

            foreach(GameObject obj in collectibles)
            {
                if (obj != null)
                    obj.GetComponent<FlippedOff_collectible>().TriggerJump();
            }
            slamSFX.Play();
        }
    }

    private void GetMoveDirection()
    {
        if (!moving)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                moving = true;
                direction = pivotPoints.transform.GetChild(0).position;
                rotateAxis = Vector3.right;
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                moving = true;
                direction = pivotPoints.transform.GetChild(1).position;
                rotateAxis = Vector3.forward;
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                moving = true;
                direction = pivotPoints.transform.GetChild(2).position;
                rotateAxis = Vector3.back;
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                moving = true;
                direction = pivotPoints.transform.GetChild(3).position;
                rotateAxis = Vector3.left;
            }

        }
        
    }
}
