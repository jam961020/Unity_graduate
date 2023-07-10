using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;

public class TextureManager : MonoBehaviour
{
    bool flag = false;
    private Texture2D change_img;
    private Texture2D texture;
    public RawImage streaming;
    ToggleManager toggleManager;
    // Start is called before the first frame update

    private void SystemIOFileLoad()
    {
        int counter = toggleManager.framecounter_return()-2;

        var path = @"C:\Users\" +Environment.UserName + @"\UnityGraduate\PythonStream\test" + counter.ToString() + ".png";
        byte[] byteTexture = File.ReadAllBytes(path);                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                      
        if (byteTexture.Length > 0)
        {
            change_img = texture;
            if (change_img != null) Texture2D.Destroy(change_img);
            texture = new Texture2D(0, 0);
            texture.LoadImage(byteTexture);
            streaming.texture = texture;
        }
    }

    void Start()
    {
        toggleManager = GetComponent<ToggleManager>();
    }

    // Update is called once per frame
    void Update()
    {
        SystemIOFileLoad();
    }
}
