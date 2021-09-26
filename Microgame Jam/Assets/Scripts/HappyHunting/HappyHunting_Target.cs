using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HappyHunting_Target : MonoBehaviour
{
    public GameObject _crosshair;
    public SpriteRenderer _sprite;
    // Start is called before the first frame update
    void Start()
    {
        _crosshair = GameObject.Find("HappyHunting_Crosshair");
        _sprite = GetComponent<SpriteRenderer>();
        
    }

    // Update is called once per frame
    void Update()
    {
        foreach (GameObject citizen in _crosshair.GetComponent<HappyHunting_Crosshair>()._citizens)
        {
            if (citizen.GetComponent<HappyHunting_Citizen>()._target)
            {
                _sprite.sprite = citizen.GetComponent<SpriteRenderer>().sprite;
            }

        }
        
    }
}
