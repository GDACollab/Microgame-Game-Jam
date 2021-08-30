using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PongJuggle_PaddleRotate : MonoBehaviour
{
    public float speed;
    private float rotateSpeed;
    // Start is called before the first frame update
    void Start()
    {
        rotateSpeed = speed * (Random.Range(0,2)*2-1);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.Rotate(new Vector3(0f, 0f, rotateSpeed), Space.Self);
    }
}
