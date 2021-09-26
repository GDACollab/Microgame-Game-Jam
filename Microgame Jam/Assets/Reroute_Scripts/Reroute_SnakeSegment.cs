using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reroute_SnakeSegment : MonoBehaviour
{
    public Vector2 lastFramePos;

    private void LateUpdate()
    {
        lastFramePos = transform.position;
    }
}
