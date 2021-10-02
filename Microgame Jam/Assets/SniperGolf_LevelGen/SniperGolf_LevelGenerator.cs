using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SniperGolf_LevelGenerator : MonoBehaviour
{
    private SniperGolf_Segment[,] grid;

    private SniperGolf_Segment[] segmentOptions;
    [SerializeField] private SniperGolf_Segment[] hardSegmentOptions;
    [SerializeField] private SniperGolf_Segment[] mediumSegmentOptions;
    [SerializeField] private SniperGolf_Segment[] easySegmentOptions;

    [SerializeField] private int width = 4;
    [SerializeField] private int height = 3;
    [SerializeField] private float spacing = 1;
    [SerializeField] private Transform gridStart;

    private void Start()
    {
        SetDifficulty();
        Generate();
    }

    public void Generate()
    {
        grid = new SniperGolf_Segment[width, height];

        bool holePlaced = false;
        for (int i = 0; i < width; i++)
            for (int j = 0; j < height; j++)
            {
                SniperGolf_Segment newSegment = GetRandomSegment();
                Vector3 position = new Vector3 (i * spacing, j * spacing, 0) + gridStart.position;
                Quaternion rotation = Quaternion.Euler(0, 0, Random.Range(0, 4) * 90f);

                while (newSegment.tag == "Hole" && holePlaced)
                    newSegment = GetRandomSegment();
                
                if (i == width-1 && j == height-1 && !holePlaced)
                    newSegment = GetHoleSegment();
                
                if (newSegment.tag == "Hole")
                {
                    // Hole must be upright
                    rotation = Quaternion.Euler(0, 0, 0);
                    holePlaced = true;
                }

                grid[i, j] = (SniperGolf_Segment) Instantiate(newSegment, position, rotation, GameObject.Find("SniperGolf_Sides").transform);
            }
    }

    private SniperGolf_Segment GetRandomSegment()
    {
        return segmentOptions[Random.Range(0, segmentOptions.Length)];
    }

    private SniperGolf_Segment GetHoleSegment()
    {
        return segmentOptions[0];
    }

    private void SetDifficulty()
    {
        if (GameController.Instance.gameDifficulty == 1)
            segmentOptions = easySegmentOptions;
        else if (GameController.Instance.gameDifficulty == 2)
            segmentOptions = mediumSegmentOptions;
        else
            segmentOptions = hardSegmentOptions;
    }
}