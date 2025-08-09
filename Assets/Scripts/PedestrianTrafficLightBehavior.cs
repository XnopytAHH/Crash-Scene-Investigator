using UnityEngine;
using System.Collections;
public class PedestrianTrafficLightBehaviour : MonoBehaviour
{
    public string color;

    [SerializeField]
    GameObject greenLight;
    [SerializeField]
    GameObject redLight;
    public string trafficLightDirection;

    Material material;
    void Awake()
    {
        material = gameObject.transform.GetChild(0).GetComponent<Renderer>().material;

        greenLight.SetActive(false);
        redLight.SetActive(false);

    }
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
