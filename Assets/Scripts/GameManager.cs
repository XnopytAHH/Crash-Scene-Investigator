using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem.Controls;
using StarterAssets;
using UnityEngine.UI;
using TMPro;
using Unity.Cinemachine;
using UnityEngine.SceneManagement;
using UnityEngine.VFX;
using UnityEngine.Rendering.Universal;
using Unity.Mathematics;
using UnityEngine.Rendering;
using Unity.VisualScripting;
using System;

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
    public GameObject player;
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
    private bool startingNewDay = false;
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
    /// <summary>
    /// levelManager is a reference to the LevelManager script that manages the levels in the game.
    /// </summary>
    [SerializeField]
    LevelManager levelManager;
    /// <summary>
    /// caseFile is a reference to the CaseFile script that manages the case file in the game.
    /// </summary>
    [SerializeField]
    CaseFile caseFile;
    /// <summary>
    /// caseFileObject is a reference to the GameObject that contains the case file UI.
    /// </summary>
    public GameObject caseFileObject;
    /// <summary>
    /// timerUI is a reference to the timer UI that displays the time left in the level
    /// </summary>
    [SerializeField]
    private TextMeshProUGUI timerUI;
    /// <summary>
    /// levelTimer is a float that holds the time left in the level.
    /// </summary>
    private float levelTimer = 180f;
    /// <summary>
    /// warningIndicator is a reference to the warning indicator UI that shows when the level is about to end.
    /// </summary>
    [SerializeField]
    Volume warningIndicator;
    /// <summary>
    /// volumeProfile is a reference to the volume profile for the warning indicator.
    /// </summary>
    [SerializeField]
    VolumeProfile volumeProfile;
    /// <summary>
    /// vignetteEffect is a reference to the vignette effect in the volume profile.
    /// </summary>
    private Vignette vignetteEffect;
    /// <summary>
    /// lensDistortionEffect is a reference to the lens distortion effect in the volume profile.
    /// </summary>
    private LensDistortion lensDistortionEffect;
    /// <summary>
    /// indicatorGreen is the color for the warning indicator when there is sufficient time left.
    /// </summary>
    [SerializeField]
    Color indicatorGreen;
    /// <summary>
    /// indicatorYellow is the color for the warning indicator when time is running low.
    /// </summary>
    [SerializeField]
    Color indicatorYellow;
    /// <summary>
    /// indicatorRed is the color for the warning indicator when time is critical.
    /// </summary>
    [SerializeField]
    Color indicatorRed;
    /// <summary>
    /// beenToLevel is a boolean that indicates whether the player has been to the level before.
    /// </summary>
    public bool beenToLevel = false;

    [SerializeField]
    Material newMat;
    /// <summary>
    /// pingRunning is a boolean that indicates whether the ping effect is currently running.
    /// </summary>
    public bool pingRunning = false;
    /// <summary>
    /// crosshair is a reference to the crosshair UI element.
    /// </summary>
    [SerializeField]
    Image crosshair;
    /// <summary>
    /// currentCause is a reference to the current Toggle that indicates the cause of accident
    /// </summary>
    Toggle currentCause;
    [SerializeField]
    TMP_Dropdown culpritDropdown;
    private bool inCutscene = false; // Flag to indicate if the game is in a cutscene state
    [SerializeField]
    GameObject explosionPrefab;
    float timer;

    ///<summary>
    /// endScreen is a reference to the canvas that displays the end screen UI.
    /// </summary>
    [SerializeField]
    Canvas endScreen;
    /// <summary>
    /// correctText is a reference to the TextMeshProUGUI element that displays the correct answer text.
    /// </summary>
    [SerializeField]
    TextMeshProUGUI correctText;
    /// <summary>
    /// caseNameUI is a reference to the TextMeshProUGUI element that displays the case name text.
    /// </summary>
    [SerializeField]
    TextMeshProUGUI caseNameUI;
    /// <summary>
    /// culpritUI is a reference to the TextMeshProUGUI element that displays the culprit name text.
    /// </summary>
    [SerializeField]
    TextMeshProUGUI culpritUI;
    /// <summary>
    /// culpritScoreUI is a reference to the TextMeshProUGUI element that displays the culprit score text.
    /// </summary>
    [SerializeField]
    TextMeshProUGUI culpritScoreUI;
    /// <summary>
    /// causeUI is a reference to the TextMeshProUGUI element that displays the cause text.
    /// </summary>
    [SerializeField]
    TextMeshProUGUI causeUI;
    /// <summary>
    /// causeScoreUI is a reference to the TextMeshProUGUI element that displays the cause score text.
    /// </summary>
    [SerializeField]
    TextMeshProUGUI causeScoreUI;
    /// <summary>
    /// cluesUI is a reference to the TextMeshProUGUI element that displays the clues collected
    /// </summary>
    [SerializeField]
    TextMeshProUGUI cluesUI;
    /// <summary>
    /// cluesScoreUI is a reference to the TextMeshProUGUI element that displays the clues score
    /// </summary>
    [SerializeField]
    TextMeshProUGUI cluesScoreUI;
    /// <summary>
    /// timerendUI is a reference to the TextMeshProUGUI element that displays the timer
    /// </summary>
    [SerializeField]
    TextMeshProUGUI timerendUI;
    ///<summary>
    /// timerendScoreUI is a reference to the TextMeshProUGUI element that displays the timer score
    /// </summary>
    [SerializeField]
    TextMeshProUGUI timerendScoreUI;
    /// <summary>
    /// totalscoreUI is a reference to the TextMeshProUGUI element that displays the total score
    /// </summary>
    [SerializeField]
    TextMeshProUGUI totalscoreUI;
    /// <summary>
    /// correctCause is a bool that indicates whether the selected cause is correct.
    /// </summary>
    bool correctCause;
    /// <summary>
    /// correctCulprit is a bool that indicates whether the selected culprit is correct.
    /// </summary>
    bool correctCulprit;

    /// <summary>
    /// Start is called before the first frame update
    /// </summary>
    void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
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
        endScreen.enabled = false; // Enable the end screen UI
        crosshair.enabled = true; // Enable the crosshair in the office scene
        timerUI.enabled = false; // Disable the timer UI at the start
        pauseMenu.enabled = false; // Disable the pause menu at the start
        player = GameObject.FindWithTag("Player");
        if (player == null)
        {
            Debug.LogError("Player object not found!"); // Log an error if the player object is not found

        }
        else
        { Debug.Log("Player object found: " + player.name); }

        //loop through all toggles and set them to false
        
        
        
        if (SceneManager.GetActiveScene().name == "office")
        {
            caseFileObject = FindObjectsByType<CollectibleBehavior>(FindObjectsInactive.Include, FindObjectsSortMode.None)[0].gameObject; // Find the case file collectible object in the office scene


            if (caseFileObject != null)
            {
                caseFileObject.SetActive(false); // Hide the case file object when transitioning to a new scene
            }

            if (volumeProfile.TryGet(out Vignette vignetteEffect) && volumeProfile.TryGet(out LensDistortion lensDistortionEffect))
            {
                vignetteEffect.color.value = indicatorGreen; // Set the initial vignette color to green
                vignetteEffect.intensity.value = 0f; // Set the vignette intensity to a default value
                lensDistortionEffect.intensity.value = 0f; // Set the lens distortion intensity to a default value
            }
            deathSpawnParticles = GameObject.FindWithTag("NPC").GetComponentInChildren<VisualEffect>(false);
            deathSpawnParticles.Stop(); // Stop the death spawn particles if they are playing

            if (startingNewDay)
            {
                backgroundAnimator.Play("Closed", 0, 0f); // Play the fade-in animation immediately
                backgroundAnimator.SetBool("isOpen", false); // Ensure the background is closed at the start
                StartCoroutine(StartDayCoroutine()); // Start the day if a new day is starting
                Debug.Log("Starting a new day, background animator is set to closed");
            }
            else
            {
                player.GetComponent<PlayerBehavior>().hasFile = true;
                if (caseFileObject != null)
                {

                    caseFileObject.SetActive(false); // Hide the case file object when transitioning to a new scene
                }
                backgroundAnimator.Play("open", 0, 0f); // Play the fade-in animation immediately

                Debug.Log("Not starting a new day, background animator is set to open");
                StartCoroutine(transitionToNewScene()); // Transition to the new scene if not starting a new day

            }
        }
        else if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            
        
            Toggle[] toggles = GameObject.FindObjectsByType<Toggle>(FindObjectsSortMode.None);
            foreach (Toggle toggle in toggles)
            {
            toggle.isOn = false; // Set all toggles to false
            }
        
            crosshair.enabled = false; // Disable the crosshair in the main menu
        }
        else
        {
            warningIndicator = GameObject.Find("Global Volume").GetComponent<Volume>();
            volumeProfile = warningIndicator.sharedProfile; // Get the volume profile from the global volume
            if (volumeProfile.TryGet(out Vignette vignetteEffect) && volumeProfile.TryGet(out LensDistortion lensDistortionEffect))
            {
                vignetteEffect.color.value = indicatorGreen; // Set the initial vignette color to green
                vignetteEffect.intensity.value = 0.309f; // Set the vignette intensity to a default value
                lensDistortionEffect.intensity.value = 0f; // Set the lens distortion intensity to a default value
            }
            else
            {
                Debug.LogError("Vignette or Lens Distortion effect not found in volume profile!"); // Log an error if the effects are not found
            }
            backgroundAnimator.SetBool("isOpen", true); // Ensure the background is open in other scenes
            StartCoroutine(StartLevel()); // Start the level coroutine if not in the office scene
        }


    }
    IEnumerator transitionToNewScene()
    {

        if (volumeProfile.TryGet(out Vignette vignetteEffect))
        {
            vignetteEffect.color.value = Color.black; // Set the initial vignette color to green
            vignetteEffect.intensity.value = 1f; // Set the vignette intensity to a default value

        }
        if (volumeProfile.TryGet(out LensDistortion lensDistortionEffect))
        {

            lensDistortionEffect.intensity.value = 0f; // Set the lens distortion intensity to a default value
        }
        while (vignetteEffect.intensity.value > 0.0f) // Gradually increase the vignette intensity
        {
            vignetteEffect.intensity.value -= 0.01f; // Gradually increase the vignette intensity
            if (lensDistortionEffect.intensity.value != 0f)
            {
                lensDistortionEffect.intensity.value += 0.01f; // Gradually increase the lens distortion intensity
            }
            yield return new WaitForSeconds(0.01f); // Wait for a short duration to create a smooth transition
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
                audioSource.pitch = UnityEngine.Random.Range(0.97f, 1.03f); // Randomize the pitch for variation
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
        isPaused = true; // Set the paused state to true when the case file is open
        crosshair.enabled = false; // Disable the crosshair when the case file is open
    }
    public void closeCaseFile()
    {
        caseFileCanvas.enabled = false; // Hide the case file canvas
        isPaused = false; // Set the paused state to true when the case file is open
        crosshair.enabled = true; // Enable the crosshair when the case file is closed
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
        player.GetComponent<PlayerBehavior>().hasFile = false; // Reset the case file flag
        dayCounter.CrossFadeAlpha(0.0f, 0f, true); // Fade in the day counter

        dayCounter.CrossFadeAlpha(1.0f, 1f, true); // Fade in the day counter
        yield return new WaitForSeconds(2f); // Wait for 2 seconds before fading out
        dayCounter.CrossFadeAlpha(0.0f, 1f, true); // Fade out the day counter
        yield return new WaitForSeconds(2f); // Wait for the fade out to complete
        dayCounter.enabled = false; // Disable the day counter after fading out

        backgroundAnimator.SetBool("isOpen", true);

        yield return new WaitForSeconds(1f); // Wait for the end of the frame to ensure everything is set up correctly
        beenToLevel = false; // Reset the flag indicating whether the player has been to the level before


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

        caseFile.UpdateDetails("Case File " + currentLevel + " - " + levelManager.levelName[currentLevel], levelManager.levelDate[currentLevel]); // Update the case file details with the current day and date
        caseFileObject.SetActive(true); // Show the case file object after starting the day
    }
    public void initiateLevel()
    {
        StartCoroutine(changeLevel());
    }
    IEnumerator changeLevel()
    {
        yield return ExitTransition(); // Start the exit transition
        SceneManager.LoadScene("Level " + currentLevel); // Load the next level scene based on the current level
        isPaused = false; // Set the paused state to false when changing levels
        beenToLevel = true; // S
    }
    private IEnumerator StartLevel()
    {
        yield return new WaitForSeconds(1f); // Wait for 1 second before starting the level
        Cursor.lockState = CursorLockMode.Locked; // Lock the cursor to the game window
        player = GameObject.FindWithTag("Player"); // Find the player GameObject
        player.GetComponent<FirstPersonController>().enabled = false; // Disable the character controller to

        yield return LevelCutscene(); // Start the level cutscene
        player.GetComponent<FirstPersonController>().enabled = true; // Enable the character controller after the cutscene
        player.GetComponent<PlayerBehavior>().isBusy = false; // Set the player as not busy after the cutscene
        StartCoroutine(LevelTimer()); // Start the level timer coroutine

    }
    private IEnumerator LevelCutscene()
    {
        inCutscene = true; // Set the cutscene state to true
        
        GameObject.Find("AccidentElements").GetComponent<Animator>().Play("Level1Accident", 0, 0f); // Play the accident cutscene animation
        GameObject.Find("AccidentElements").GetComponent<Animator>().speed = 0;
        yield return new WaitForSecondsRealtime(3f); // Wait for 2 seconds in real time to allow the cutscene to start
        GameObject.Find("AccidentElements").GetComponent<Animator>().speed = 1;
        Time.timeScale = 1f; // Resume the game after the cutscene starts
        while (inCutscene)
        {
            yield return new WaitForSeconds(0.1f); // Wait for a short duration to allow the cutscene to play
        }

    }
    public void EndCutscene()
    {
        inCutscene = false;
    }
    public void Impact()
    {
        Instantiate(explosionPrefab, GameObject.FindWithTag("Impact").transform.position, Quaternion.identity); // Instantiate the explosion effect at the impact point's position
    }
    private IEnumerator LevelTimer()
    {
        timer = levelTimer; // Set the timer to the level time
        warningIndicator = GameObject.Find("Global Volume").GetComponent<Volume>();
        volumeProfile = warningIndicator.sharedProfile; // Get the volume profile from the global volume
        timerUI.enabled = true; // Enable the timer UI

        if (volumeProfile.TryGet(out Vignette vignetteEffect))
        {

            vignetteEffect.color.value = indicatorGreen; // Set the initial vignette color to green
            vignetteEffect.intensity.value = 0.309f; // Set the vignette intensity to a default value
        }
        else
        {
            Debug.LogError("Vignette effect not found in volume profile!"); // Log an error if the vignette effect is not found
        }
        while (timer > 0)
        {
            timer -= Time.deltaTime; // Decrease the timer by the time since the last frame
            if (Mathf.Floor(timer % 60) < 10)
            {
                timerUI.text = "Time Left: " + Mathf.Floor(timer / 60) + ":0" + Mathf.Floor(timer % 60); // Update the timer UI text
            }
            else
            {
                timerUI.text = "Time Left: " + Mathf.Floor(timer / 60) + ":" + Mathf.Floor(timer % 60); // Update the timer UI text
            }

            if (Mathf.Ceil(timer) > 120)
            {

                vignetteEffect.color.value = indicatorGreen; // Set the vignette color to green when there is sufficient time left
                vignetteEffect.intensity.value = 0.309f; // Set the vignette intensity to a default value
            }
            else if (timer >= 60)
            {
                vignetteEffect.color.value = indicatorYellow; // Set the vignette color to yellow when time is low
                vignetteEffect.intensity.value = 0.349f; // Set the vignette intensity to a default value
            }
            else
            {
                vignetteEffect.color.value = indicatorRed; // Set the vignette color to red when time is critical
                vignetteEffect.intensity.value = 0.4f; // Set the vignette intensity to a default value
            }
            yield return null; // Wait for the next frame
        }
        StartCoroutine(ExitSequence()); // Start the exit sequence when the timer ends
        Debug.Log("Level timer ended");
        // Logic to end the level or transition to the next level can be added here
        yield return null; // Wait for the end of the frame to ensure everything is set up correctly
    }
    public IEnumerator ExitTransition()
    {
        player.GetComponent<PlayerBehavior>().OnOpenInventory(); // Close the case file if it is open
        player.GetComponent<FirstPersonController>().enabled = false; // Disable the character controller to prevent movement
        player.GetComponent<PlayerBehavior>().isBusy = true; // Set the player as busy during the exit sequence
        if (volumeProfile.TryGet(out Vignette vignetteEffect))
        {
            vignetteEffect.color.value = Color.black; // Set the initial vignette color to green
            vignetteEffect.intensity.value = 0f; // Set the vignette intensity to a default value

        }
        if (volumeProfile.TryGet(out LensDistortion lensDistortionEffect))
        {

            lensDistortionEffect.intensity.value = 0f; // Set the lens distortion intensity to a default value
        }
        while (vignetteEffect.intensity.value < 1.0f) // Gradually increase the vignette intensity
        {
            vignetteEffect.intensity.value += 0.01f; // Gradually increase the vignette intensity
            lensDistortionEffect.intensity.value -= 0.01f; // Gradually increase the lens distortion intensity
            yield return new WaitForSeconds(0.05f); // Wait for a short duration to create a smooth transition
        }
    }
    public IEnumerator ExitSequence()
    {
        timerUI.enabled = false; // Disable the timer UI

        yield return ExitTransition(); // Start the exit transition
        Debug.Log("Exit sequence end going to office");
        SceneManager.LoadScene("office"); // Load the office scene to reset the level
        yield return new WaitForSeconds(1f); // Wait for 1 second before continuing
        player.GetComponent<PlayerBehavior>().isBusy = false; // Set the player as not busy after the exit sequence
    }
    public void ExitLevel()
    {
        StopAllCoroutines(); // Stop all coroutines

        StartCoroutine(ExitSequence()); // Start the exit sequence when the player exits the level

    }
    public IEnumerator pingFile()
    {
        GameManager.Instance.pingRunning = true; // Set the ping effect as running

        Outline caseFileOutline = caseFileObject.GetComponent<Outline>();
        while (caseFileOutline.OutlineWidth < 10f) // Gradually increase the outline width
        {
            caseFileOutline.OutlineColor = Color.green; // Set the outline color to green
            caseFileOutline.OutlineWidth += 0.3f; // Increase the outline width
            yield return new WaitForSeconds(0.01f); // Wait for a short duration to create a smooth transition
        }
        while (caseFileOutline.OutlineWidth > 3f) // Gradually decrease the outline width
        {

            caseFileOutline.OutlineWidth -= 0.3f; // Decrease the outline width
            yield return new WaitForSeconds(0.01f); // Wait for a short duration to create a smooth transition
        }
        caseFileOutline.OutlineColor = Color.yellow; // Set the outline color to red
        Debug.Log("Case file pinged.");
        GameManager.Instance.pingRunning = false; // Set the ping effect as not running
    }
    public void ChangeCause(Toggle toggle)
    {
        if (toggle != currentCause)
        {
            Debug.Log("Changing cause to: " + toggle.name);
            if (currentCause != null)
            {
                currentCause.isOn = false; // Uncheck the previous cause toggle
            }
            currentCause = toggle; // Set the current cause to the newly selected toggle
        }
        else
        {

            currentCause.isOn = false; // Uncheck the current cause if it is already selected
            currentCause = null; // Reset the current cause to null
        }
    }
    public bool CheckCompletion()
    {
        
        if (currentCause != null && culpritDropdown.value != 0)
        {
            Debug.Log("Cause selected: " + currentCause.name + ", Culprit selected: " + culpritDropdown.options[culpritDropdown.value].text);
            return true;
        }
        else
        {
            Debug.Log("No cause or culprit selected.");
            Debug.Log("Current cause: " + (currentCause != null ? currentCause.name : "None") + ", Culprit dropdown value: " + culpritDropdown.value);
            return false; // Return false if no cause is selected
        }
    }
    public void EndDay()
    {
        endScreen.enabled = true; // Enable the end screen UI
        int totalScore = 0;
        int cluesFound = GameObject.Find("EvidenceCanvas").transform.childCount; // Count the number of clues found
        float timeRemaining = MathF.Ceiling(timer);
        Debug.Log(currentCause.name +"|"+ levelManager.causeList[currentLevel] + " | " + culpritDropdown.value + " | " + levelManager.culpritList[currentLevel]);
        if (currentCause.name == levelManager.causeList[currentLevel])
        {
            correctCause = true;
        }
        else
        {
            correctCause = false; // Set the correct cause flag to false if the selected cause does not match the level's cause
        }
        if (culpritDropdown.value == levelManager.culpritList[currentLevel])
        {
            correctCulprit = true;
        }
        else
        {
            correctCulprit = false; // Set the correct culprit flag to false if the selected culprit does not match the level's culprit
        }
        caseNameUI.text = "Case File " + currentLevel + " - " + levelManager.levelName[currentLevel]; // Update the case name UI text
        if (correctCulprit)
        {
            correctText.text = "Solved!";
        }
        else
        {
            correctText.text = "Unsolved";
        }
        if (correctCause)
        {
            causeUI.text = "Cause of accident identified:";
            causeScoreUI.text = 1000 + " points";
            totalScore += 1000; // Add the score for the cause of accident
        }
        else
        {
            causeUI.text = "Cause of accident incorrect.";
            causeScoreUI.text = 0 + " points";
        }
        if (correctCulprit)
        {
            culpritUI.text = "Culprit identified:";
            culpritScoreUI.text = 500 + " points";
            totalScore += 500; // Add the score for the culprit
        }
        else
        {
            culpritUI.text = "Culprit not identified.";
        }

        cluesUI.text = "Clues collected: " + cluesFound + "/" + levelManager.clueNumber[currentLevel]; // Update the clues UI text
        cluesScoreUI.text = cluesFound * 100 + " points"; // Update the clues score UI text
        totalScore += cluesFound * 100; // Add the score for the clues collected
        timerendUI.text = "Time remaining: " + timeRemaining + " seconds"; // Update the timer UI text
        timerendScoreUI.text = timeRemaining * 10 + " points"; // Update the timer score UI text
        totalScore += (int)(timeRemaining * 10); // Add the score for the time remaining

        totalscoreUI.text = totalScore + " points"; // Update the total score UI text

    }
    public void NextLevel()
    {
        currentLevel++;
        startingNewDay = true; // Set the flag to indicate a new day is starting
        SceneManager.LoadScene("office");
    }
}