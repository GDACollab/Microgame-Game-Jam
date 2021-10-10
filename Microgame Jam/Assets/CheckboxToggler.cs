using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UnityEventCheckbox : UnityEvent<int, bool> { };

public class CheckboxToggler : MonoBehaviour
{
    public int associatedBuildIndex;
    public UnityEventCheckbox checkboxCallback = new UnityEventCheckbox();
    // Start is called before the first frame update
    void Start()
    {
        GetComponent <Toggle>().onValueChanged.AddListener(WasToggled);
    }

    public void WasToggled(bool toggle) {
        checkboxCallback.Invoke(associatedBuildIndex, toggle);
    }
}
