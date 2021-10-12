using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    public Dictionary<int, Toggle> activeToggles;
    public Scrollbar scrollbar;

    int excludedGamesFlag;



    public void UpdateFlag(int buildIndex, bool toggle)
    {
        var checkboxGrid = transform.GetChild(0).GetChild(0);
        var currToggle = checkboxGrid.GetChild(buildIndex).GetComponent<Toggle>();
        if (activeToggles.Count > 4 || (toggle == true && !activeToggles.ContainsKey(buildIndex)))
        {
            if (toggle)
            {
                excludedGamesFlag &= ~GetFlagFromBuildIndex(buildIndex);
                activeToggles.Add(buildIndex, currToggle);
            }
            else
            {
                excludedGamesFlag |= GetFlagFromBuildIndex(buildIndex);
                activeToggles.Remove(buildIndex);
            }
            PlayerPrefs.SetInt("excludedGames", excludedGamesFlag);
            SetExcludedGames();
        }
        else {
            currToggle.isOn = true;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        activeToggles = new Dictionary<int, Toggle>();
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
            if (checkbox.GetComponent<Toggle>().isOn) {
                activeToggles.Add(i, checkbox.GetComponent<Toggle>());
            }
            i++;
        }
    }

    public void MoveSlider(Scrollbar slider) {
        var checkboxGrid = transform.GetChild(0).GetChild(0).GetComponent<RectTransform>();
        checkboxGrid.anchoredPosition = baseCheckboxGridPos + new Vector3(0, slider.value * 682, 0);
    }

    public void CloseMenu() {
        // We need to now reload the current scene:
        mainMenu.GetNewGame();
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
        // Make sure main menu has a list of everything we currently have:
        SetExcludedGames();
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
        scrollbar.value -= Input.GetAxis("Mouse ScrollWheel");
        scrollbar.value = Mathf.Clamp(scrollbar.value, 0, 1);
    }
}
