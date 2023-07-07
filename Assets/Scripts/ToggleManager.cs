using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleManager : MonoBehaviour
{
    private Toggle toggle;
    private Text toggletext;
    private StreamingManager streamingManager;
    int framecounter;
    // Start is called before the first frame update

    bool State()
    {
        return toggle.isOn;
    }

    void Start()
    {
        toggle = GetComponent<Toggle>();
        toggletext = toggle.GetComponentInChildren<Text>();
        streamingManager = GetComponent<StreamingManager>();
        framecounter = 0;

        toggle.isOn = false;
        streamingManager.removedir(streamingManager.unityPath());
        
    }

    // Update is called once per frame
    void Update()
    {
        if (toggle.isOn)
        {
            toggletext.text = "Running";
            framecounter++;
            streamingManager.Stream(framecounter);
        }
        else
        {
            toggletext.text = "Stop";
            framecounter = 0;
            streamingManager.removedir(streamingManager.unityPath());
        }
    }
}
