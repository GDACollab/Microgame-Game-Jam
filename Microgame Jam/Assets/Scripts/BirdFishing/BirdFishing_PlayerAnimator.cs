using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdFishing_PlayerAnimator : MonoBehaviour
{
    private Animator animator;
    private new Rigidbody rigidbody;

    // Start is called before the first frame update
    void Awake()
    {
        animator = GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetFloat( "X", rigidbody.velocity[0] );
        animator.SetFloat( "Y", rigidbody.velocity[2] );
    }
}
