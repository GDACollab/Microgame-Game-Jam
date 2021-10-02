using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EtchASketch_KnobWobbler : MonoBehaviour
{
    [SerializeField] private Transform leftKnob;
    [SerializeField] private Transform rightKnob;
    
    private float timeElapsedLeft;
    private float timeElapsedRight;

    // Update is called once per frame
    void Update() {
        if (GameController.Instance.timerOn) {
            // Player Input to rotate knobs
            if (Input.GetKeyDown(KeyCode.DownArrow)) {
                Animate(leftKnob, leftKnob.rotation, leftKnob.rotation *= Quaternion.Euler(0, 0, 45), timeElapsedLeft);
            }
            if (Input.GetKeyDown(KeyCode.UpArrow)) {
                Animate(leftKnob, leftKnob.rotation, leftKnob.rotation *= Quaternion.Euler(0, 0, -45), timeElapsedLeft);
            }
            if (Input.GetKeyDown(KeyCode.LeftArrow)) {
                Animate(rightKnob, rightKnob.rotation, rightKnob.rotation *= Quaternion.Euler(0, 0, 45), timeElapsedRight);
            }
            if (Input.GetKeyDown(KeyCode.RightArrow)) {
                Animate(rightKnob, rightKnob.rotation, rightKnob.rotation *= Quaternion.Euler(0, 0, -45), timeElapsedRight);
            }
            if (Input.GetKeyDown("space")) {
                //idk do something cool here
            }
        }
        timeElapsedRight += Time.deltaTime;
        timeElapsedLeft += Time.deltaTime;
    }
    void Animate(Transform obj, Quaternion start, Quaternion end, float timer)
    {
        if (timer < 1f)
        {
            float t = timer / 1f;
            t = t * t * (3f - 2f * t);
            obj.rotation = Quaternion.Slerp(start, end, t);
        }
        else
        {
            obj.rotation = end;
        }
    }
}

