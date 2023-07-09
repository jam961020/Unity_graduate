using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class TextureManager : MonoBehaviour
{
    bool flag = false;
    private Texture2D change_img;
    public RawImage streaming;
    ToggleManager toggleManager;
    // Start is called before the first frame update

    private void SystemIOFileLoad()
    {
        int counter = toggleManager.framecounter_return();

        var path = @"C:\Users\user\UnityGraduate\PythonStream\test" + counter.ToString() + ".png";
        byte[] byteTexture = File.ReadAllBytes(path);
        if (byteTexture.Length > 0)
        {
            Texture2D texture = new Texture2D(0, 0);
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
