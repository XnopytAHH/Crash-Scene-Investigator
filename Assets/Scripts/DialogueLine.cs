/*
* Author: Lim En Xu Jayson
* Date: 16/8/2025
* Description: A class to dictate individual lines of dialogue for NPC interactions.
*/
using UnityEngine;
[System.Serializable]
public class DialogueLine
{
    /// <summary>
    /// The audio clip associated with the dialogue line. Usually the soundfont that said character uses
    /// </summary>
    public AudioClip AudioClip;
    /// <summary>
    /// The text of the dialogue line.
    /// </summary>
    public string Line;
    /// <summary>
    /// The image associated with the dialogue line if needed
    /// </summary>
    public Sprite lineImage; 
}
