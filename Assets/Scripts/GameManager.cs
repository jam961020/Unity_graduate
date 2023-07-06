using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using System.Diagnostics;

public class GameManager : MonoBehaviour
{

    

    Process process = new Process();
    // Start is called before the first frame update
    void Start()
    {
        process.StartInfo.FileName = @"python";
        process.StartInfo.Arguments = @".\python\test.py";
        process.StartInfo.RedirectStandardOutput = true;
        process.StartInfo.UseShellExecute = false;
        process.StartInfo.CreateNoWindow = true;

        
        process.Start();
        process.BeginOutputReadLine();

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
