using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopstarPoser_PoseConfig : MonoBehaviour
{
    [SerializeField] private GameObject [] limbs;
    private float [][] poses;

    public float [] pose1;
    public float [] pose2;
    public float [] pose3;
    public float [] pose4;
    public float [] pose5;
    public float [] pose6;
    public float [] pose7;
    public float [] pose8;
    public float [] pose9;
    public float [] pose10;

    public float[] selectedPose;

    // Start is called before the first frame update
    void Start()
    {
        poses = new float[][] {pose1, pose2, pose3, pose4, pose5, pose6, pose7, pose8, pose9, pose10};

        //Choose a pose to pick based on the difficulty
        selectedPose = pose1;

        if (GameController.Instance.gameDifficulty == 1)
        {
            selectedPose = poses[UnityEngine.Random.Range(0, 3)];
        }
        else if (GameController.Instance.gameDifficulty == 2)
        {
            selectedPose = poses[UnityEngine.Random.Range(3, 6)];
        }
        else
        {
            selectedPose = poses[UnityEngine.Random.Range(6, 10)];
        }


        int i = 0;
        foreach(GameObject limb in limbs)
        {
            //If a secondary limb, add an offset based on the attached limb
            if (i % 2 != 0)
            {
                limb.transform.rotation = Quaternion.Euler(0, 0, selectedPose[i] + selectedPose[i - 1]);
            }
            //Rotate a limb into a preconfigured rotation
            else 
            {
                limb.transform.rotation = Quaternion.Euler(0, 0, selectedPose[i]);
            }
            i++;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
