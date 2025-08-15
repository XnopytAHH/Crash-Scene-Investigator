/*
* Author: Lim En Xu Jayson
* Date: 16/8/2025
* Description: Manages the textbox UI element.
*/
using UnityEngine;
using UnityEngine.SceneManagement;
public class Textbox : MonoBehaviour
{
    /// <summary>
    /// Called when the textbox is created.
    /// </summary>
    public void textboxCreated()
    {
        SceneManager.activeSceneChanged += OnSceneChanged;
    }
    /// <summary>
    /// Called when the active scene changes.
    /// </summary>
    private void OnSceneChanged(Scene current, Scene next)
    {
        SceneManager.activeSceneChanged -= OnSceneChanged; // Unsubscribe from the event to prevent memory leaks
        Debug.Log("Scene changed from " + current.name + " to " + next.name);
        // Optionally, you can perform any cleanup or state reset here
        Destroy(gameObject); // Destroy the textbox when the scene changes
    }
}
