using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleManager : MonoBehaviour
{
    private Toggle toggle;
    private Text toggletext;
    // Start is called before the first frame update
    void Start()
    {
        toggle = GetComponent<Toggle>();
        toggletext = toggle.GetComponentInChildren<Text>();

        toggle.isOn = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (toggle.isOn)
        {
            toggletext.text = "Running";
        }
        else
        {
            toggletext.text = "Stop";
        }
    }
}
