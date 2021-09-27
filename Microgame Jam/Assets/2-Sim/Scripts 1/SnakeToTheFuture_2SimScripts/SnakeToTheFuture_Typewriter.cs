using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Text))]
public class SnakeToTheFuture_Typewriter : MonoBehaviour
{
    public GameController gameController;
    public float Delay = 0.1f;
    public int MasterTextSkip = 5;
    int TextSkip;

    //----------

    [TextArea(5,10)]
    public string[] FullTexts;

    int MaxNumberTexts;
    int CurrentNumberText;

    string CurrentText = "";
    Text Text;
    public SpriteRenderer Textbox;

    [HideInInspector]
    public bool AwaitSpace;

    //----------

    public GameObject DialogueOptionController;
    public int TextsBeforeOption;

    [HideInInspector]

    public int TextOptionIndex = 0;

    public int WalletText;
    public int EngineText;

    //----------

    public SnakeToTheFuture_CharacterAnimator SnakeAnimator;
    public SnakeToTheFuture_CharacterAnimator SnakeLadyAnimator;
    public SnakeToTheFuture_PortalAnimations PortalAnimations;

    //---------
    
    // Start is called before the first frame update
    void Start()
    {
        AwaitSpace = false;
        TextSkip = MasterTextSkip;
        Text = gameObject.GetComponent<Text>();
        StartCoroutine(ShowText());
        MaxNumberTexts = FullTexts.Length;
        CurrentNumberText = 0;
        //DialogueOptions.SetActive(false);
        Textbox.enabled = true;
        SnakeAnimator.ChangeSprite("main");
        SnakeLadyAnimator.ChangeSprite("main");
        //PortalAnimations = Portal.GetComponent<SnakeToTheFuture_PortalAnimations>();
        
    }


    IEnumerator ShowText()
    {
        for (int i = 0;  i < FullTexts[CurrentNumberText].Length; i = i+TextSkip)
        {
            CurrentText = FullTexts[CurrentNumberText].Substring(0,i);
            Text.text = CurrentText;

            if (FullTexts[CurrentNumberText].Length-CurrentText.Length <= TextSkip)
            {
                TextSkip = 1;
                //print("REDUCED");
            }

            if (i == FullTexts[CurrentNumberText].Length-1)
            {
                AwaitSpace = true;
                TextSkip = MasterTextSkip;
                //print("END");
            }

            yield return new WaitForSeconds(Delay);
        }
    }

    IEnumerator DelayEnd (float time, string ending)
    {
        yield return new WaitForSeconds(time);
        if (ending == "lose")
        {
            print("lose");
            GameController.Instance.LoseGame();
        }
        else if (ending == "win")
        {
            print("win");
            PortalAnimations.PortalVisible(true);
            yield return new WaitForSeconds(time);
            StartCoroutine(GetMerge());
            print("joe mama");
        }
    }
    IEnumerator GetMerge()
    {
        var currScene = SceneManager.GetSceneByName("snakeToTheFuture_2sim");
        var nextScene = SceneManager.GetSceneByName("snakeToTheFuture_3heist");
        var loading = SceneManager.LoadSceneAsync(sceneName: "snakeToTheFuture_3heist", LoadSceneMode.Additive);
        while (!loading.isDone)
        {
            yield return null;
        }
        foreach (GameObject o in currScene.GetRootGameObjects())
        {
            Destroy(o);
        }
        SceneManager.MergeScenes(nextScene, currScene);
        SceneManager.UnloadSceneAsync(nextScene);
    }

    void Update()
    {
        if (AwaitSpace)
        {
            //change sprites to blushes at Text #
            if (CurrentNumberText == 2)
            {
                SnakeAnimator.ChangeSprite("alt");
                SnakeLadyAnimator.ChangeSprite("alt");
            }
            else
            {
                SnakeAnimator.ChangeSprite("main");
                SnakeLadyAnimator.ChangeSprite("main"); 
            }
            
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (CurrentNumberText == TextsBeforeOption)
                {
                    //print("SHOW OPTION");
                    DialogueOptionController.GetComponent<SnakeToTheFuture_SimTestGUI>().ToggleBothVisible(true);
                    Textbox.enabled = false;  
                    Text.enabled = false;
                    //AwaitSpace = false;
                }

                if (CurrentNumberText > TextsBeforeOption)
                {
                    //print("HIDE OPTION");
                    DialogueOptionController.GetComponent<SnakeToTheFuture_SimTestGUI>().ToggleBothVisible(false);
                    Textbox.enabled = true;
                    Text.enabled = true;
                }

                if (TextOptionIndex == 1)
                {
                    //steal wallet
                    CurrentNumberText = WalletText;
                    StartCoroutine(ShowText());
                    StartCoroutine(DelayEnd(1f, "lose"));
                    AwaitSpace = false;
                    
                }

                if (TextOptionIndex == 2)
                {
                    //get engine
                    CurrentNumberText = EngineText;
                    print("PORTAL APPEAR");
                    StartCoroutine(ShowText());
                    StartCoroutine(DelayEnd(1f, "win"));
                    AwaitSpace = false;
                }

                if (TextOptionIndex == 0)
                {
                    //print("RECIEVED");
                    if (CurrentNumberText != FullTexts.Length)
                    {
                        CurrentNumberText++;
                        StartCoroutine(ShowText());
                        //print("PROCESSED");
                        AwaitSpace = false;
                    }
                }
            }
        }
    }
}
