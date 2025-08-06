using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem.Controls;
using StarterAssets;
using UnityEngine.UI;
using TMPro;
using Unity.Cinemachine;
using UnityEngine.SceneManagement;
using UnityEngine.VFX;

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
    public bool isPaused = false;
    /// <summary>
    /// backgroundAnimator is a reference to the background animator.
    /// </summary>
    private Animator backgroundAnimator;
    /// <summary>
    /// backgroundUI is a reference to the background UI GameObject.
    /// </summary>
    [SerializeField]
    private GameObject backgroundUI;
    /// <summary>
    /// DayCounter is a textmeshproUGUI that displays the current day in the game.
    /// </summary>

    private TextMeshProUGUI dayCounter;
    /// <summary>
    /// startingNewDay is a boolean that indicates whether the game is starting a new day.
    /// </summary>
    public bool startingNewDay = false;
    /// <summary>
    /// bigBossAnimator is a reference to the animator for the big boss NPC.
    /// </summary>
    private Animator bigBossAnimator;
    /// <summary>
    /// deathSpawnParticles is a reference to the visual effect for the death spawn particles.
    /// </summary>
    private VisualEffect deathSpawnParticles;
    /// <summary>
    /// bigBoss is a reference to the big boss NPC behavior script.
    /// </summary>
    private NPCBehavior bigBoss;
    void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded; // Subscribe to the scene loaded event
    }
    void Awake()
    {
        
        backgroundUI = GameObject.FindWithTag("BackgroundUI");
        dayCounter = GameObject.Find("DayCounter").GetComponent<TextMeshProUGUI>();
        dayCounter.enabled = false; // Disable the day counter at the start
        backgroundAnimator = backgroundUI.GetComponent<Animator>();
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
        

        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            mainMenu.enabled = true; // Enable the main menu if the game has not started
        }
        else
        {
            mainMenu.enabled = false; // Disable the main menu if the game has started
        }
        if (player != null)
        {
            if (player.GetComponent<PlayerBehavior>().isBusy)
            {
                Cursor.lockState = CursorLockMode.None; // Unlock the cursor if the player is busy
                player.GetComponent<FirstPersonController>().enabled = false; // Disable the character controller to prevent movement
            }
            else if (isPaused)
            {
                Cursor.lockState = CursorLockMode.None; // Unlock the cursor if the game is paused
                player.GetComponent<FirstPersonController>().enabled = false; // Disable the character controller to prevent movement
            }
            else if (!player.GetComponent<PlayerBehavior>().isBusy)
            {
                Cursor.lockState = CursorLockMode.Locked; // Lock the cursor if the player is not busy
                player.GetComponent<FirstPersonController>().enabled = true; // Enable the character controller
            }

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
        if (SceneManager.GetActiveScene().name == "office")
        {
            deathSpawnParticles = GameObject.FindWithTag("NPC").GetComponentInChildren<VisualEffect>(false);
            deathSpawnParticles.Stop(); // Stop the death spawn particles if they are playing
            backgroundAnimator.Play("Closed", 0, 0f); // Play the fade-in animation immediately
            backgroundAnimator.SetBool("isOpen", false); // Ensure the background is closed at the start
            if (startingNewDay)
            {
                StartCoroutine(StartDayCoroutine()); // Start the day if a new day is starting
                Debug.Log("Starting a new day, background animator is set to closed");
            }
            else
            {
                Debug.Log("Not starting a new day, background animator is set to open");
                backgroundAnimator.SetBool("isOpen", true); // Ensure the background is open when in the office scene
            }
        }
    }
    public void StartGame()
    {
        
        Debug.Log("Game Started");
        startingNewDay = true; // Set the flag to indicate a new day is starting
        Cursor.lockState = CursorLockMode.Locked; // Lock the cursor at the start
        Debug.Log("Starting new day: " + currentLevel);
        StartCoroutine(StartGameCoroutine());
    }
    IEnumerator StartGameCoroutine()
    {
        Debug.Log("Game Started 1");
        Time.timeScale = 1; // Ensure the game is running at normal speed
        backgroundAnimator.SetBool("isOpen", false); // Close the background UI
        yield return new WaitForSeconds(2f); // Wait for 2 seconds before starting the game
        Debug.Log("Game Started 1");
        currentLevel = 1; // Set the current level to 1 when the game starts
        UnityEngine.SceneManagement.SceneManager.LoadScene("office"); // Load the first level

        Cursor.lockState = CursorLockMode.Locked; // Lock the cursor to the game window
        yield return null; // Wait for the end of the frame to ensure everything is set up correctly
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
        
        isPaused = false; // Set the paused state to false
    }
    public void returnToMainMenu()
    {

        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu"); // Load the main menu scene
        currentLevel = 0; // Reset the current level
        resumeGame(); // Resume the game to ensure the pause menu is closed
        
    }
    public void openCaseFile()
    {
        caseFileCanvas.enabled = true; // Show the case file canvas
    }
    public void closeCaseFile()
    {
        caseFileCanvas.enabled = false; // Hide the case file canvas
    }
    private IEnumerator StartDayCoroutine()
    {
        // UI for new day
        backgroundAnimator.Play("Closed", 0, 0f); // Play the fade-in animation immediately
        backgroundAnimator.SetBool("isOpen", false); // Ensure the background is closed at the start
        dayCounter.enabled = true; // Enable the day counter
        dayCounter.text = "Day " + currentLevel; // Update the day counter text
        bigBoss = GameObject.FindWithTag("NPC").GetComponent<NPCBehavior>(); // Find the big boss NPC
        bigBossAnimator = GameObject.FindWithTag("NPC").GetComponent<Animator>();
        bigBossAnimator.Play("BossStandby", 0, 0f); // Play the standby animation for the big boss NPC
        player.GetComponent<PlayerBehavior>().isBusy = true; // Set the player as busy while starting the day
        dayCounter.CrossFadeAlpha(0.0f, 0f, true); // Fade in the day counter
        
        dayCounter.CrossFadeAlpha(1.0f, 1f, true); // Fade in the day counter
        yield return new WaitForSeconds(2f); // Wait for 2 seconds before fading out
        dayCounter.CrossFadeAlpha(0.0f, 1f, true); // Fade out the day counter
        yield return new WaitForSeconds(2f); // Wait for the fade out to complete
        dayCounter.enabled = false; // Disable the day counter after fading out

        backgroundAnimator.SetBool("isOpen", true);

        yield return new WaitForSeconds(1f); // Wait for the end of the frame to ensure everything is set up correctly



        //Death spawn in and dialogue
        deathSpawnParticles.Play(); // Play the death spawn particles effect

        yield return new WaitForSeconds(2f); // Wait for 2 seconds before starting the dialogue

        bigBossAnimator.Play("BossSpawn", 0, 0f); // Play the spawn in animation for the big boss NPC
        yield return new WaitForSeconds(3f); // Wait for 3 seconds to allow the animation to play
        deathSpawnParticles.Stop(); // Stop the death spawn particles effect

        yield return null; // Wait for the end of the frame to ensure everything is set up correctly
        // Start the dialogue with the big boss NPC
        Dialogue currentDialogueLines = bigBoss.getNPCLines(0); // Get the dialogue lines for the big boss NPC

        Debug.Log("Starting dialogue with big boss: " + bigBoss.gameObject.name);
        StartCoroutine(NPCDialogue(bigBoss.gameObject, currentDialogueLines)); // Start the dialogue coroutine with the big boss NPC
        yield return null; // Wait for the end of the frame to ensure everything is set up correctly
        startingNewDay = false; // Reset the flag after starting the day
    }
}
