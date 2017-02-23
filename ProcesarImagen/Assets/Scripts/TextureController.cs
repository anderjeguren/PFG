using UnityEngine;


public class TextureController : MonoBehaviour
{
    /// <summary>
    /// The webcam texture.
    /// </summary>
    private WebCamTexture webCamTexture;

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
            PluginBehaviour.ExecuteBlur(webCamTexture);
        }
    }
}