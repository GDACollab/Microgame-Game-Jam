using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveIfWebGL : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
#if UNITY_WEBGL
        gameObject.SetActive(false);
#endif
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
