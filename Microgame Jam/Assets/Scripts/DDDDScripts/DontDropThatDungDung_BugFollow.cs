using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDropThatDungDung_BugFollow : MonoBehaviour
{
    private Rigidbody2D rb;
    public float speed = 6f;
    public float stopDistance = 1f;
    private Transform target;


    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        rb = this.GetComponent<Rigidbody2D>();

    }

    // Update is called once per frame
    void Update()
    {
        if(Vector2.Distance(transform.position, target.position) > stopDistance) {
            transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
        }
        Vector3 direction = target.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
        rb.rotation = angle;
    }

}
