using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDropThatDungDung_PoopSpawn : MonoBehaviour
{
    public int quantity = 10;
    public List<GameObject> spawnPool;
    public int limitX, limitY;


    // Start is called before the first frame update
    void Start()
    {
        spawnObjects();
    }

    public void spawnObjects()
    {
        int randomItem = 0;
        GameObject toSpawn;

        float screenX, screenY;
        Vector2 pos;

        for(int i = 0; i < quantity; i++)
        {
            randomItem = Random.Range(0, spawnPool.Count);
            toSpawn = spawnPool[randomItem];

            screenX = Random.Range(-1*limitX, limitX);
            screenY = Random.Range(-1*limitY, limitY);

            pos = new Vector2(screenX, screenY);

            GameObject temp = Instantiate(toSpawn, pos, toSpawn.transform.rotation);
            temp.name = toSpawn.name;
            
        }
    }

}
