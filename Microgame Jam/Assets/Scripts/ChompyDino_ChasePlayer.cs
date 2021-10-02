using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChompyDino_ChasePlayer : MonoBehaviour
{
    public float chargeCooldown;
    public float chargeForce;
    public float enemySpeed;

    private Rigidbody2D rb;
	private SpriteRenderer rend;
	
    public GameObject player;

    private ChompyDino_PlayerMovement playerMovement;

    private float sizeIncrease;
    private float startScale;
    private Vector2 towardPlayer;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
		rend = GetComponent<SpriteRenderer>();
        startScale = gameObject.transform.localScale.x;
        playerMovement = player.GetComponent<ChompyDino_PlayerMovement>();

        StartCoroutine(ChargeTimer());
    }

    // Update is called once per frame
    void Update()
    {
        sizeIncrease = GameController.Instance.gameDifficulty
            + GameController.Instance.gameTime;

        float newScale = startScale + sizeIncrease * 0.08f;
        gameObject.transform.localScale = new Vector3(newScale, newScale, 1);

        // Get position of player
        Transform enemyTransform = GetComponent<Transform>();
        Transform playerTransform = player.GetComponent<Transform>();
        float playerX = playerTransform.position.x + playerMovement.rb.velocity.x/2;
        float playerY = playerTransform.position.y + playerMovement.rb.velocity.y/2;
        float enemyX = enemyTransform.position.x;
        float enemyY = enemyTransform.position.y;
        // Make vector out of the difference between the enemy and player
        Vector2 vectorBetween = new Vector2(playerX - enemyX, playerY - enemyY);
        vectorBetween.Normalize();
        towardPlayer = vectorBetween;
        // Face the enemy towards the player
        //        if (vectorBetween.x < 0) enemyTransform.Rotation.y = 180;
        //        else enemyTransform.Rotation.y = 0;
        // Set enemy velocity to that vector
        rb.velocity += vectorBetween * enemySpeed * Time.deltaTime;
		
		// flip enemy based on player position
		if (playerTransform.position.x <= enemyX)
		{
			rend.flipX = true;
		} else{
			rend.flipX = false;
		}
    }

    private IEnumerator ChargeTimer()
    {
        while(true)
        {
            yield return new WaitForSeconds(chargeCooldown);

            rb.AddForce(towardPlayer * chargeForce);

            yield return new WaitForSeconds(1);
            rb.velocity = Vector2.zero;
        }
    }
}
