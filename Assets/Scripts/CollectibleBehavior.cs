/*
* Author: Lim En Xu Jayson
* Date: 9/6/2025
* Description: Handles the behavior of collectible items in the game.
*/
using UnityEngine;

public class CollectibleBehavior : MonoBehaviour
{

    /// <summary>
    /// The MeshRenderer component for the collectible object.
    /// Used to change the color when highlighted or collected.
    /// </summary>

    public Vector3 originalPos;

    /// <summary>
    /// Highlights the collectible by changing its color.
    /// </summary>
    void Start()
    {
        originalPos = transform.position;
    }
    public void Highlight()
    {
        gameObject.GetComponent<Outline>().enabled = true; // Enable the outline component
        gameObject.GetComponent<Outline>().OutlineColor = Color.green; // Set the outline color to green
    }
    /// <summary>
    /// Unhighlights the collectible by resetting its color to the original.
    /// </summary>
    public void Unhighlight()
    {
        
        gameObject.GetComponent<Outline>().OutlineColor = Color.yellow; // Set the outline color to yellow
    }
    /// <summary>
    /// Collects the collectible, modifies the player's score, plays a sound, and destroys the collectible object.
    ///</summary>
    
}

