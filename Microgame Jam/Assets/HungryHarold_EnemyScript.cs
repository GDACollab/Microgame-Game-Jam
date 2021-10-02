using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HungryHarold_EnemyScript: MonoBehaviour
{
    public float enemyspeed = 1;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * Time.deltaTime * enemyspeed);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Ground")
        {
            Destroy(gameObject);
        }

    }
}
