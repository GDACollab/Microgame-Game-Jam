using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TestRunGameController : MonoBehaviour
{
    [Tooltip("The index of the transition scene.")]
    public int transitionSceneIndex;

    [Tooltip("How much time should pass before the test starts")]
    public float testDelay = 3f;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(StartTest());
    }

    IEnumerator StartTest()
    {
        Debug.Log($"Starting test in ~{testDelay} seconds...");

        yield return new WaitForSeconds(testDelay);

        GameController.Instance.WinGame();
    }
}
