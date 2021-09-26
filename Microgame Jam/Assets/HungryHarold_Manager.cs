using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HungryHarold_Manager : MonoBehaviour
{
    [SerializeField] private GameObject fish;
    [SerializeField] private GameObject enemy;
    [SerializeField] private AudioSource managerAudio;

    public int collected;
    public int fattest = 3;
    public Vector2 fishMin;
    public Vector2 fishMax;
    public Vector2 enemyMin;
    public Vector2 enemyMax;
    public float Timebetweenspawn;
    private float Timebetweenspawncount;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < fattest; i++)
        {
            float x = UnityEngine.Random.Range(fishMin.x, fishMax.x);
            float y = UnityEngine.Random.Range(fishMin.y, fishMax.y);
            Vector2 randomfishPos = new Vector2(x, y);
            Instantiate(fish, randomfishPos, Quaternion.identity);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (collected == fattest)
        {
            collected = 0;
            GameController.Instance.WinGame();
            managerAudio.Play();
        }
        Timebetweenspawncount -= Time.deltaTime;
        if (Timebetweenspawncount <= 0)
        {
            Timebetweenspawncount = Timebetweenspawn;
            for (int i = 0; i < 9; i++)
            {
                float x = UnityEngine.Random.Range(enemyMin.x, enemyMax.x);
                float y = UnityEngine.Random.Range(enemyMin.y, enemyMax.y);
                Vector2 randomenemyPos = new Vector2(x, y);
                Instantiate(enemy, randomenemyPos, Quaternion.identity);
            }
        }

    }
}
