using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ChecklistManager : MonoBehaviour
{
    public MainMenuNavigation mainMenu;
    // This should be in order of their scene build order:
    [Tooltip("This should be in order of their scene build order.")]
    public List<string> manualGameNames;
    public GameObject toggleText;
    public Vector3 baseCheckboxGridPos;

    int excludedGamesFlag;



    public void UpdateFlag(int buildIndex, bool toggle)
    {
        if (toggle)
        {
            excludedGamesFlag &= ~GetFlagFromBuildIndex(buildIndex);
        }
        else
        {
            excludedGamesFlag |= GetFlagFromBuildIndex(buildIndex);
        }
        PlayerPrefs.SetInt("excludedGames", excludedGamesFlag);
        SetExcludedGames();
    }

    // Start is called before the first frame update
    void Start()
    {
        GetFlags();
        var checkboxGrid = transform.GetChild(0).GetChild(0);
        baseCheckboxGridPos = checkboxGrid.GetComponent<RectTransform>().anchoredPosition;
        int i = 0;
        foreach (string name in manualGameNames) {
            var checkbox = Instantiate(toggleText, checkboxGrid);
            checkbox.GetComponentInChildren<Text>().text = name;
            checkbox.GetComponent<CheckboxToggler>().associatedBuildIndex = i;
            checkbox.GetComponent<CheckboxToggler>().checkboxCallback.AddListener(UpdateFlag);
            checkbox.GetComponent<Toggle>().isOn = (((excludedGamesFlag >> i) & 1) == 0);
            i++;
        }
    }

    public void MoveSlider(Scrollbar slider) {
        var checkboxGrid = transform.GetChild(0).GetChild(0).GetComponent<RectTransform>();
        checkboxGrid.anchoredPosition = baseCheckboxGridPos + new Vector3(0, slider.value * 682, 0);
    }

    public void CloseMenu() {
        gameObject.SetActive(false);
    }

    public void GetFlags() {
        if (PlayerPrefs.HasKey("excludedGames"))
        {
            excludedGamesFlag = PlayerPrefs.GetInt("excludedGames");

            // Set relevant checkboxes.
        }
        else {
            // Set the excludedGamesFlag to nothing.
            excludedGamesFlag = 0;

            // Set all the checkboxes.

            PlayerPrefs.SetInt("excludedGames", excludedGamesFlag);
            
        }
    }

    private int GetFlagFromBuildIndex(int buildIndex) {
        return 1 << buildIndex;
    }

    // Should be called on menu exit:
    public void SetExcludedGames() {
        List<int> gamesToExclude = new List<int>();
        for (int i = 0; i < SceneManager.sceneCountInBuildSettings - mainMenu.minSceneIndex; i++) {
            var flag = excludedGamesFlag >> i;
            if ((flag & 1) == 1) {
                gamesToExclude.Add(i + mainMenu.minSceneIndex);
            }
        }
        mainMenu.OnExcludedGamesUpdate(gamesToExclude);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
