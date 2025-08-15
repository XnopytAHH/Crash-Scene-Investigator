/*
* Author: Lim En Xu Jayson
* Date: 16/8/2025
* Description: Manages traffic light behavior, mainly changing the color of the current light
*/
using UnityEngine;
using System.Collections;
public class TrafficLightBehaviour : MonoBehaviour
{
    /// <summary>
    /// The current color of the traffic light.
    /// </summary>
    public string color;
    /// <summary>
    /// The green light GameObject.
    /// </summary>
    [SerializeField]
    GameObject greenLight;
    /// <summary>
    /// The red light GameObject.
    /// </summary>
    [SerializeField]
    GameObject redLight;
    /// <summary>
    /// The orange light GameObject.
    /// </summary>
    [SerializeField]
    GameObject orangeLight;
    /// <summary>
    /// The direction of the traffic light (NS or EW).
    /// </summary>
    [SerializeField]
    public string trafficLightDirection;
    /// <summary>
    /// The material of the traffic light.
    /// </summary>
    Material material;
    /// <summary>
    /// Initialize the current traffic light.
    /// </summary>
    void Awake()
    {
        material = gameObject.transform.GetChild(0).GetComponent<Renderer>().material;

        greenLight.SetActive(false);
        redLight.SetActive(false);
        orangeLight.SetActive(false);
        
    }
    /// <summary>
    /// Sets the color of the traffic light to the specified value.
    /// </summary>
    public void SetColor(string newColor)
    {
        color = newColor;
        if (newColor == "green")
        {
            greenLight.SetActive(true);
            redLight.SetActive(false);
            orangeLight.SetActive(false);

        }
        else if (newColor == "red")
        {
            greenLight.SetActive(false);
            redLight.SetActive(true);
            orangeLight.SetActive(false);

        }
        else if (newColor == "orange")
        {
            greenLight.SetActive(false);
            redLight.SetActive(false);
            orangeLight.SetActive(true);

        }
        else
        {
            Debug.LogWarning("Invalid traffic light color: " + color);
        }
    }
    
}
