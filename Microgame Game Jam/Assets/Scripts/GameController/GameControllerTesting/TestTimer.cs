using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestTimer : MonoBehaviour
{
    public bool willLose = false;

    public float timeToBurn = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    IEnumerator timeThenSwitch()
    {
        yield return new WaitForSeconds(timeToBurn);

        if(willLose)
        {
            GameController.Instance.LoseGame();
        }
        else
        {
            GameController.Instance.WinGame();
        }
    }
}
