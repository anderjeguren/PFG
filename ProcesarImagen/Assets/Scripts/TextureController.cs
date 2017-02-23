using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;


public class TextureController : MonoBehaviour
{
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
    private WebCamTexture webCamTexture;

    /// <summary>
    /// Buffer used to transfer info to the plugin
    /// </summary>
    private byte[] data;

    // Use this for initialization
    void Initialize()
    {
        data = new byte[webCamTexture.height * webCamTexture.width * 3];
    }

    // Use this for initialization
    void Start()
    {
        webCamTexture = new WebCamTexture();
        GetComponent<Renderer>().material.mainTexture = webCamTexture;
    }

    // Update is called once per frame
    void Update()
    {
        if (!webCamTexture.isPlaying && Input.GetKey(KeyCode.Space))
            webCamTexture.Play();

        else if (webCamTexture.isPlaying && Input.GetKey(KeyCode.Space))
            webCamTexture.Stop();

        if (webCamTexture.isPlaying)
        {
            //First version blur image
            ExecuteBlur(webCamTexture);
        }
    }


    void WebCamToByteArray(WebCamTexture webCamTexture, byte[] buffer)
    {
        Color32[] colors = webCamTexture.GetPixels32();
        for (int i = 0; i < colors.Length; i++)
        {
            buffer[(i * 3)] = colors[i].r;
            buffer[(i * 3) + 1] = colors[i].g;
            buffer[(i * 3) + 2] = colors[i].b;
        }
    }

    void ExecuteBlur(WebCamTexture webCamTexture)
    {
        Initialize();
        WebCamToByteArray(webCamTexture, data);
        var img = createFromExternal(data, webCamTexture.height, webCamTexture.width);
        var gray = cvtColor(ref img);
        gaussianBlur(ref gray);
        canny(ref gray);
        if (!imageShow(ref gray, "Hello world!"))
        {
            releaseImage(ref gray);
            releaseImage(ref img);
        }
        releaseImage(ref gray);
        releaseImage(ref img);
    }

}