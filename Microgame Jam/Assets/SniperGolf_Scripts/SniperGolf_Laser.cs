using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SniperGolf_Laser : MonoBehaviour
{
    [SerializeField] private Transform crosshair;

    // Start is called before the first frame update
    void Start()
    {
        crosshair = GameObject.Find("Crosshair").transform;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 rotateDirection = crosshair.position - this.transform.position;
        float angle = Mathf.Atan2(rotateDirection.x, rotateDirection.y) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, -angle));
    }
}
