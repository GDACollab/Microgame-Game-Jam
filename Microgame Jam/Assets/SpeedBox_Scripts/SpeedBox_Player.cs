using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedBox_Player : MonoBehaviour
{
    SpeedBox_Game game;

    public bool disableControls = true;

    // Start is called before the first frame update
    void Start()
    {
        game = GameObject.FindGameObjectWithTag("Grid").GetComponent<SpeedBox_Game>();
        Invoke("EnableControls", 0.35f);
    }

    // Update is called once per frame
    void Update()
    {
        if (!disableControls)
        {
            PlayerInput();
        }
    }

    void EnableControls()
    {
        disableControls = false;
    }

    void PlayerInput()
    {
        Vector2Int direction = Vector2Int.zero;
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
        {
            direction = Vector2Int.up;
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
        {
            direction = Vector2Int.down;
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            direction = Vector2Int.left;
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            direction = Vector2Int.right;
        }

        if (direction != Vector2Int.zero)
        {
            Move(direction);
        }
    }

    private void Move(Vector2Int direction)
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction);
        Vector2 oldPosition = transform.position;
        if (hit.collider.gameObject.CompareTag("Finish"))
        {
            transform.position = hit.point + (Vector2)direction * 0.5f;
            disableControls = true;
            game.Finish(direction);
        }
        else
        {
            game.effects.HitParticle(hit.point, direction, hit.distance);
            transform.position = hit.point - (Vector2)direction * 0.5f;
            game.Shake(hit.distance, direction, 0.1f);
            if ((transform.position - game.box.transform.GetChild(1).position).magnitude < 0.6f)
            {
                transform.position = hit.point + (Vector2)direction * 0.5f;
                disableControls = true;
                game.Finish(direction);
            } 
        }

        game.effects.AfterImage(oldPosition, transform.position);
        if (hit.distance > 1) game.sounds.Play(SpeedBox_Sounds.SFX.Laser, 1f);
        game.sounds.Play(SpeedBox_Sounds.SFX.Crash, hit.distance * 0.07f + 0.5f);
    }
}
