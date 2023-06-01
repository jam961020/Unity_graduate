using System.Collections;
using System.Diagnostics;
using UnityEngine;

public class VideoCapture : MonoBehaviour
{
    public string outputFilePath = "C:/Users/user/Desktop/output.mp4";
    private Process ffmpegProcess;

    public bool IsRecording { get; internal set; }

    void Start()
    {
        // FFmpeg ���μ��� ���� �� ����
        ffmpegProcess = new Process();
        ffmpegProcess.StartInfo.FileName = "C:\\Users\\user\\Desktop\\����\\ffmpeg-6.0-essentials_build\\bin\\ffmpeg.exe";
        ffmpegProcess.StartInfo.Arguments = "-y -f rawvideo -pixel_format rgb24 -video_size " +
            Screen.width + "x" + Screen.height + " -framerate 30 -i - -c:v libx264 -preset ultrafast " +
            "-qp 0 -movflags +faststart " + outputFilePath;
        ffmpegProcess.StartInfo.RedirectStandardInput = true;
        ffmpegProcess.StartInfo.UseShellExecute = false;
        ffmpegProcess.StartInfo.CreateNoWindow = true;

        // FFmpeg ���μ��� ����
        ffmpegProcess.Start();
    }

    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        // �������� ȭ���� ���� ���������� ����
        RenderTexture.active = source;
        Texture2D frameTexture = new Texture2D(source.width, source.height, TextureFormat.RGB24, false);
        frameTexture.ReadPixels(new Rect(0, 0, source.width, source.height), 0, 0);
        frameTexture.Apply();

        // �ؽ�ó �����͸� ����Ʈ �迭�� ��ȯ�Ͽ� FFmpeg�� ����
        byte[] frameData = frameTexture.GetRawTextureData();
        ffmpegProcess.StandardInput.BaseStream.Write(frameData, 0, frameData.Length);

        // Texture2D ��ü�� �޸𸮿��� ����
        UnityEngine.Object.Destroy(frameTexture);

        // ȭ�鿡 ������
        Graphics.Blit(source, destination);
    }

    void OnApplicationQuit()
    {
        // FFmpeg ���μ��� ����
        if (!ffmpegProcess.HasExited)
        {
            ffmpegProcess.StandardInput.BaseStream.Close();
            ffmpegProcess.StandardInput.Close();
            ffmpegProcess.WaitForExit();
        }
        ffmpegProcess.Close();
    }
}
