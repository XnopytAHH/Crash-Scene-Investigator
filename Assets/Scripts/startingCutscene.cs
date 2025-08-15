/*
* Author: Lim En Xu Jayson
* Date: 16/8/2025
* Description: Starts the main menu cutscene.
*/
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
public class startingCutscene : MonoBehaviour
{
    /// <summary>
    /// Reference to the player's car GameObject.
    /// </summary>
    private GameObject myCar;
    /// <summary>
    /// Reference to the company logo GameObject.
    /// </summary>
    [SerializeField]
    private GameObject companyLogo;
    /// <summary>
    /// Duration of the fade in/out effect for the logo.
    /// </summary>
    [SerializeField]
    private float fadeDuration = 1f;
    /// <summary>
    /// Duration for which the logo is displayed.
    /// </summary>
    [SerializeField]
    private float logoDisplayDuration = 2f;
    /// <summary>
    /// Reference to the background animator.
    /// </summary>
    private Animator backgroundAnimator;
    /// <summary>
    /// Reference to the menu animator.
    /// </summary>
    private Animator menuAnimator;
    /// <summary>
    /// Reference to the sound manager.
    /// </summary>
    private SoundManager soundManager;
    /// <summary>
    /// initialize at the start of the scene
    /// </summary>
    void Start()
    {
        soundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();
        StartCoroutine(StartIntro());
    }

    /// <summary>
    /// Shows the company logo with a fade in/out effect.
    /// </summary>
    private IEnumerator ShowCompanyLogo()
    {
        yield return new WaitForSeconds(1f); // Wait for 2 seconds before showing the logo
        companyLogo.GetComponent<Image>().CrossFadeAlpha(1.0f, fadeDuration, true);
        yield return new WaitForSeconds(logoDisplayDuration);
        companyLogo.GetComponent<Image>().CrossFadeAlpha(0.0f, fadeDuration, true);
        yield return new WaitForSeconds(fadeDuration); // Wait for the fade out to complete
    }
    /// <summary>
    /// Starts the intro sequence.
    /// </summary>
    private IEnumerator StartIntro()
    {
        Cursor.lockState = CursorLockMode.Locked; // Unlock the cursor at the start
        backgroundAnimator = GameObject.FindWithTag("BackgroundUI").GetComponent<Animator>();
        backgroundAnimator.SetBool("isOpen", false); // Ensure the background is closed at the start
        backgroundAnimator.Play("Closed", 0, 0f); // Play the fade-in animation immediately
        Debug.Log("Background Animator found: " + backgroundAnimator);
        menuAnimator = GameObject.FindWithTag("UI Menu").GetComponent<Animator>();
        companyLogo.GetComponent<Image>().CrossFadeAlpha(0.0f, 0f, false);
        menuAnimator.SetBool("MenuIn", false); // Ensure the menu is not visible at the start
        yield return StartCoroutine(ShowCompanyLogo());

        backgroundAnimator.SetBool("isOpen", true);
        Debug.Log("Fade in animation started");
        yield return new WaitForSeconds(3f);

        yield return StartCoroutine(StartingCutscene());
    }
    /// <summary>
    /// Starts the cutscene sequence.
    /// </summary>
    private IEnumerator StartingCutscene()
    {
        myCar = GameObject.Find("car");

        while (myCar.transform.position.x < -6f)
        {

            myCar.GetComponent<Rigidbody>().AddForce(myCar.transform.forward * 2000f, ForceMode.Force);
            yield return null; // Wait for the next frame
        }
        yield return new WaitForSeconds(0.3f); // Wait for 0.3 seconds before stopping the car
        soundManager.MenuMusic();
        Time.timeScale = 0f; // Pause the game
        myCar.GetComponent<Rigidbody>().linearVelocity = Vector3.zero; // Stop the car
        menuAnimator.SetBool("MenuIn", true); // Trigger the menu animation
        Debug.Log("Car has reached the destination, game paused and menu triggered.");
        Cursor.lockState = CursorLockMode.None; // Store the previous cursor lock state

    }
}
