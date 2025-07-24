using System.Collections;
using UnityEngine;
/// <summary>
/// PlayerBehavior is a MonoBehaviour that handles player interactions with NPCs and objects in the game.
/// </summary>
public class PlayerBehavior : MonoBehaviour
{
    /// <summary>
    /// canInteract is a boolean flag that determines if the player can currently interact with objects in the game.
    /// </summary>
    private bool canInteract = false; // Flag to check if the player can interact with objects
    /// <summary>
    /// currentNPC is a reference to the current NPC the player is looking at or interacting with in the game.
    /// </summary>
    private GameObject currentNPC; // Reference to the current NPC the player is interacting with
    /// <summary>
    /// spawnPoint is a Transform that represents the point from which the player will spawn the raycast to interact with objects in the game.
    /// </summary>
    [SerializeField]
    private Transform spawnPoint; // The point from which the player will interact with objects
    /// <summary>
    /// interactDistance is a float that defines the maximum distance within which the player can interact with objects.
    /// </summary>
    [SerializeField]
    private float interactDistance = 3f; // Distance within which the player can interact with objects
    /// <summary>
    /// isBusy is a boolean flag that indicates whether the player is currently busy and cannot interact with objects.
    /// </summary>
    public bool isBusy = false; // Flag to check if the player is busy and cannot interact with objects
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hitInfo;
        if (Physics.Raycast(spawnPoint.position, spawnPoint.forward, out hitInfo, interactDistance))
        {

            if (hitInfo.collider.CompareTag("NPC"))
            {
                currentNPC = hitInfo.collider.gameObject; // Set the current NPC to the hit object
                canInteract = true; // Allow interaction with the NPC
            }
            else
            {
                currentNPC = null; // Reset currentNPC if not interacting with an NPC
            }
        }
        else
        {
            canInteract = false; // Disable interaction if no object is hit
            currentNPC = null; // Reset currentNPC if no hit detected
        }
    }
    void OnInteract()
    {
        if (!isBusy)
        {
            // Check if the player can interact with objects
            if (canInteract)
            {
                // Check if the player has detected a coin or a door
                if (currentNPC != null)
                {
                    Debug.Log("Interacting with NPC: " + currentNPC.name);
                    Dialogue currentDialogueLines = currentNPC.GetComponent<NPCBehavior>().getNPCLines();
                    isBusy = true; // Set the player as busy to prevent further interactions
                    StartCoroutine(GameManager.Instance.NPCDialogue(currentNPC, currentDialogueLines));
                }

            }
        }
    }
    
}
