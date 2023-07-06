using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor.Scripting.Python;
using UnityEditor;
#endif

using System.IO;
using UnityEngine.UI;

using System.Diagnostics;
using System;

public class test : MonoBehaviour
{
    bool flag = false;
    private Texture2D change_img;
    public RawImage thisImg;

    //#if UNITY_EDITOR
    //    [MenuItem("Python/Ensure Naming")]
    //#endif
    //    static void RunEnsureNaming()
    //    {
    //        string scriptPath = Path.Combine(Application.dataPath, "test.py");

    //        UnityEngine.Debug.Log("It's wokring on out");

    //#if UNITY_EDITOR
    //        PythonRunner.RunFile(scriptPath);
    //        UnityEngine.Debug.Log("It's working");
    //#endif
    //    }

#if UNITY_EDITOR
    [MenuItem("Python/test_Diagnostics")]
#endif
    static void pythonTest()
    {
        //if (Input.GetKeyDown(KeyCode.R))
        //{
            try
            {
                Process psi = new Process();
                psi.StartInfo.FileName = "C:/Users/user/yolov5-master/venvyolo/Scripts/python.exe";
                psi.StartInfo.Arguments = "C:/Users/user/Documents/GitHub/Unity_graduate/Assets/test.py";
                psi.StartInfo.CreateNoWindow = true;
                psi.StartInfo.UseShellExecute = false;
                psi.Start();
                UnityEngine.Debug.Log("python execute");
            }
            catch (Exception e)
            {
                UnityEngine.Debug.Log("python fuck off: " + e.Message);
            }
        //}
    }

    static void PrintHelloWorldFromPython()
    {
#if UNITY_EDITOR
        PythonRunner.RunString(@"
import UnityEngine;
import cv2;
cv2.imshow(
UnityEngine.Debug.Log('why not?')
");
#endif
    }

    private void SystemIOFileLoad()
    {
        pythonTest();

        if (!flag)
        {
            var path = "F:/testing/test.png";
            byte[] byteTexture = File.ReadAllBytes(path);
            if (byteTexture.Length > 0)
            {
                flag = true;
                UnityEngine.Debug.Log("Loding success");
                Texture2D texture = new Texture2D(0, 0);
                texture.LoadImage(byteTexture);
                UnityEngine.Debug.Log(byteTexture.Length);
                UnityEngine.Debug.Log(thisImg.texture);
                thisImg.texture = texture;
            }
            

        }
    }
    // Start is called before the first frame update
    void Start()
    {
        //    PrintHelloWorldFromPython();
        //UnityEngine.Debug.Log("It's working on real outside");
#if UNITY_EDITOR
        //RunEnsureNaming();
#endif
        
    }

    // Update is called once per frame
    void Update()
    {
        //pythonTest();

        //change_img = Resources.Load("test") as Texture2D;

        //UnityEngine.Debug.Log(change_img);

        //thisImg.texture = change_img;

        if (thisImg.texture==null)
        {
            SystemIOFileLoad();
        }
    }
}