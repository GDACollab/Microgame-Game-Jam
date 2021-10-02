using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class UnityEventGetFloat : UnityEvent<float> { }

public class SniperGolf_PowerMinigameController : MonoBehaviour
{
    /// <summary>
    /// How the slider should move when activated from 0% to 100% (this is mirrored when the slider is going back down).
    /// </summary>
    [Tooltip("How the slider should move when activated from 0% to 100%")]
    public AnimationCurve sliderCurve;

    /// <summary>
    /// What color the slider should turn as it moves from 0% to 100%
    /// </summary>
    [Tooltip("What color the slider should turn as it moves from 0% to 100%")]
    public Gradient sliderGradient;

    /// <summary>
    /// For other scripts to get a callback from if the space key was hit.
    /// </summary>
    public UnityEventGetFloat hitSpace = new UnityEventGetFloat();

    public UnityEventGetFloat hitSpaceUpdate = new UnityEventGetFloat();

    /// <summary>
    /// How fast the slider should be going every frame.
    /// </summary>
    [Tooltip("How fast the slider should be going every frame.")]
    public float sliderSpeed = 1.0f;

    Slider progressSlider;
    Image fillImage;
    float powerProgress = 0;
    bool isGoingDown = false;

    private void Start()
    {
        progressSlider = GetComponentInChildren<Slider>();
        fillImage = progressSlider.fillRect.GetComponent<Image>();
    }

    // As soon as this is activated, start moving the slider up.
    private void OnEnable()
    {
        powerProgress = 0;
        isGoingDown = false;
    }

    // This is only activated when the slider is enabled, so no need 
    private void Update()
    {
        progressSlider.value = sliderCurve.Evaluate(powerProgress);
        fillImage.color = sliderGradient.Evaluate(powerProgress);
        if (isGoingDown) {
            powerProgress -= Time.deltaTime * sliderSpeed;
            // We went up, then back down, and the space bar wasn't hit, so stop sliding:
            if (powerProgress <= 0) {
                isGoingDown = false;
                gameObject.SetActive(false);
                hitSpace.Invoke(0);
            }
        } else {
            powerProgress += Time.deltaTime * sliderSpeed;
            if (powerProgress >= 1) {
                isGoingDown = true;
            }
        }
        hitSpaceUpdate.Invoke(powerProgress);

        if (Input.GetKeyDown("space")) {
            hitSpace.Invoke(powerProgress);
            gameObject.SetActive(false);
        }
    }
}
