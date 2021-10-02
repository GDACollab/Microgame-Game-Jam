using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDropThatDungDung_TitleFade : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<Renderer>().enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.anyKey)
        {
            gameObject.GetComponent<Renderer>().enabled = false;
        }
    }

}
