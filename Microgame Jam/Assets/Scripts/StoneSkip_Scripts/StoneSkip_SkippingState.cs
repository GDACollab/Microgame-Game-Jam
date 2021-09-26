using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoneSkip_SkippingState : StoneSkip_State
{
    StoneSkip_SkippingState(string stateName) : base(stateName) { }

    public GameObject lavaObject;
    public GameObject rockObject;
    public GameObject hotrockObject;
    public GameObject beegboatObject;
    public GameObject paperboatObject;
    public GameObject homerunboatObject;
    public GameObject fillObject;
    public GameObject smokeObject;
    public Slider powerBar;
    // max height of the rock
    float h = 10;
    //Current height of the rock 
    float ch = 10;
    // time it should take rock to reach max height
    float th = .955f;
    // location of the surface of the lava
    float surface = -4f;
    // initial velocity
    float vi = 0;
    // acceleration
    float a = 0;
    float rock_velocity = 0f;
    bool input_checked = false;
    string ending = null;
    
    //This is in percent of velocity lost per frame
    public float velocityDecayRate;

    Vector3 rock_rotation;

    private StoneSkip_TextureScroller scroller;

    private void Start()
    {
        scroller = GetComponentInChildren<StoneSkip_TextureScroller>();
        ch = 10f;
        h = 10f;
        th = .955f;
        surface = -4f;
        input_checked = false;
    }

    private void refreshA() {
        a = -2 * ch / (th * th);
        vi = 2 * ch * th;
    }

    private void Update(){
        if (Input.GetKeyDown(KeyCode.Space) && activeState && !input_checked && ending == null)
        {
            if (rockObject.transform.position.y - surface < .3f){
                powerBar.value += .3f;
                hotrockObject.GetComponent<SpriteRenderer>().color = new Color(1,1,1,1);
                hotrockObject.GetComponent<AudioSource>().Play();
                StartCoroutine(fade_hot());
                print("Perfect!");
                input_checked = true;
            }else if(rockObject.transform.position.y - surface < 1f){
                powerBar.value += .15f;
                print("Nice!");
                input_checked = true;
            }else {
                print("Missed!");
                input_checked = true;
            }
            check_fill_color();
        }
    }

    private void check_fill_color(){
        if (powerBar.value < .4){
            fillObject.GetComponent<Image>().color = new Color(1, 0.92f, 0.016f, 1);
        }else{
            fillObject.GetComponent<Image>().color = new Color(0,1,0,1);
        }
        ch = 10 * powerBar.value + 4;
    }

    private void reset_input(){
        input_checked = false;
    }

    public override void StateStart()
    {
        Debug.Log("Entered Skipping State");
        scroller.active = true;
        // this calculates a and vi to match what h and th are
        // should allow you to change h and th as needed without messing with a and vi
        refreshA();
        Debug.Log("a: " + a + "vi: " + vi);
        rockObject.transform.position = new Vector3(0, surface, 0);
    }

    //This is called on FixedUpdate()
    public override void StateUpdate()
    {
        //Decay velocity
        powerBar.value -= velocityDecayRate;
        check_fill_color();
        //ch = h * powerBar.value;
        //Debug.Log("h: " + h + "value: " + powerBar.value);
        if (GameController.Instance.gameTime >= 17)
        {
            if (powerBar.value < .4 && ending == null) {
                ending = "time_up";
            }
            else if (ending == null){
                ending = "win";
            }
        }
        else if (powerBar.value == 0 && ending == null) {
            ending = "power_low";
        }

        // when the rock is at the surface bounce it back up
        if (rockObject.transform.position.y <  surface){
            bounce();
        }
        rock_velocity += a * Time.deltaTime;
        // just some flair for fun, could tie this to momentum
        rockObject.transform.Rotate(rock_rotation);
        rockObject.transform.position += new Vector3(0, rock_velocity*Time.deltaTime, 0);
    }

    private void bounce(){
        if (ending != null){
            if (ending == "done"){
                return;
            }
            StartCoroutine(move_ship());
            ending = "done";
        }
        StartCoroutine(fade_hot());
        refreshA();
        rock_velocity = vi;
        rockObject.transform.localScale += new Vector3(.1f, .1f, 0);
        rock_rotation = new Vector3(0, 0, Random.Range(-10f,10f));
        Invoke("reset_input", th);
    }

    private IEnumerator fade_hot(){
        Color alpha = hotrockObject.GetComponent<SpriteRenderer>().color;
        while (alpha.a > 0){
            yield return new WaitForEndOfFrame();
            alpha.a -= .01f;
            hotrockObject.GetComponent<SpriteRenderer>().color = alpha;
        }
    }

    private IEnumerator move_ship(){
        switch (ending){
            case "win":
                while(beegboatObject.transform.position.x > 0){
                    yield return new WaitForEndOfFrame();
                    beegboatObject.transform.position += new Vector3(-.13f,0,0);
                }
                smokeObject.transform.position = new Vector3(0,-1,0);
                smokeObject.transform.localScale = new Vector3(1.5f,1.5f,0);
                beegboatObject.GetComponent<AudioSource>().Play();
                GameController.Instance.WinGame();
                break;
            case "power_low":
                while(paperboatObject.transform.position.x > 0){
                    yield return new WaitForEndOfFrame();
                    paperboatObject.transform.position += new Vector3(-.14f,0,0);
                }
                paperboatObject.GetComponent<AudioSource>().Play();
                GameController.Instance.LoseGame();
                break;
            case "time_up":
                while(homerunboatObject.transform.position.x > 0){
                    yield return new WaitForEndOfFrame();
                    homerunboatObject.transform.position += new Vector3(-.13f,0,0);
                }
                homerunboatObject.GetComponent<AudioSource>().Play();
                GameController.Instance.LoseGame();
                break;
        }
        
    }
}


