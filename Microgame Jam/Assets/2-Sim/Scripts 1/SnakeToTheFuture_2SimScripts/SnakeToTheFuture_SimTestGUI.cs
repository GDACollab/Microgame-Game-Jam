using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeToTheFuture_SimTestGUI : MonoBehaviour
{
    
    public GameObject Option1Object;
    SnakeToTheFuture_SimSpriteBehavior SpriteBehavior;
    public GameObject Option2Object;

    //when OptionSelected = 0, neither option is selected, this persists until the options first show up
    //when OptionSelected = 1, the top option (steal wallet) is selected
    //when OptionSelected = 2, the bottom option (get engine) is selected
    int OptionSelected = 1;
    GameObject OptionObjectSelected;
    public GameObject TextController;
    SnakeToTheFuture_Typewriter Typewriter;

    //okay this is really dumb but
    //when the options show up, you use spacebar to select one
    //the act of pressing spacebar selects one of the two options
    //however spacebar also advances you into the option prompt
    //the following int is used to nullify the initial spacebar press, allowing you to choose an option
    //again, i know this is dumb, but it's 11:25 PM, so whaddya do
    int NullifySpacebar = 0;

    bool OptionsVisible = false;
    

    // Start is called before the first frame update
    void Start()
    {
        OptionObjectSelected = Option1Object;
        SpriteBehavior = OptionObjectSelected.GetComponent<SnakeToTheFuture_SimSpriteBehavior>();
        SpriteBehavior.ToggleSelectedSprite();
        Typewriter = TextController.GetComponent<SnakeToTheFuture_Typewriter>();
    }

    public void ToggleBothVisible(bool visibility)
    {
        Option1Object.GetComponent<SnakeToTheFuture_SimSpriteBehavior>().ToggleVisible(visibility);
        Option2Object.GetComponent<SnakeToTheFuture_SimSpriteBehavior>().ToggleVisible(visibility);
    }

    // Update is called once per frame
    void Update()
    {
        OptionsVisible = Option1Object.GetComponent<SnakeToTheFuture_SimSpriteBehavior>().Visible;
        //print(OptionSelected);
        if (OptionsVisible == true)
        {
            if ((Input.GetKeyDown(KeyCode.DownArrow))
            | (Input.GetKeyDown(KeyCode.UpArrow))
            | (Input.GetKeyDown(KeyCode.LeftArrow))
            | (Input.GetKeyDown(KeyCode.RightArrow)))
            {
                //choosing other option
                if (OptionSelected == 1)
                {
                    OptionSelected = 2;
                }
                else if (OptionSelected == 2)
                {
                    OptionSelected = 1;
                }
                SpriteBehavior = OptionObjectSelected.GetComponent<SnakeToTheFuture_SimSpriteBehavior>();
                SpriteBehavior.ToggleSelectedSprite();

                //toggle sprite to be selected
                if (OptionSelected == 1)
                {
                    //steal wallet
                    print("steal wallet");
                    OptionObjectSelected = Option1Object;
                    SpriteBehavior = OptionObjectSelected.GetComponent<SnakeToTheFuture_SimSpriteBehavior>();
                    SpriteBehavior.ToggleSelectedSprite();
                }
                if (OptionSelected == 2)
                {
                    //get engine
                    print("get engine");
                    OptionObjectSelected = Option2Object;
                    SpriteBehavior = OptionObjectSelected.GetComponent<SnakeToTheFuture_SimSpriteBehavior>();
                    SpriteBehavior.ToggleSelectedSprite();
                }
            }

        
        if ((Input.GetKeyDown(KeyCode.DownArrow))
            | (Input.GetKeyDown(KeyCode.UpArrow))
            | (Input.GetKeyDown(KeyCode.LeftArrow))
            | (Input.GetKeyDown(KeyCode.RightArrow))
            | (Input.GetKeyDown(KeyCode.Space)))
        {
            print("HERE WE ARE");
            if (OptionSelected == 1)
            {
                print("wallet sent");
            }
            else if (OptionSelected == 2)
            {
                print("engine sent");
            }
            
            if (OptionSelected != 0)
            {
                Typewriter.TextOptionIndex = OptionSelected;
                print(OptionSelected);
            }
            
            if (OptionSelected == 0)
            {
                //print("initialized");
                OptionSelected = 1;
            }
        }
        }
        
    }
}
