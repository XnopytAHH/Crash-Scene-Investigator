using UnityEngine;

public class EvidenceBehaviour : MonoBehaviour
{
    /// <summary>
    /// Audio clip to play when the collectible is collected.
    /// </summary>
    [SerializeField]
    AudioClip collectibleAudioClip; // Audio clip for the collectible sound
    public void Collect(PlayerBehavior player)
    {

        //player.collectedSomething(this); // Call the player's method to modify the score
        AudioSource.PlayClipAtPoint(collectibleAudioClip, transform.position, 1f); // Play the collectible sound
        EvidenceCamera captureCamera = GameObject.FindWithTag("MainCamera").GetComponent<EvidenceCamera>(); // Find the EvidenceCamera in the scene
        player.modifyInventory(true, gameObject.name, captureCamera.CaptureView()); // Add the collectible to the player's inventory
        Destroy(gameObject); // Destroy the coin object
    }
}
