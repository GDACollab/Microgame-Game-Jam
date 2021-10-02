using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarpSpeed_MicrogameManager : MonoBehaviour
{
    [SerializeField] private GameObject WarpSpeed_Gate;
    [SerializeField] private GameObject WarpSpeed_Lightstreak;
    private float gateTime = 0.0f;
    public float gatePeriod = 0.1f;
    public int collected;
    public int numToSpawn;
    public Vector2 Min;
    public Vector2 Max;
    public bool gameOver;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        float x = UnityEngine.Random.Range(Min.x, Max.x);
        float y = UnityEngine.Random.Range(Min.y, Max.y);
        Vector2 randomPos = new Vector2(x, y);

        gateTime += Time.deltaTime;

        if (gateTime >= gatePeriod)
        {
            gateTime = gateTime - gatePeriod;
            Instantiate(WarpSpeed_Gate, randomPos, Quaternion.identity);
        }
        if (collected >= 3 + GameController.Instance.gameDifficulty)
        {
            collected = 0;
            if (GameController.Instance.timerOn)
            {
                GameController.Instance.WinGame();
            }
        }
    }
}
