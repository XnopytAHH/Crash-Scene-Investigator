using UnityEngine;

public class EvidenceBehaviour : MonoBehaviour
{
    /// <summary>
    /// Audio clip to play when the collectible is collected.
    /// </summary>
    [SerializeField]
    AudioClip collectibleAudioClip; // Audio clip for the collectible sound
    [SerializeField]
    string collectibleName; // Name of the collectible, used for identification
    [SerializeField]
    string collectibleDesc; // Description of the collectible
    [SerializeField]
    bool destroyOnPickup;
    bool isCollected = false; // Flag to check if the collectible has already been collected

    public void Collect(PlayerBehavior player)
    {
        if (isCollected) return; // If already collected, do nothing
        isCollected = true; // Mark as collected

        //player.collectedSomething(this); // Call the player's method to modify the score
        AudioSource.PlayClipAtPoint(collectibleAudioClip, transform.position, 1f); // Play the collectible sound
        EvidenceCamera captureCamera = GameObject.FindWithTag("MainCamera").GetComponent<EvidenceCamera>(); // Find the EvidenceCamera in the scene
        player.modifyInventory(true, collectibleName, collectibleDesc, captureCamera.CaptureView()); // Add the collectible to the player's inventory
        if (destroyOnPickup)
        {
            Destroy(gameObject); // Destroy the coin object
        }
    }
}
