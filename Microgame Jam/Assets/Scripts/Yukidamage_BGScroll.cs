using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Yukidamage_BGScroll : MonoBehaviour
{

    public float scroll_speed = 1f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 offset = new Vector2(Time.time * scroll_speed, 0);
        GetComponent<Renderer>().material.mainTextureOffset = offset;
    }
}
