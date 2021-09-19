using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MaskController : MonoBehaviour
{
    float progress = 0;
    float dir = -1;
    public float speed = 5;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void ToggleMaskDirection() {
        dir *= -1;
    }

    // Update is called once per frame
    void Update()
    {
        progress += dir * Time.unscaledDeltaTime * speed;
        progress = Mathf.Clamp(progress, 0, 1);
        GetComponent<Image>().fillAmount = progress;
    }
}
