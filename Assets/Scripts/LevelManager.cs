/*
* Author: Lim En Xu Jayson
* Date: 16/8/2025
* Description: Stores information about the game levels.
*/
using System;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    
    /// <summary>
    /// levelName is a list containing the names of the levels in the game.
    /// </summary>
    public String[] levelName = new String[]
    {
        "",
        "Crash at Clementi Road",
        "Crash at Bukit Timah Road"
    };
    /// <summary>
    /// levelDate is a list containing the dates associated with each level.
    /// </summary>
    public String[] levelDate = new String[]
    {
       "",
        "Date: 20XX/10/01, 08:02hrs",
        "Date: 20XX/10/02, 14:18hrs"
    };
    /// <summary>
    /// culpritList is a list containing the indices of the culprits for each level.
    /// </summary>
    public int[] culpritList = new int[]
    {
        0,
        1,
        2,
    };
    /// <summary>
    /// causeList is a list containing the possible causes for each level.
    /// </summary>
    public String[] causeList = new String[]
    {
        "what",
        "Crossing unfocused",
        "Red Light"
    };
    /// <summary>
    /// clueNumber is a list containing the number of clues for each level.
    /// </summary>
    public int[] clueNumber = new int[]
    {
        0,
        3,
        3
    };
    /// <summary>
    /// Culprit1 is a list containing the names of the first culprits for each level.
    /// </summary>
    public string[] Culprit1 = new string[]
    {
        "",
        "Pedestrian",
        "Green Car"
    };
    /// <summary>
    /// Culprit2 is a list containing the names of the second culprits for each level.
    /// </summary>
    public string[] Culprit2 = new string[]
    {
        "",
        "Driver",
        "Purple Car"
    };
    /// <summary>
    /// accidentPhoto is a list containing the accident photos for each level.
    /// </summary>
    public Sprite[] accidentPhoto;
}
