using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedBox_Box : MonoBehaviour
{
    [SerializeField] Vector2Int[] startPositions;
    [SerializeField] Vector2Int[] finishPositions;
    [SerializeField] bool canSwap;

    public Vector2Int startPosition;
    public Vector2Int finishPosition;
    public bool canRotate = true;

    Transform player;
    Transform finish;
    bool swap;

    private void OnEnable()
    {
        startPosition = startPositions[Random.Range(0, startPositions.Length)];
        finishPosition = finishPositions[Random.Range(0, finishPositions.Length)];

        player = transform.GetChild(2);
        finish = transform.GetChild(1);
        finish.transform.localScale = Vector2.one * 0.5f;
        finish.transform.rotation = Quaternion.Euler(0, 0, 45);

        if (canSwap) swap = Random.Range(0, 2) == 0;
        player.transform.localPosition = Vector2.one * 0.5f + (!swap ? (Vector2)startPosition : finishPosition);
        finish.transform.localPosition = Vector2.one * 0.5f + (!swap ? (Vector2)finishPosition : startPosition);
    }
}
