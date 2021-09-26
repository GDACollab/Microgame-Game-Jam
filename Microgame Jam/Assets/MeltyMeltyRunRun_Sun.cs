using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeltyMeltyRunRun_Sun : MonoBehaviour
{
    public GameObject Player;
    public GameObject LaserFX;
    public GameObject LaserFXTwo;
    
    private bool foundPlayer = false;
    private LineRenderer laser;
    private float timer = 0;
    private float laserX;
    private float laserY;

    //Two States
        //SEARCH FOR PLAYER
            //OSCILLATE BACK AND FORTH
                //CHECK FOR PLAYER IN UPDATE

        //FOUND PLAYER
            //CONTINUE TO FOLLOW THEM
                //TURN MELTING TO TRUE

    // Start is called before the first frame update
    void Start()
    {
        laser = transform.GetChild(0).GetChild(0).GetComponent<LineRenderer>();
        foundPlayer = true;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (Player.GetComponent<MeltyMeltyRunRun_Player>().lost) {
            laser.SetPosition(1, new Vector3(19f, 9f, 0f));
            //make sun smile
        }

        if (foundPlayer && !Player.GetComponent<MeltyMeltyRunRun_Player>().lost) {
            //raycast to player, if doesn't hit anything before getting there then
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Player.transform.position - transform.position, 60);
            laser.SetPosition(1, (Vector2) hit.transform.position);
            LaserFX.transform.position = hit.transform.position;
            LaserFXTwo.transform.position = hit.transform.position;

            if (hit.collider.transform == Player.transform) {
                Player.GetComponent<MeltyMeltyRunRun_Player>().setMelt(true);
            } else {
                foundPlayer = false;
                Player.GetComponent<MeltyMeltyRunRun_Player>().setMelt(false);
            }
        } else { //haven't found player
            //laser.SetPosition(1, new Vector3(19f, 9f, 0f));
            //oscillate between -4.5 x & 0 x
            //oscillate between -2 y & -4.5 y

            laserX = Mathf.PingPong(timer, 4.5f);
            laserY = Mathf.PingPong(timer, 2.5f);

            float temp2 = laserY + 2;

            Vector3 temp = new Vector3(-1 * laserX, -1 * temp2, 150);

            RaycastHit2D hit = Physics2D.Raycast(transform.position, temp, 60);
            if (hit.collider.transform == Player.transform)
                foundPlayer = true;
            laser.SetPosition(1, (Vector2) hit.point);
            LaserFX.transform.position = hit.point;
            LaserFXTwo.transform.position = hit.point;
        }
    }
}
