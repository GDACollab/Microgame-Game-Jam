using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class snakeToTheFuture_heistMovement : MonoBehaviour
{
    Vector2 dir = Vector2.right;
    int direction = 3;
    int directionChange = 0;
    bool ate = false;
    int objective = 0;
    List<Transform> tail = new List<Transform>();
    public GameObject snakeToTheFuture_snakeTail;
    public GameObject apple2;
    public GameObject portal2;
    public Sprite Tail;
    public Sprite Turn;
    public Sprite TurnOut;
    public Sprite ActualTail;
    public GameController gameController;
    public GameObject Original;
    int i = 0;
    // Use this for initialization
    void Start () {
        InvokeRepeating("Move", 0.3f, 0.3f);    
    }
   
    // Update is called once per frame
    void Update () {
        if (Input.GetKeyDown(KeyCode.RightArrow)){
        transform.rotation = Quaternion.Euler(0,0,0);
        direction=0;
        return;
        }
        if (Input.GetKeyDown(KeyCode.DownArrow)){
        transform.rotation = Quaternion.Euler(0,0,-90);
        direction=3;
        return;
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow)){
        transform.rotation = Quaternion.Euler(0,0,180);
        direction=2;
        return; 
        }
        if (Input.GetKeyDown(KeyCode.UpArrow)){
        transform.rotation = Quaternion.Euler(0,0,90);
        direction=1;
        }
    }
   
    void Move() {
        Vector2 oldPos = transform.position;
        transform.Translate(dir);
        if (i<5 | ate==true){
        GameObject g =(GameObject)Instantiate(snakeToTheFuture_snakeTail,
                                              oldPos,
                                              Quaternion.identity);
        tail.Insert(0, g.transform);
        i++;
        ate = false;
        }
        if (tail.Count > 0) {
            tail.Last().position = oldPos;
        if (direction==0){
            tail.Last().transform.rotation = Quaternion.Euler(0,0,0);
        } else if (direction==1){
            tail.Last().transform.rotation = Quaternion.Euler(0,0,90);
        } else if (direction==2){
            tail.Last().transform.rotation = Quaternion.Euler(0,0,180);
        } else {
            tail.Last().transform.rotation = Quaternion.Euler(0,0,-90);
        }
        if (directionChange==direction){
            tail.Last().GetComponent<SpriteRenderer>().sprite = Tail;
        } else if (directionChange==0 && direction==1){
            tail.Last().GetComponent<SpriteRenderer>().sprite = TurnOut;
            tail.Last().transform.rotation = Quaternion.Euler(0,0,180);
        } else if (directionChange==0 && direction==3){
            tail.Last().GetComponent<SpriteRenderer>().sprite = Turn;
        } else if (directionChange==1 && direction==2){
            tail.Last().GetComponent<SpriteRenderer>().sprite = TurnOut;
            tail.Last().transform.rotation = Quaternion.Euler(0,180,0);
        } else if (directionChange==1 && direction==0){
            tail.Last().GetComponent<SpriteRenderer>().sprite = Turn;
        } else if (directionChange==2 && direction==1){
            tail.Last().GetComponent<SpriteRenderer>().sprite = Turn;
        } else if (directionChange==2 && direction==3){
            tail.Last().GetComponent<SpriteRenderer>().sprite = TurnOut;
            tail.Last().transform.rotation = Quaternion.Euler(0,0,0);
        } else if (directionChange==3 && direction==0){
            tail.Last().GetComponent<SpriteRenderer>().sprite = TurnOut;
            tail.Last().transform.rotation = Quaternion.Euler(0,0,90);
        } else if (directionChange==3 && direction==2){
            tail.Last().GetComponent<SpriteRenderer>().sprite = Turn;
        } 
        tail.Insert(0, tail.Last());
        tail.RemoveAt(tail.Count-1);
        if (tail.Last().GetComponent<SpriteRenderer>().sprite == TurnOut){
            tail.Last().transform.rotation = tail[tail.Count()-2].transform.rotation;
        }
        tail.Last().GetComponent<SpriteRenderer>().sprite = ActualTail;
        directionChange=direction;
        }
    }

    void OnTriggerEnter2D(Collider2D coll) {
        if (coll.name.StartsWith("snakeToTheFuture_apple")){
            Destroy(coll.gameObject);
            ate = true;
            objective++;
            return;
        }
        if (coll.name.StartsWith("snakeToTheFuture_portal1")){
            Destroy(coll.gameObject);
            Original.GetComponent<SpriteRenderer>().enabled = false;
            GameObject carChassise =(GameObject)Instantiate(apple2,
                                            new Vector2 (0,20),
                                              Quaternion.identity);
            GameObject portalTheSecond =(GameObject)Instantiate(portal2,
                                            new Vector2 (5,-5),
                                              Quaternion.identity);
            return;
        }
        if (coll.name.StartsWith("snakeToTheFuture_portal2")){
            gameController.WinGame();
            foreach (GameObject o in Object.FindObjectsOfType<GameObject>()) {
                Destroy(o);
            }
            return;
        }
        gameController.LoseGame();
        foreach (GameObject o in Object.FindObjectsOfType<GameObject>()) {
            Destroy(o);
        }
    }
}

