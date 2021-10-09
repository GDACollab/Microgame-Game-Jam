using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class ScoreTracker : MonoBehaviour
{
    public GameObject WinState;
    public GameObject LoseState;

    public GameObject controllerWinPrefab;
    public GameObject controllerLosePrefab;
    public GameObject controllerNeutralPrefab;

    public List<GameObject> liveObjects;
    List<GameObject> livesToDestroy = new List<GameObject>();

    bool didLose = false;

    private void OnEnable()
    {
        foreach (GameObject life in livesToDestroy) {
            Destroy(life);
        }
        livesToDestroy = new List<GameObject>();
        // This is to prevent any one animation from starting when TransitionScene is reloaded.
        // Since these objects are not root level, their active states are saved, so we have to specifically make sure
        // that they're set to be active.
        WinState.SetActive(false);
        LoseState.SetActive(false);
    }

    protected void SetAnim(GameObject prefab) {
        int lives = GameController.Instance.maxFails - GameController.Instance.gameFails;
        for (int i = 0; i < liveObjects.Count; i++)
        {
            GameObject life = liveObjects[i];
            GameObject newLife = Instantiate(prefab, life.transform.position, life.transform.rotation, life.transform.parent);
            liveObjects[i] = newLife;
            // Iterate through all the displays of lives, set the ones that are unused to be not active.
            if (lives > 0)
            {
                newLife.SetActive(true);
            }
            else {
                newLife.SetActive(false);
            }
            lives--;
            livesToDestroy.Add(life);
            life.SetActive(false);
        }
    }

    public void SetNeutralAnim() {
        SetAnim(controllerNeutralPrefab);
    }

    public void DidWin(bool win, UnityEvent canHideGame, UnityEvent canShowGame, UnityEvent canUnpause) {
        GameObject prefabToUse;
        if (win)
        {
            prefabToUse = controllerWinPrefab;
        }
        else
        {
            prefabToUse = controllerLosePrefab;
        }

        SetAnim(prefabToUse);

        if (win)
        {
            WinState.SetActive(true);
            WinState.GetComponent<TransitionAnimation>().SetEvents(canHideGame, canShowGame, canUnpause);
        }
        else {
            LoseState.SetActive(true);
            LoseState.GetComponent<TransitionAnimation>().SetEvents(canHideGame, canShowGame, canUnpause);
        }
    }
}
