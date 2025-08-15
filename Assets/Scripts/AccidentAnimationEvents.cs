/*
* Author: Lim En Xu Jayson
* Date: 15/8/2025
* Description: Handles events triggered by accident animations
*/
using Unity.VisualScripting;
using UnityEngine;

public class AccidentAnimationEvents : MonoBehaviour
{
    /// <summary>
    /// Reference to the Animator component.
    /// </summary>
    Animator animator;
    /// <summary>
    /// Impact event triggered by the animation.
    /// </summary>
    public void Impact()
    {
        // This method is called when the impact animation event is triggered.
        // You can add code here to handle the impact event, such as playing a sound or triggering an effect.
        Debug.Log("Impact event triggered.");


    }
    /// <summary>
    /// End cutscene event triggered by the animation.
    /// </summary>
    public void EndCutscene()
    {
        // This method is called when the cutscene ends.
        // You can add code here to handle the end of the cutscene, such as enabling player controls or UI elements.
        Debug.Log("Cutscene ended.");
        animator = GetComponent<Animator>();
        animator.enabled = false;
        GameManager.Instance.EndCutscene();
    }
}
