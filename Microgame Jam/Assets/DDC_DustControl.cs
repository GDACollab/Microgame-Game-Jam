using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DDC_DustControl : MonoBehaviour
{
    
    public Rigidbody2D rb;
    public Rigidbody2D rb2;
    DDC_RobotController robotController;
    private ParticleSystem ps;

    public float speed;
    public float suck;
    public float distance;
    public AudioSource pop;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb2 = GameObject.Find("CAM").GetComponent<Rigidbody2D>();
        robotController = GameObject.Find("CAM").GetComponent<DDC_RobotController>();
        ps = GetComponent<ParticleSystem>();
        ps.Pause();
        pop = GameObject.Find("DDC_Pop").GetComponent<AudioSource>();
        
    }

    // Update is called once per frame
    void Update()
    {
        distance = Vector3.Distance(transform.position, rb2.position);
        suck = (speed * Time.deltaTime) / distance;
        if (distance < .1)
        {
            pop.Play();
            Destroy(gameObject);
            DDC_GameManager.total--;
            robotController.speed = robotController.speed + 0.5f;
            robotController.rotationSpeed = robotController.rotationSpeed + 0.5f; 

        }
        

    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Vaccum"))
        {
            transform.position = Vector3.MoveTowards(transform.position, rb2.position, suck);
            transform.Rotate(0f, 0f, 8, Space.Self);
            ps.Play();
        }
    }
    void OnTriggerLeave2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Vaccum"))
        {
            ps.Pause();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (SceneManager.GetActiveScene().name == "DDC_Scene")
        {
            if (collision.gameObject.CompareTag("wall"))
            {
                Destroy(gameObject);
                DDC_GameManager.total--;
            }
        }
    }
}
