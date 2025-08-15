/*
* Author: Lim En Xu Jayson
* Date: 16/8/2025
* Description: Class dictating the dialogue system for NPC interactions.
*/
using UnityEngine;
[System.Serializable]
public class Dialogue
{
    /// <summary>
    /// The current dialogue level.
    /// </summary>
    public int dialogueLevel = 0; 
    /// <summary>
    /// The lines of dialogue for the NPC.
    /// </summary>
    public DialogueLine[] dialogueLines;


}
