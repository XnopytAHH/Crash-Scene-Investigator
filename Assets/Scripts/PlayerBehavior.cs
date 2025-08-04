using System.Collections;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using StarterAssets;
using Unity.VisualScripting;
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
    /// <summary>
    /// currentCollectible is a reference to the current collectible item the player is looking at or interacting with in the game.
    /// </summary>
    private GameObject currentCollectible; // Reference to the current collectible item the player is interacting with
    /// <summary>
    /// playerInventory is a reference to the player's inventory, which is used to manage collected items.
    /// </summary>
    public List<Evidence> playerInventory;
    /// <summary>
    /// caseFile is a gameObject that represents the player's case file, which can be used to store information about the player's progress or collected items.
    /// </summary>
    [SerializeField]
    GameObject caseFile; // Reference to the player's case file
    /// <summary>
    /// starterAssets is a reference to the StarterAssetsInputs component, which is used to handle player input.
    /// </summary>
    private StarterAssetsInputs starterAssets; // Reference to the StarterAssetsInputs component for player input handling
    void Start()
    {
        playerInventory = new List<Evidence>();
        starterAssets = GetComponent<StarterAssetsInputs>(); // Get the StarterAssetsInputs component
        caseFile.SetActive(false); // Ensure the case file is initially hidden
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
            else if (hitInfo.collider.CompareTag("BackgroundNPC"))
            {

                currentNPC = hitInfo.collider.gameObject; // Set the current background NPC to the hit object
                canInteract = true; // Allow interaction with the background NPC
            }
            else if (hitInfo.collider.CompareTag("Collectible"))
            {
                currentCollectible = hitInfo.collider.gameObject; // Set the current collectible to the hit object
                currentCollectible.GetComponent<CollectibleBehavior>().Highlight(); // Highlight the collectible
                canInteract = true; // Allow interaction with the collectible
            }
            else
            {
                if (currentCollectible != null)
                {
                    // If the collectible is not the one currently hit, unhighlight it
                    currentCollectible.GetComponent<CollectibleBehavior>().Unhighlight();
                }
                currentNPC = null; // Reset currentNPC if not interacting with an NPC
                currentCollectible = null; // Reset currentCollectible if no hit detected
            }
        }
        else
        {
            if (currentCollectible != null)
            {
                // If the collectible is not the one currently hit, unhighlight it
                currentCollectible.GetComponent<CollectibleBehavior>().Unhighlight();
            }
            canInteract = false; // Disable interaction if no object is hit
            currentNPC = null; // Reset currentNPC if no hit detected
            currentCollectible = null; // Reset currentCollectible if no hit detected
        }
    }
    void OnInteract()
    {
        if (!isBusy)
        {
            // Check if the player can interact with objects

            if (canInteract)
            {
                Debug.Log("Player can interact with an object.");
                // Check if the player has detected a coin or a door
                if (currentNPC != null)
                {
                    if (currentNPC.CompareTag("BackgroundNPC"))
                    {
                        // If the NPC is a background NPC, show dialogue
                        Debug.Log("Interacting with Background NPC: " + currentNPC.name);
                        StartCoroutine(currentNPC.GetComponent<PedestrianBehaviour>().ShowDialogue());
                    }
                    else
                    {
                        // If the NPC is an interactive NPC, start dialogue
                        Debug.Log("Interacting with NPC: " + currentNPC.name);
                        Dialogue currentDialogueLines = currentNPC.GetComponent<NPCBehavior>().getNPCLines();
                        isBusy = true; // Set the player as busy to prevent further interactions
                        StartCoroutine(GameManager.Instance.NPCDialogue(currentNPC, currentDialogueLines));
                    }
                }
                else if (currentCollectible != null)
                {
                    Debug.Log("Collecting collectible: " + currentCollectible.name);
                    // Collect the collectible and modify the player's score
                    currentCollectible.GetComponent<CollectibleBehavior>().Collect(this);
                    currentCollectible = null; // Reset currentCollectible after collection
                }

            }
        }
    }
    public void modifyInventory(bool add, string item, Texture2D itemImage)
    {
        // This method can be used to modify the player's inventory based on the add parameter
        if (add)
        {
            // Logic to add an item to the inventory
            Debug.Log("Item added to inventory.");
            playerInventory.Add(new Evidence { evidenceName = item, evidenceImage = itemImage });
            CaseFile caseFile = GameObject.FindWithTag("CaseFileUI").GetComponent<CaseFile>(); // Find the CaseFile component in the scene
            caseFile.AddEvidence(new Evidence { evidenceName = item, evidenceImage = itemImage }); // Add the evidence to the case file
        }
        else
        {
            // Logic to remove an item from the inventory
            Debug.Log("Item removed from inventory.");
            playerInventory.Remove(playerInventory.FirstOrDefault(e => e.evidenceName == item));

        }
    }
    void OnPause()
    {
        if (GameManager.Instance.isPaused)
        {
            GameManager.Instance.resumeGame(); // Resume the game if it is paused
        }
        else
        {

            GameManager.Instance.pauseGame(); // Pause the game if it is not paused
        }
    }
    void OnOpenInventory()
    {
        caseFile.SetActive(!caseFile.activeSelf); // Toggle the visibility of the case file
        if (caseFile.activeSelf)
        {
            Cursor.lockState = CursorLockMode.None; // Unlock the cursor when the case file is open
            starterAssets.cursorInputForLook = false; // Disable mouse look input when the case file is open
            GameManager.Instance.openCaseFile(); // Call the method to open the case file
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked; // Lock the cursor when the case file is closed
            starterAssets.cursorInputForLook = true; // Re-enable mouse look input when the case file is closed
            GameManager.Instance.closeCaseFile(); // Call the method to close the case file
        }

    }
}
