/*
* Author: Lim En Xu Jayson
* Date: 16/8/2025
* Description: Handles the behavior of collectible evidence in the game. Handles the collection and interaction with evidence items.
*/
using UnityEngine;

public class EvidenceBehaviour : MonoBehaviour
{
    /// <summary>
    /// Audio clip to play when the collectible is collected.
    /// </summary>
    [SerializeField]
    AudioClip collectibleAudioClip; 
    /// <summary>
    /// The name of the collectible item.
    /// </summary>
    [SerializeField]
    string collectibleName; 
    /// <summary>
    /// The description of the collectible item.
    /// </summary>
    [SerializeField]
    string collectibleDesc; 
    /// <summary>
    /// destroyOnPickup dictates whether the object should be destroyed upon collection.
    /// </summary>
    [SerializeField]
    bool destroyOnPickup;
    /// <summary>
    /// isCollected indicates whether the collectible has already been collected.
    /// </summary>
    bool isCollected = false; 
    /// <summary>
    /// Collects the evidence item.
    /// </summary>
    public void Collect(PlayerBehavior player)
    {
        if (isCollected) return; // If already collected, do nothing
        isCollected = true; // Mark as collected

        
        AudioSource.PlayClipAtPoint(collectibleAudioClip, transform.position, 1f); // Play the collectible sound
        EvidenceCamera captureCamera = GameObject.FindWithTag("MainCamera").GetComponent<EvidenceCamera>(); // Find the EvidenceCamera in the scene
        player.modifyInventory(true, collectibleName, collectibleDesc, captureCamera.CaptureView()); // Add the collectible to the player's inventory
        if (destroyOnPickup)
        {
            Destroy(gameObject); // Destroy the coin object
        }
    }
}
