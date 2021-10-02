using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SpeedBox_Game : MonoBehaviour
{
    public GameObject box;
    public GameObject player;
    public Camera cam;
    public SpeedBox_Effects effects;
    public SpeedBox_Sounds sounds;
    public GameControllerDevelop main;
    [SerializeField] GameObject[] boxesArray = new GameObject[27];
    [SerializeField] Color[] colorsArray = new Color[9];
    [SerializeField] Color[] finishColors = new Color[3];
    GameObject[,,] boxes = new GameObject[3, 3, 3];
    Color[,] boxColors = new Color[3, 3];

    [SerializeField] float moveTimePerDistance;
    [SerializeField] float resetTimePerDistance;
    [SerializeField] float aplitude;
    [SerializeField] float transitionAplitude;
    [SerializeField] float transitionMoveTime;
    [SerializeField] float transitionResetTime;
    [SerializeField] float trailRotationSpeed;

    int stage = 1;
    Quaternion trailTargetRotation;
    GameObject boxOffset;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        boxOffset = transform.GetChild(0).gameObject;
        //=v=v=v=v=v=v=v=v=v=v=v=[ONLY FOR PLAYABLE WEBGL]=v=v=v=v=v=v=v=v=v=v=v=
        //main.SetMaxTimer(20 * Mathf.Pow(0.9f, PlayerPrefs.GetInt("SpeedBox_Wins", 0)));
        //Debug.Log("This Snippet of Code Should Only be Running for the WebGL Build");
        //=^=^=^=^=^=^=^=^=^=^=^=[ONLY FOR PLAYABLE WEBGL]=^=^=^=^=^=^=^=^=^=^=^=
        int a = 0, b = 0, c = 0;
        foreach (GameObject box in boxesArray)
        {
            boxes[c, b, a] = box;

            a++;
            b += a / 3;
            c += b / 3;
            a %= 3;
            b %= 3;
        }
        foreach (Color color in colorsArray)
        {
            boxColors[b, a] = color;

            a++;
            b += a / 3;
            a %= 3;
        }
        NewBox(Vector2.one * -0.5f);
        if (box.GetComponent<SpeedBox_Box>().canRotate)
        {
            boxOffset.transform.rotation = Quaternion.Euler(0, 0, Random.Range(0, 4) * 90);
        }
    }

    public void Finish(Vector2Int direction)
    {
        stage++;
        if (stage > 3)
        {
            effects.Shatter(box.transform.GetChild(1).position, finishColors[main.gameDifficulty - 1]);
            sounds.Play(SpeedBox_Sounds.SFX.Shatter, 1);
            Destroy(box.transform.GetChild(1).gameObject);
            /*/=v=v=v=v=v=v=v=v=v=v=v=[ONLY FOR PLAYABLE WEBGL]=v=v=v=v=v=v=v=v=v=v=v=
            PlayerPrefs.SetInt("SpeedBox_Wins", PlayerPrefs.GetInt("SpeedBox_Wins", 0) + 1);
            Debug.Log("This Snippet of Code Should Only be Running for the WebGL Build");
            if (PlayerPrefs.GetInt("SpeedBox_Wins", 0) > 5)
            {
                PlayerPrefs.SetInt("SpeedBox_Difficulty", PlayerPrefs.GetInt("SpeedBox_Difficulty", 1) + 1);
                PlayerPrefs.SetInt("SpeedBox_Wins", 0);
                if (PlayerPrefs.GetInt("SpeedBox_Difficulty", 1) > 3) PlayerPrefs.SetInt("SpeedBox_Difficulty", 1);
            }
            //=^=^=^=^=^=^=^=^=^=^=^=[ONLY FOR PLAYABLE WEBGL]=^=^=^=^=^=^=^=^=^=^=^=*/
            Invoke("Win", 1);
        }
        else
        {
            Transition(direction);
        }
    }

    void Win()
    {
        GameControllerDevelop.Instance.WinGame();
    }

    public void Shake(float distance, Vector2 direction, float randomness)
    {
        LeanTween.cancel(box);
        direction = (direction + Vector2.one * Random.Range(0, 0)).normalized;
        LeanTween.moveLocal(effects.targetDirection.gameObject, Vector2.one * 0.5f + direction * Mathf.Pow(1.15f, distance) * aplitude, moveTimePerDistance * distance).setEaseOutExpo();
        LeanTween.moveLocal(boxOffset, Vector2.one * 0.5f + direction * Mathf.Pow(1.15f, distance) * aplitude, moveTimePerDistance * distance).setEaseOutExpo().setOnComplete(delegate ()
        {
            LeanTween.moveLocal(boxOffset, Vector2.one * 0.5f, moveTimePerDistance * distance).setEaseInOutSine();
            LeanTween.moveLocal(effects.targetDirection.gameObject, Vector2.one * 0.5f, moveTimePerDistance * distance).setEaseInOutSine();
        });
    }

    GameObject NewBox(Vector2 position)
    {
        GameObject box = Instantiate(boxes[main.gameDifficulty - 1, stage - 1, Random.Range(0, 3)], transform.GetChild(0));
        box.transform.localPosition = position;
        box.GetComponent<Tilemap>().color = boxColors[main.gameDifficulty - 1, stage - 1];
        effects.trailEffect.startColor = boxColors[main.gameDifficulty - 1, stage - 1];
        effects.trailEffect.endColor = boxColors[main.gameDifficulty - 1, stage - 1];
        box.transform.GetChild(1).GetComponent<SpriteRenderer>().color = finishColors[main.gameDifficulty - 1];
        if (this.box != null)
        {
            Destroy(this.box);
        }
        this.box = box;
        player = box.transform.Find("Player").gameObject;
        return box;

    }

    void Transition(Vector2Int direction)
    {
        effects.targetDistance = 1;

        bool clockwise = true;
        if (direction.y > 0)
        {
            clockwise = player.transform.position.x < 0;
        }
        else if (direction.y < 0)
        {
            clockwise = player.transform.position.x > 0;
        }
        else if (direction.x < 0)
        {
            clockwise = player.transform.position.y < 0;
        }
        else if (direction.x > 0)
        {
            clockwise = player.transform.position.y > 0;
        }

        sounds.Play(SpeedBox_Sounds.SFX.Change, 1);
        float angle = direction switch
        {
            { x: 0, y: 1} => 90,
            { x: 0, y: -1 } => 270,
            { x: 1, y: 0 } => 0,
            { x: -1, y: 0 } => 180,
            _ => 0
        };
        trailTargetRotation = Quaternion.AngleAxis(angle, Vector3.forward);
        LeanTween.rotateZ(boxOffset, boxOffset.transform.rotation.eulerAngles.z + (clockwise ? -90 : 90), transitionMoveTime).setEaseInCubic();
        LeanTween.move(effects.targetDirection.gameObject, (Vector2)direction * transitionAplitude, transitionMoveTime).setEaseOutExpo();
        LeanTween.rotateZ(effects.targetDirection.gameObject, angle, 3 * (transitionMoveTime + transitionResetTime)).setEaseOutCubic();
        LeanTween.moveLocal(boxOffset, (Vector2)direction * transitionAplitude + Vector2.one * 0.5f, transitionMoveTime).setEaseOutExpo().setOnComplete(delegate ()
        {
            float targetAngle = 0;
            NewBox(box.transform.localPosition);
            if (box.GetComponent<SpeedBox_Box>().canRotate) 
            {
                targetAngle = Random.Range(0, 4) * 90;
            }
            boxOffset.transform.rotation = Quaternion.Euler(0, 0, targetAngle + (clockwise ? 90 : 270));
            LeanTween.rotateZ(boxOffset, targetAngle, transitionResetTime).setEaseOutCubic();
            LeanTween.moveLocal(boxOffset, Vector2.one * 0.5f, transitionResetTime).setEaseInOutSine();
            LeanTween.move(effects.targetDirection.gameObject, Vector2.zero, transitionResetTime).setEaseInOutSine();
            effects.targetDistance = 0.1f;
        });
    }

    private void Update()
    {
        
    }
}
