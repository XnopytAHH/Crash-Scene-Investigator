using UnityEngine;
using System.Collections;
public class TrafficLightBehaviour : MonoBehaviour
{
    public string color;
    [SerializeField]
    public float greenLightDuration = 10f;
    [SerializeField]
    public float redLightDuration = 10f;
    [SerializeField]
    public float blinkingLightDuration = 5f;
    [SerializeField]
    GameObject greenLight;
    [SerializeField]
    GameObject redLight;
    [SerializeField]
    GameObject orangeLight;

    Material material;
    void Awake()
    {
        material = gameObject.transform.GetChild(0).GetComponent<Renderer>().material;

        greenLight.SetActive(false);
        redLight.SetActive(false);
        orangeLight.SetActive(false);
        StartCoroutine(TrafficLightCycle());
    }

    IEnumerator TrafficLightCycle()
    {
        while (true)
        {
            color = "green";
            greenLight.SetActive(true);
            redLight.SetActive(false);
            yield return new WaitForSeconds(greenLightDuration);
            color = "blinking";
            greenLight.SetActive(false);
            orangeLight.SetActive(true);
            yield return new WaitForSeconds(blinkingLightDuration);
            color = "red";
            redLight.SetActive(true);
            orangeLight.SetActive(false);
            yield return new WaitForSeconds(redLightDuration);
            
        }
    }
}
