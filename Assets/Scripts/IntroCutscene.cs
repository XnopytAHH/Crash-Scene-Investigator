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
    AudioSource audioSource;
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
    public void StartCutscene()
    {
        audioSource.PlayOneShot(cricketSound);
    }
    public void PlayCrashHonk()
    {
        audioSource.PlayOneShot(crashHonk);
    }
    public void FadeToWhite()
    {
        whiteFade.enabled = true;
        whiteFade.CrossFadeAlpha(1f, 1f, false);
        StartCoroutine(fadeTiming());
    }
    IEnumerator fadeTiming()
    {
        yield return new WaitForSeconds(2f); // Wait for 2 seconds
        SceneManager.LoadScene("Tutorial");
    }
}
