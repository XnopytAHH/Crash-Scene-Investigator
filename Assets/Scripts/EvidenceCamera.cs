/*
* Author: Lim En Xu Jayson
* Date: 16/8/2025
* Description: Handles the render texture for evidence capturing
*/
using UnityEngine;

public class EvidenceCamera : MonoBehaviour
{
    /// <summary>
    /// The camera used for capturing evidence.
    /// </summary>
    public Camera captureCamera; 
    /// <summary>
    /// The render texture used to capture evidence
    /// </summary>
    public RenderTexture renderTexture; 
    /// <summary>
    /// Captures the current view of the evidence camera.
    /// </summary>

    public Texture2D CaptureView()
    {
        // Activate the RenderTexture and read from it
        RenderTexture.active = renderTexture;
        Texture2D texture = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.RGB24, false);
        texture.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
        texture.Apply();
        RenderTexture.active = null;
        captureCamera.targetTexture = null;

        return texture;
    }
}