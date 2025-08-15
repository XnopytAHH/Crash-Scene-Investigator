/*
* Author: Lim En Xu Jayson
* Date: 16/8/2025
* Description: A class to represent evidence in the game.
*/
using UnityEngine;
using UnityEngine.UI;
[System.Serializable]
public class Evidence
{
    /// <summary>
    /// The name of the evidence item.
    /// </summary>
    public string evidenceName; 
    /// <summary>
    /// The description of the evidence item.
    /// </summary>
    public string evidenceDescription; 
    /// <summary>
    /// The image of the evidence item.
    /// </summary>
    public Texture2D evidenceImage; 
}
