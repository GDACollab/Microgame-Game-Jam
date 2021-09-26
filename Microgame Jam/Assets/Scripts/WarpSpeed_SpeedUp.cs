using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarpSpeed_SpeedUp : MonoBehaviour
{
    private Rigidbody2D rb;
    public float speed;
    public float speedIncrease;
    public WarpSpeed_MicrogameManager manager;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        GameObject managerobject = GameObject.Find("WarpSpeed_MicrogameManager");
        if (managerobject != null)
        {
            manager = managerobject.GetComponent<WarpSpeed_MicrogameManager>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 dir = new Vector2(-2, 0);
        rb.velocity = dir * (speed + (speedIncrease * manager.collected));
    }
}
