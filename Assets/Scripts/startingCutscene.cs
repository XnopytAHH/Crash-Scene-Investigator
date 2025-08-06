using UnityEngine;
using UnityEngine.UI;
using System.Collections;
public class startingCutscene : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private GameObject myCar;
    [SerializeField]
    private GameObject companyLogo;
    [SerializeField]
    private float fadeDuration = 1f;
    [SerializeField]
    private float logoDisplayDuration = 2f;
    private Animator backgroundAnimator;
    private Animator menuAnimator;


    void Start()
    {
        StartCoroutine(StartIntro());
    }


    // Update is called once per frame
    void Update()
    {
        
    }
    private IEnumerator ShowCompanyLogo()
    {
        yield return new WaitForSeconds(1f); // Wait for 2 seconds before showing the logo
        companyLogo.GetComponent<Image>().CrossFadeAlpha(1.0f, fadeDuration, true);
        yield return new WaitForSeconds(logoDisplayDuration);
        companyLogo.GetComponent<Image>().CrossFadeAlpha(0.0f, fadeDuration, true);
        yield return new WaitForSeconds(fadeDuration); // Wait for the fade out to complete
    }
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
    private IEnumerator StartingCutscene()
    {
        myCar = GameObject.Find("car");

        while (myCar.transform.position.x < -6f)
        {

            myCar.GetComponent<Rigidbody>().AddForce(myCar.transform.forward * 2000f, ForceMode.Force);
            yield return null; // Wait for the next frame
        }
        yield return new WaitForSeconds(0.3f); // Wait for 0.3 seconds before stopping the car
        Time.timeScale = 0f; // Pause the game
        myCar.GetComponent<Rigidbody>().linearVelocity = Vector3.zero; // Stop the car
        menuAnimator.SetBool("MenuIn", true); // Trigger the menu animation
        Debug.Log("Car has reached the destination, game paused and menu triggered.");
        Cursor.lockState = CursorLockMode.None; // Store the previous cursor lock state

    }
}
