using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem.Controls;
using StarterAssets;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private float dialogueSpeed = 0.05f; // Speed of dialogue text appearing
    public static GameManager Instance;
    [SerializeField]
    private AudioSource audioSource;
    void Awake()
    {
        Canvas dialogueUI = GameObject.FindWithTag("UI Dialogue").GetComponent<Canvas>();
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
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void StartGame()
    {
        Debug.Log("Game Started");
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
        dialogueUI.transform.GetChild(2).transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = npc.name;
        player.GetComponent<FirstPersonController>().enabled = false;
        dialogueUI.enabled = true; // Show the dialogue UI
        foreach (DialogueLine line in dialogueLines.dialogueLines)
        {
            bool linePrinting = true; // Flag to check if the player is typing
            dialogueUI.transform.GetChild(0).GetComponent<Image>().sprite = line.CharacterSprite;
            yield return stepThruDialogue(dialogueUI.transform.GetChild(1).transform.GetChild(0).GetComponent<TextMeshProUGUI>(), dialogueSpeed, line);
            linePrinting = false; // Set the flag to false after the line is printed
            
            yield return new WaitUntil(() => !linePrinting && (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))); 
            yield return new WaitForSeconds(0.1f); // Small delay to prevent immediate re-triggering
            
            
        }
        player.GetComponent<PlayerBehavior>().isBusy = false; // Set the player as not busy after dialogue
        player.GetComponent<FirstPersonController>().enabled = true;
        dialogueUI.enabled = false; // Hide the dialogue UI
    }
    private IEnumerator stepThruDialogue(TextMeshProUGUI textbox,  float delay, DialogueLine line)
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
                audioSource.PlayOneShot(line.AudioClip); // Play the audio clip for the letter
            }
            
            textbox.text += letter; // Add one letter at a time
            
            yield return new WaitForSeconds(delay); // Wait for the specified delay
        }
    }
}
