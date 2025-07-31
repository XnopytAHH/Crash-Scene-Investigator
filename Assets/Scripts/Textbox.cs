using UnityEngine;
using UnityEngine.SceneManagement;
public class Textbox : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void textboxCreated()
    {
        SceneManager.activeSceneChanged += OnSceneChanged;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnSceneChanged(Scene current, Scene next)
    {
        SceneManager.activeSceneChanged -= OnSceneChanged; // Unsubscribe from the event to prevent memory leaks
        Debug.Log("Scene changed from " + current.name + " to " + next.name);
        // Optionally, you can perform any cleanup or state reset here
       Destroy(gameObject); // Destroy the textbox when the scene changes
    }
}
