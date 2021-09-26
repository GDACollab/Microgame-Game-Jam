using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDropThatDungDung_Generator : MonoBehaviour
{
    public GameObject[] layout_prefabs;


    void Start()
    {
        int layout = Random.Range(0,3);
        Instantiate(layout_prefabs[layout]);
    }

    
}

