/*
* Author: Lim En Xu Jayson
* Date: 16/8/2025
* Description: Handles the majority of game logic and state management.
*/
using System;

using UnityEngine;

public class NPCBehavior : MonoBehaviour
{
    /// <summary>
    /// Array of Dialogue objects containing the lines for this NPC
    /// </summary>
    [SerializeField]
    public Dialogue[] dialogueLines;
    /// <summary>
    /// Description of the NPC for the case file
    /// </summary>
    [SerializeField]
    public string description;
    /// <summary>
    /// Gets the dialogue lines for this NPC
    /// </summary>
    public Dialogue getNPCLines(int dialogueIndex)
    {
        if (dialogueIndex < dialogueLines.Length)
        {
            return dialogueLines[dialogueIndex];
        }
        return null;
    }
    /// <summary>
    /// Forces the npc to face the player
    /// </summary>
    void Update()
    {
        Vector3 directiontoplayer = (GameObject.FindWithTag("Player").transform.position - gameObject.transform.position)*-1;
        directiontoplayer.y = 0; // Keep the NPC facing horizontally
        gameObject.transform.rotation = Quaternion.LookRotation(directiontoplayer);
    }
}
