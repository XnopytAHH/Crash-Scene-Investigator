using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem.Controls;
using StarterAssets;
using UnityEngine.UI;
using TMPro;
using Unity.Cinemachine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private float dialogueSpeed = 0.05f; // Speed of dialogue text appearing
    public static GameManager Instance;
    [SerializeField]
    private AudioSource audioSource;
    /// <summary>
    /// currentLevel is an integer that represents the current level of the game.
    /// </summary>
    public int currentLevel = 0;
    /// <summary>
    /// pauseMenu is the canvas for the pause menu.
    /// </summary>
    [SerializeField]
    public Canvas pauseMenu;
    /// <summary>
    /// mainMenu is the canvas for the main menu.
    /// </summary>
    [SerializeField]
    public Canvas mainMenu;
    /// <summary>
    /// player is a reference to the player GameObject.
    /// </summary>
    private GameObject player;
    /// <summary>
    /// currentAudio is an AudioClip that holds the audio for the current dialogue line.
    /// </summary>
    private AudioClip currentAudio;
    /// <summary>
    /// caseFileCanvas is a reference to the case file canvas.
    /// </summary>
    private Canvas caseFileCanvas;
    /// <summary>
    /// isPaused is a boolean that indicates whether the game is currently paused.
    /// </summary>
    public bool isPaused = false; // Flag to check if the game is paused
    void Awake()
    {
        OnSceneLoaded(SceneManager.GetActiveScene(), LoadSceneMode.Single); // Initialize the pause menu when the scene is loaded
        Canvas dialogueUI = GameObject.FindWithTag("UI Dialogue").GetComponent<Canvas>();
        caseFileCanvas = GameObject.FindWithTag("CaseFileUI").GetComponent<Canvas>();
        caseFileCanvas.enabled = false; // Hide the case file canvas at the start
        audioSource = gameObject.GetComponent<AudioSource>();
        dialogueUI.enabled = false;
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
    }
    void Update()
    {
        SceneManager.sceneLoaded += OnSceneLoaded; // Subscribe to the scene loaded event

        if (SceneManager.GetActiveScene().name != "MainMenu")
        {
            mainMenu.enabled = false; // Disable the main menu if the game has started
        }
        else
        {
            mainMenu.enabled = true; // Enable the main menu if the game has not started
        }
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        pauseMenu.enabled = false; // Disable the pause menu at the start
        player = GameObject.FindWithTag("Player");
        //loop through all toggles and set them to false
        Toggle[] toggles = GameObject.FindObjectsByType<Toggle>(FindObjectsSortMode.None);
        foreach (Toggle toggle in toggles)
        {
            toggle.isOn = false; // Set all toggles to false
        }
    }
    public void StartGame()
    {
        Debug.Log("Game Started");
        currentLevel = 1; // Set the current level to 1 when the game starts
        UnityEngine.SceneManagement.SceneManager.LoadScene("office"); // Load the first level
        Time.timeScale = 1; // Ensure the game is running at normal speed
        Cursor.lockState = CursorLockMode.Locked; // Lock the cursor to the game window
    }
    public void ExitGame()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // Stop playing in the editor
#endif
    }
    public IEnumerator NPCDialogue(GameObject npc, Dialogue dialogueLines)
    {
        GameObject player = GameObject.FindWithTag("Player");
        Canvas dialogueUI = GameObject.FindWithTag("UI Dialogue").GetComponent<Canvas>();
        dialogueUI.transform.GetChild(1).transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = npc.name;
        player.GetComponent<FirstPersonController>().enabled = false;
        dialogueUI.enabled = true; // Show the dialogue UI
        foreach (DialogueLine line in dialogueLines.dialogueLines)
        {
            bool linePrinting = true; // Flag to check if the player is typing
            yield return stepThruDialogue(dialogueUI.transform.GetChild(0).transform.GetChild(0).GetComponent<TextMeshProUGUI>(), dialogueSpeed, line);
            linePrinting = false; // Set the flag to false after the line is printed

            yield return new WaitUntil(() => !linePrinting && (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)));
            yield return new WaitForSeconds(0.1f); // Small delay to prevent immediate re-triggering


        }
        player.GetComponent<PlayerBehavior>().isBusy = false; // Set the player as not busy after dialogue
        player.GetComponent<FirstPersonController>().enabled = true;
        dialogueUI.enabled = false; // Hide the dialogue UI
    }
    private IEnumerator stepThruDialogue(TextMeshProUGUI textbox, float delay, DialogueLine line)
    {
        textbox.text = ""; // Clear the textbox before starting
        foreach (char letter in line.Line.ToCharArray())
        {
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
            {
                textbox.text = line.Line; // If space is pressed, show the full text immediately
                Debug.Log("Dialogue skipped");
                yield return new WaitForEndOfFrame(); // Wait for the end of the frame to ensure the text is fully displayed
                yield break; // Exit the coroutine

            }
            if (letter != ' ') // If the letter is a space, skip the delay
            {
                audioSource.pitch = Random.Range(0.97f, 1.03f); // Randomize the pitch for variation
                audioSource.PlayOneShot(line.AudioClip); // Play the audio clip for the letter
            }

            textbox.text += letter; // Add one letter at a time

            yield return new WaitForSeconds(delay); // Wait for the specified delay
        }
    }
    public void pauseGame()
    {
        Time.timeScale = 0; // Pause the game
        Cursor.lockState = CursorLockMode.None; // Unlock the cursor
        pauseMenu.enabled = true; // Enable the pause menu
        player.GetComponent<FirstPersonController>().enabled = false; // Disable the character controller to prevent movement
        isPaused = true; // Set the paused state to true

    }
    public void resumeGame()
    {
        Cursor.lockState = CursorLockMode.Locked; // Lock the cursor again
        pauseMenu.enabled = false; // Disable the pause menu
        Time.timeScale = 1; // Resume the game
        if (!player.GetComponent<PlayerBehavior>().isBusy)
        {
            player.GetComponent<FirstPersonController>().enabled = true; // Enable the character controller
        }
        isPaused = false; // Set the paused state to false
    }
    public void returnToMainMenu()
    {
        Time.timeScale = 1; // Ensure the game is running at normal speed
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu"); // Load the main menu scene
        currentLevel = 0; // Reset the current level
        Cursor.lockState = CursorLockMode.None; // Unlock the cursor
    }
    public void openCaseFile()
    {
        caseFileCanvas.enabled = true; // Show the case file canvas
    }
    public void closeCaseFile()
    {
        caseFileCanvas.enabled = false; // Hide the case file canvas
    }
}
