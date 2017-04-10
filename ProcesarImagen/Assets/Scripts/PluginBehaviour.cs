using System;
using System.Runtime.InteropServices;
using UnityEngine;

internal class PluginBehaviour : MonoBehaviour {

    struct ImageData
    {
        IntPtr cvMat;
    }

    [DllImport("NativeDll")]
    static extern void testFunction();

    [DllImport("NativeDll")]
    static extern bool openVideoCapture(int value);

    [DllImport("NativeDll")]
    static extern ImageData cvtColor(ref ImageData ptr);

    [DllImport("NativeDll")]
    static extern ImageData createFromExternal(byte[] buffer, int rows, int cols);

    [DllImport("NativeDll")]
    static extern ImageData captureFrame();

    [DllImport("NativeDll")]
    static extern void gaussianBlur(ref ImageData ptr);

    [DllImport("NativeDll")]
    static extern void canny(ref ImageData ptr);

    [DllImport("NativeDll")]
    static extern bool imageShow(ref ImageData ptr, String caption);

    [DllImport("NativeDll")]
    static extern void releaseImage(ref ImageData ptr);

    /// <summary>
    /// The webcam texture.
    /// </summary>
    private static byte[] buffer;

    // Use this for initialization
    static void Initialize(WebCamTexture webCamTexture)
    {
        buffer = new byte[webCamTexture.height * webCamTexture.width * 3];
    }

    static void WebCamToByteArray(WebCamTexture webCamTexture, byte[] data)
    {
        Color32[] colors = webCamTexture.GetPixels32();
        for (int i = 0; i < colors.Length; i++)
        {
            data[(i * 3)] = colors[i].r;
            data[(i * 3) + 1] = colors[i].g;
            data[(i * 3) + 2] = colors[i].b;
        }
    }

    internal static void ExecuteBlur(WebCamTexture webCamTexture)
    {
        Initialize(webCamTexture);
        WebCamToByteArray(webCamTexture, buffer);
        var img = createFromExternal(buffer, webCamTexture.height, webCamTexture.width);
        var gray = cvtColor(ref img);
        gaussianBlur(ref gray);
        canny(ref gray);
        if (!imageShow(ref gray, "Blur window"))
        {
            releaseImage(ref gray);
            releaseImage(ref img);
        }
        releaseImage(ref gray);
        releaseImage(ref img);
    }
}
