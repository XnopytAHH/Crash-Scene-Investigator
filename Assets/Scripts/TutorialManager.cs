using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.VFX;

public class TutorialManager : MonoBehaviour
{
    
    private GameObject player;
    private NPCBehavior bigBoss;
    private Animator bigBossAnimator;
    private VisualEffect deathSpawnParticles;

    public IEnumerator StartTutorial()
    {
        player = GameObject.Find("Player");
        player.GetComponent<PlayerBehavior>().isBusy = true; // Set the player as busy while starting the day
        yield return new WaitForSeconds(1f);
        Debug.Log("Tutorial Started");
        bigBoss = GameObject.FindWithTag("NPC").GetComponent<NPCBehavior>(); // Find the big boss NPC
        bigBossAnimator = GameObject.FindWithTag("NPC").GetComponent<Animator>();
        deathSpawnParticles = GameObject.FindWithTag("NPC").GetComponentInChildren<VisualEffect>(false); // Find the death spawn particles effect
        bigBossAnimator.Play("BossStandby", 0, 0f); // Play the standby animation for the big boss NPC
        //Death spawn in and dialogue
        deathSpawnParticles.Play(); // Play the death spawn particles effect

        yield return new WaitForSeconds(2f); // Wait for 2 seconds before starting the dialogue

        bigBossAnimator.Play("BossSpawn", 0, 0f); // Play the spawn in animation for the big boss NPC
        yield return new WaitForSeconds(3f); // Wait for 3 seconds to allow the animation to play
        deathSpawnParticles.Stop(); // Stop the death spawn particles effect

        yield return null; // Wait for the end of the frame to ensure everything is set up correctly
        // Start the dialogue with the big boss NPC
        Dialogue currentDialogueLines = bigBoss.getNPCLines(7); // Get the dialogue lines for the big boss NPC

        Debug.Log("Starting dialogue with big boss: " + bigBoss.gameObject.name);
        StartCoroutine(GameManager.Instance.NPCDialogue(bigBoss.gameObject, currentDialogueLines)); // Start the dialogue coroutine with the big boss NPC
        yield return null; // Wait for the end of the frame to ensure everything is set up correctly
    }
}
