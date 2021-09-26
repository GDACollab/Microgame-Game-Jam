using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdFishing_BirdContainer : MonoBehaviour
{
    public float speed = 5f;

    void Update()
    {
        transform.position = transform.position + new Vector3( 0, speed * Time.deltaTime, 0 );
    }
}
