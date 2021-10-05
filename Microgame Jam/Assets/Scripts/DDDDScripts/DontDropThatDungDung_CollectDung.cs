using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DontDropThatDungDung_CollectDung : MonoBehaviour
{
    public DontDropThatDungDung_SignSpectacle bruhHandler; //Sorry my normal variable names are starting to leak out

    private void OnTriggerEnter2D (Collider2D other) 
    {
        if (SceneManager.GetActiveScene().name == "DontDropThatDungDung_GameScene")
        {
            bruhHandler.DungDungSignIncrease();
            DontDropThatDungDung_Score.scoreAmount += 100;
            Destroy(other.gameObject);
        }
    }
}
