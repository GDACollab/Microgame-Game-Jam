using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MeltyMeltyRunRun_WinCondition : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (SceneManager.GetActiveScene().name == "MeltyMeltyRunRun_PlayScene")
        {
            GameController.Instance.WinGame();
        }
    }
}
