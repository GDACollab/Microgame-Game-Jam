using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Yukidamage_Title : MonoBehaviour
{

    public Yukidamage_Manager manager;
    private Rigidbody2D rb;
    [SerializeField] float speed;
    [SerializeField] float lifetime;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    
    void Update()
    {
        rb.velocity = new Vector2(-manager.speed, 0);
        if(GameController.Instance.gameTime >= lifetime){
            Destroy(gameObject);
        }
    }
}
