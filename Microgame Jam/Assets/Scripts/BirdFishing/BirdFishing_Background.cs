using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdFishing_Background : MonoBehaviour
{
    public float speed = .02f;
    // Update is called once per frame
    void Update()
    {
        float scale = transform.localScale[0];
        scale += speed * Time.deltaTime;
        transform.localScale = new Vector3( scale, scale, scale );
    }
}
