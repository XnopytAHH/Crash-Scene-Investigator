using UnityEngine;
using System.Collections;
public class TrafficLightBehaviour : MonoBehaviour
{
    public string color;
    
    [SerializeField]
    GameObject greenLight;
    [SerializeField]
    GameObject redLight;
    [SerializeField]
    GameObject orangeLight;
    [SerializeField]
    public string trafficLightDirection;

    Material material;
    void Awake()
    {
        material = gameObject.transform.GetChild(0).GetComponent<Renderer>().material;

        greenLight.SetActive(false);
        redLight.SetActive(false);
        orangeLight.SetActive(false);
        
    }
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
