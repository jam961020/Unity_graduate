using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class StreamingManager : MonoBehaviour
{
    private ToggleManager toggleManager;
    public Camera streamcamera;
    private int resWidth;
    private int resHeight;
    public string path;
    string unitypath;
    string pythonpath;
    // Start is called before the first frame update
    void Start()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 30;

        //toggleManager = GetComponent<ToggleManager>();
        resWidth = 640;
        resHeight = 360;

        path = @"C:\Users\" + Environment.UserName + @"\UnityGraduate\";

        unitypath = path + @"UnityStream\";
        pythonpath = path + @"PythonStream";
    }
#if UNITY_EDITOR
    [MenuItem("test/username")]
    static void test()
    {
        string path = @"C:\Users\" + Environment.UserName + @"\test\" + @"intothetest\";

        Debug.Log(path);

        Directory.CreateDirectory(path);
    }
#endif

    void makedir()
    {

        DirectoryInfo dirUnity = new DirectoryInfo(unitypath);
        DirectoryInfo dirPython = new DirectoryInfo(pythonpath);

        if (!dirPython.Exists)
        {
            Directory.CreateDirectory(pythonpath);
        }
        if (!dirUnity.Exists)
        {
            Directory.CreateDirectory(unitypath);
        }
    }

    public void removedir(string delpath)
    {

        if (delpath == null) return;

        DirectoryInfo dirdelpath = new DirectoryInfo(delpath);

        if (dirdelpath.Exists && delpath !=null)
        {
            File.SetAttributes(delpath, FileAttributes.Normal);

            foreach (string _folder in Directory.GetDirectories(delpath))
            {
                removedir(_folder);
            }

            foreach (string _file in Directory.GetFiles(delpath))
            {
                File.SetAttributes(_file, FileAttributes.Normal);
                File.Delete(_file);
            }
            Directory.Delete(delpath);
        }
    }

    public string unityPath()
    {
        return unitypath;
    }

    public string Stream(int count)
    {
        makedir();

        string name;
        name = unitypath + "Image" + count.ToString() + ".png";

        RenderTexture rt = new RenderTexture(resWidth, resHeight, 24);
        streamcamera.targetTexture = rt;
        Texture2D screenShot = new Texture2D(resWidth, resHeight, TextureFormat.RGB24, false);
        Rect rec = new Rect(0, 0, screenShot.width, screenShot.height);
        streamcamera.Render();
        RenderTexture.active = rt;
        screenShot.ReadPixels(new Rect(0, 0, resWidth, resHeight), 0, 0);
        screenShot.Apply();

        byte[] bytes = screenShot.EncodeToPNG();
        File.WriteAllBytes(name, bytes);

        return name;
    }

    // Update is called once per frame
    void Update()
    {
        

    }
}
