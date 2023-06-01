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
        // FFmpeg 프로세스 생성 및 설정
        ffmpegProcess = new Process();
        ffmpegProcess.StartInfo.FileName = "C:\\Users\\user\\Desktop\\졸과\\ffmpeg-6.0-essentials_build\\bin\\ffmpeg.exe";
        ffmpegProcess.StartInfo.Arguments = "-y -f rawvideo -pixel_format rgb24 -video_size " +
            Screen.width + "x" + Screen.height + " -framerate 30 -i - -c:v libx264 -preset ultrafast " +
            "-qp 0 -movflags +faststart " + outputFilePath;
        ffmpegProcess.StartInfo.RedirectStandardInput = true;
        ffmpegProcess.StartInfo.UseShellExecute = false;
        ffmpegProcess.StartInfo.CreateNoWindow = true;

        // FFmpeg 프로세스 시작
        ffmpegProcess.Start();
    }

    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        // 렌더링된 화면을 비디오 프레임으로 전송
        RenderTexture.active = source;
        Texture2D frameTexture = new Texture2D(source.width, source.height, TextureFormat.RGB24, false);
        frameTexture.ReadPixels(new Rect(0, 0, source.width, source.height), 0, 0);
        frameTexture.Apply();

        // 텍스처 데이터를 바이트 배열로 변환하여 FFmpeg에 전달
        byte[] frameData = frameTexture.GetRawTextureData();
        ffmpegProcess.StandardInput.BaseStream.Write(frameData, 0, frameData.Length);

        // Texture2D 객체를 메모리에서 해제
        UnityEngine.Object.Destroy(frameTexture);

        // 화면에 렌더링
        Graphics.Blit(source, destination);
    }

    void OnApplicationQuit()
    {
        // FFmpeg 프로세스 종료
        if (!ffmpegProcess.HasExited)
        {
            ffmpegProcess.StandardInput.BaseStream.Close();
            ffmpegProcess.StandardInput.Close();
            ffmpegProcess.WaitForExit();
        }
        ffmpegProcess.Close();
    }
}
