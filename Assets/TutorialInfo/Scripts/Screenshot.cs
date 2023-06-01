using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class ScreenShot : MonoBehaviour
{
    public Camera camera;       //보여지는 카메라.

    private int resWidth;
    private int resHeight;
    string path;

    // Use this for initialization
    void Start()
    {
        resWidth = Screen.width;
        resHeight = Screen.height;
        path = Application.dataPath + "/ScreenShot/";
        Debug.Log(path);

    }

    public void ClickScreenShot()
    {
        DirectoryInfo dir = new DirectoryInfo(path);
        if (!dir.Exists)
        {
            Directory.CreateDirectory(path);
        }
        string name;
        if (camera.name == "Camera_NE")
        {
            name = path + System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss-NE") + ".png";
        } else if (camera.name == "Camera_NW")
        {
            name = path + System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss-NW") + ".png";
        } else if (camera.name == "Camera_SE")
        {
            name = path + System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss-SE") + ".png";
        } else
        {
            name = path + System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss-SW") + ".png";
        }
        
        RenderTexture rt = new RenderTexture(resWidth, resHeight, 24);
        camera.targetTexture = rt;
        Texture2D screenShot = new Texture2D(resWidth, resHeight, TextureFormat.RGB24, false);
        Rect rec = new Rect(0, 0, screenShot.width, screenShot.height);
        camera.Render();
        RenderTexture.active = rt;
        screenShot.ReadPixels(new Rect(0, 0, resWidth, resHeight), 0, 0);
        screenShot.Apply();

        byte[] bytes = screenShot.EncodeToPNG();
        File.WriteAllBytes(name, bytes);
    }
}