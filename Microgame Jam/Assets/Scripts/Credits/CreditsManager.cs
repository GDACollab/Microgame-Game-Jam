using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreditsManager : MonoBehaviour
{
    [Header("Objects/Assets")]
    [Tooltip("The .csv file to parse")]
    /*should follow format of
    Game Title
    Member, roles
    Member, roles
    Game Title
    Member, roles
    etc*/
    public TextAsset creditsFile;

    [Tooltip("Object to attach credits text to")]
    public GameObject creditsObject;

    [Tooltip("Canvas object holding the credits")]
    public GameObject canvasObject;

    [Header("Misc")]
    [Tooltip("How fast the credits should scroll")]
    public float scrollSpeed;

    [Tooltip("data structure to hold info from creditsFile")]
    private List<List<string>> creditsList;

    // Start is called before the first frame update
    void Start()
    {
        creditsList = new List<List<string>>();
        ParseCSV();
        WriteCredits();
    }

    void ParseCSV()
    {
        string[] lines = creditsFile.text.Split('\n');
        int gameCount = 0;

        //read CSV lines into creditsList. Each entry in creditsList is a List of strings
        //where the first entry is the game title and remaining entries are team member names and roles
        for(int lineCount = 0; lineCount < lines.Length; lineCount++) 
        {
            string[] splitLine = lines[lineCount].Split(',');
            //check if current line is a game title
            if(splitLine[1].CompareTo("") == 0) 
            {
                gameCount++;
                //if it's a title, add key to creditsList
                creditsList.Add(new List<string>());
                creditsList[gameCount - 1].Add(splitLine[0]);
            }
            //otherwise line is member name/role, format and add to list
            else
            {
                //trim excess junk from end of the line
                string formattedLine = lines[lineCount].TrimEnd(',', '\n', '\r');
                
                //replace first comma with -
                char[] ch = formattedLine.ToCharArray();
                ch[formattedLine.IndexOf(",")] = '-';
                formattedLine = new string(ch);

                //add in some spaces for better readability
                formattedLine = formattedLine.Replace(",", ", ");
                formattedLine = formattedLine.Replace("-", " - ");

                //add to creditsList
                creditsList[gameCount - 1].Add(formattedLine);
            }
        }
    }

    void WriteCredits()
    {
        //create text component on creditsObject and set it up properly
        Text creditsText = creditsObject.AddComponent<Text>();
        creditsText.font = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;
        creditsText.text = "";
        creditsText.fontSize = 20;
        creditsText.alignment = TextAnchor.UpperCenter;
        creditsText.verticalOverflow = VerticalWrapMode.Overflow;

        //read through creditsList and add entries to the text component
        foreach (List<string> l in creditsList)
        {
            foreach(string s in l)
            {
                creditsText.text += s + "\n";
            }
            creditsText.text += "\n\n";
        }

        //set position of creditsObject to be just off the screen
        creditsObject.transform.position = new Vector3(Screen.width * 0.5f, -creditsObject.GetComponent<RectTransform>().rect.height / 2, 0);
    }

    // Update is called once per frame
    void Update()
    {
        //scroll credits up the screen
        creditsObject.transform.position += new Vector3(0, scrollSpeed * Time.deltaTime, 0);   
    }
}
