using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeltyMeltyRunRun_WinCondition : MonoBehaviour
{
    private void OnTriggerEnter2D (Collider2D collision) {
        GameController.Instance.WinGame();
    }
}
