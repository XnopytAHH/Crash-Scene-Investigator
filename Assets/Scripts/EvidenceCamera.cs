using UnityEngine;

public class EvidenceCamera : MonoBehaviour
{
    public Camera captureCamera; // The camera to capture from
    public RenderTexture renderTexture; // Assign your RenderTexture in Inspector

    public Texture2D CaptureView()
    {
        // Activate the RenderTexture and read from it
        RenderTexture.active = renderTexture;
        Texture2D texture = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.RGB24, false);
        texture.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
        texture.Apply();

        // Clean up
        RenderTexture.active = null;
        captureCamera.targetTexture = null;

        return texture;
    }
}