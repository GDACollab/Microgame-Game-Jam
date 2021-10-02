using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoneSkip_ChargingState : StoneSkip_State
{
    //Abstract class setup, don't worry abt this
    StoneSkip_ChargingState(string stateName) : base(stateName) { }

    //You can make gameobjects
    public GameObject landObject;
    public GameObject smokeObject;
    public GameObject fillObject;
    public GameObject tutorialObject;
    public Slider charge;

    private bool TutorialOver = false;
    private bool TimeOut = false;
    private bool ChargeSet = false;
    public float StartTime;

    bool bounce = true;
    bool chargeup = true;

    public override void StateStart() {
        Debug.Log("Entered Charging State");
        StartCoroutine(TimerTutorial());
        chargeup = true;
        bounce = true;
    }

    //This is called on FixedUpdate()
    public override void StateUpdate() {
    	if (TutorialOver && !ChargeSet) {
            if (chargeup){
                charge.value += (Time.deltaTime/1.7f);
                if (charge.value >= 1){
                    chargeup = false;
                }
            }else{
                charge.value -= (Time.deltaTime/1.7f);
            }
            check_fill_color();
            if (bounce){
                landObject.transform.localScale += new Vector3(0, .005f, 0);
                landObject.transform.position += new Vector3(0, .05f, 0);
                if (landObject.transform.localScale.y >= .65){
                    bounce = false;
                }
            }else{
                landObject.transform.localScale -= new Vector3(0, .005f, 0);
                landObject.transform.position -= new Vector3(0, .05f, 0);
                if (landObject.transform.localScale.y <= .6){
                    bounce = true;
                }
            }
    	}
    }

    //Get input
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && TutorialOver && activeState)
        {
            ChargeSet = true;
        }
    }

    private void check_fill_color(){
        if (charge.value < .4){
            fillObject.GetComponent<Image>().color = new Color(1, 0.92f, 0.016f, 1);
        }else{
            fillObject.GetComponent<Image>().color = new Color(0,1,0,1);
        }
    }

    private IEnumerator TimerTutorial() {
    	Debug.Log("TimerTut Started");
    	yield return new WaitForSeconds(3f);
    	TutorialOver = true;
        tutorialObject.SetActive(false);
    	StartCoroutine(TimerCharge());
    	Debug.Log("TimerTut Ended");
    }

    private IEnumerator TimerCharge() {
    	Debug.Log("TimerCharge Started");
    	yield return new WaitForSeconds(3.2f);
    	TimeOut = true;
        ChargeSet = true;
        smokeObject.transform.localScale = new Vector3(1,1,1);
        StartCoroutine(IslandMove());
        StoneSkip_StaticData.launchVelocity = charge.value;
        SwitchState("Skipping");
    	Debug.Log("TimerCharge Ended");
    }

    private IEnumerator IslandMove() {
        yield return new WaitForSeconds(.2f);
        while (landObject.transform.position.x > -20)
        {
            yield return new WaitForEndOfFrame();
            smokeObject.transform.position += new Vector3(-.25f, 0, 0);
            landObject.transform.position += new Vector3(-.25f, 0, 0);
        }
    }
}
