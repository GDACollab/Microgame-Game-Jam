using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GimmeFive_Fill : MonoBehaviour
{
    public float progress;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        transform.GetChild(0).localScale = new Vector3(progress + 0.05f, progress + 0.05f, progress + 0.05f);   
    }

    public void SetProgressColor(Color color) {
        transform.GetChild(0).GetComponent<SpriteRenderer>().color = color;
    }

    public void HideSelf() {
        this.gameObject.SetActive(false);
    }
}
