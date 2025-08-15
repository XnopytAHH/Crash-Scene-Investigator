/*
* Author: Lim En Xu Jayson
* Date: 16/8/2025
* Description: Handles the behavior of pedestrian traffic lights in the game.
*/
using UnityEngine;
using System.Collections;
public class PedestrianTrafficLightBehaviour : MonoBehaviour
{
    /// <summary>
    /// The current color of the pedestrian traffic light.
    /// </summary>
    public string color;
    /// <summary>
    /// The GameObject representing the green light.
    /// </summary>
    [SerializeField]
    GameObject greenLight;
    /// <summary>
    /// The GameObject representing the red light.
    /// </summary>
    [SerializeField]
    GameObject redLight;
    /// <summary>
    /// The direction of the pedestrian traffic light (e.g., "north", "south").
    /// </summary>
    public string trafficLightDirection;
    /// <summary>
    /// The material of the pedestrian traffic light.
    /// </summary>
    Material material;
    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        material = gameObject.transform.GetChild(0).GetComponent<Renderer>().material;

        greenLight.SetActive(false);
        redLight.SetActive(false);

    }
    /// <summary>
    /// Sets the color of the pedestrian traffic light.
    /// </summary>
    public void SetColor(string newColor)
    {
        color = newColor;
        if (newColor == "green")
        {
            greenLight.SetActive(true);
            redLight.SetActive(false);
            StopAllCoroutines();

        }
        else if (newColor == "red")
        {
            greenLight.SetActive(false);
            redLight.SetActive(true);
            StopAllCoroutines();

        }
        else if (newColor == "flashing")
        {

            redLight.SetActive(false);
            StartCoroutine(FlashGreen());
        }
        else
        {
            Debug.LogWarning("Invalid traffic light color: " + color);
        }
    }
    /// <summary>
    /// Coroutine to flash the green light.
    /// </summary>
    IEnumerator FlashGreen()
    {
        while (true)
        {
            greenLight.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            greenLight.SetActive(false);
            yield return new WaitForSeconds(0.5f);
        }
    }
    
}
