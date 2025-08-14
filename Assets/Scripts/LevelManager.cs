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
    public int[] culpritList = new int[]
    {
        0,
        1,
        2,
    };
    public String[] causeList = new String[]
    {
        "what",
        "Crossing unfocused",
        "Red Light"
    };
    public int[] clueNumber = new int[]
    {
        0,
        3,
        3
    };

    public string[] Culprit1 = new string[]
    {
        "",
        "Pedestrian",
        "Green Car"
    };
    public string[] Culprit2 = new string[]
    {
        "",
        "Driver",
        "Purple Car"
    };
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
