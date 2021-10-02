using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChompyDino_GameController : MonoBehaviour
{
	public int lives;
	
	private bool win;
	
	[SerializeField] private AudioSource WinSound;
	[SerializeField] private AudioSource DeathSound;
	[SerializeField] private AudioSource BGM;
	
    // Start is called before the first frame update
    void Start()
    {
		win = false;
        lives = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameController.Instance.gameTime >= 17 && GameController.Instance.timerOn == true)
	    {
			win = true;
		}
		
		if (win == true)
		{
			WinSound.Play();
		    GameController.Instance.WinGame();
			win = false;
		}
		
		if (lives <= 0)
		{
		    DeathSound.Play();
			GameController.Instance.LoseGame();
			lives = 1;
		}
    }
}
