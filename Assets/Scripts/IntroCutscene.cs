/*
* Author: Lim En Xu Jayson
* Date: 16/8/2025
* Description: Handles some logic for the intro cutscene.
*/
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class IntroCutscene : MonoBehaviour
{
    /// <summary>
    /// CricketSound is a reference to the AudioClip that plays the cricket sound during the intro cutscene.
    /// </summary>
    public AudioClip cricketSound;
    /// <summary>
    /// CrashHonk is a reference to the AudioClip that plays the crash honk sound during the intro cutscene.
    /// </summary>
    public AudioClip crashHonk;
    /// <summary>
    /// whiteFade is a reference to the GameObject that represents the white fade effect during the intro cutscene.
    /// </summary>
    public Image whiteFade;
    /// <summary>
    /// AudioSource is a reference to the AudioSource component that plays audio clips during the intro cutscene.
    /// </summary>
    AudioSource audioSource;
    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        Animator animator = gameObject.GetComponent<Animator>();
        animator.Play("IntroCutscene");
        audioSource = gameObject.GetComponent<AudioSource>();
        // Ensure the white fade is initially disabled
        if (whiteFade != null)
        {
            whiteFade.CrossFadeAlpha(0f, 0f, false);
            whiteFade.enabled = false;
        }
    }
    /// <summary>
    /// StartCutscene is called to initiate the intro cutscene sound effects
    /// </summary>
    public void StartCutscene()
    {
        audioSource.PlayOneShot(cricketSound);
    }
    /// <summary>
    /// PlayCrashHonk is called to play the crash honk sound effect on the car collision
    /// </summary>
    public void PlayCrashHonk()
    {
        audioSource.PlayOneShot(crashHonk);
    }
    /// <summary>
    /// FadeToWhite is called to initiate the white fade effect to transition out of the level
    /// </summary>
    public void FadeToWhite()
    {
        whiteFade.enabled = true;
        whiteFade.CrossFadeAlpha(1f, 1f, false);
        StartCoroutine(fadeTiming());
    }
    /// <summary>
    /// Coroutine for the fade timing.
    /// </summary>
    
    IEnumerator fadeTiming()
    {
        yield return new WaitForSeconds(2f); // Wait for 2 seconds
        SceneManager.LoadScene("Tutorial");
    }
}
