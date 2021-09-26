using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDropThatDungDung_Generator : MonoBehaviour
{
    public GameObject[] layout_prefabs;
    public GameObject DDDDobjects;

    void Start()
    {
        int layout = Random.Range(0,3);
        GameObject LayingOut = Instantiate(layout_prefabs[layout]) as GameObject;
        LayingOut.transform.parent = DDDDobjects.transform;
    }

    
}

