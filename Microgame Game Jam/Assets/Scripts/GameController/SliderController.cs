using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderController : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        GetComponent<Slider>().value = GameController.Instance.gameTime / GameController.Instance.maxTime;
    }
}
