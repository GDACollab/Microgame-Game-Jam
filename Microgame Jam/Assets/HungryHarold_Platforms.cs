using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HungryHarold_Platforms : MonoBehaviour
{

    private PlatformEffector2D effector;
    private float waittime = 0f;
    // Start is called before the first frame update
    void Start()
    {
        effector = GetComponent<PlatformEffector2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyUp(KeyCode.DownArrow))
        {
            waittime = 0f;
        }
        if(Input.GetKey(KeyCode.DownArrow))
        {
            if (waittime <= 0)
            {
                effector.rotationalOffset = 180f;
                waittime = 0f;
            }
            else
            {
                waittime -= Time.deltaTime;
            }
        }
        if(Input.GetButtonDown("Jump"))
        {
            effector.rotationalOffset = 0;
        }
    }
}
