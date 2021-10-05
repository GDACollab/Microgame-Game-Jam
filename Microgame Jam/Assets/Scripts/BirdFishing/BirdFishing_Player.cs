using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BirdFishing_Player : MonoBehaviour
{
    public BirdFishing_GameData data;

    public float movementSpeed = 1000f;
    public float fallingSpeed = 1;

    private new Rigidbody rigidbody;

    // Start is called before the first frame update
    void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        Vector3 inputDirection = new Vector3(x,0,y);

        inputDirection.Normalize();

        inputDirection *= movementSpeed * Time.deltaTime;

        rigidbody.AddForce(inputDirection);
    }

    private void OnTriggerEnter( Collider other )
    {
        if (SceneManager.GetActiveScene().name == "BirdFishing_Scene")
        {
            data.OnCaughtBird();

            other.GetComponent<BirdFishing_Bird>().Caught();
        }
    }
}
