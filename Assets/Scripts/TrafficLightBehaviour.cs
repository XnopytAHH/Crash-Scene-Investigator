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
    Material material;
    void Awake()
    {
        material = gameObject.transform.GetChild(0).GetComponent<Renderer>().material;
        StartCoroutine(TrafficLightCycle());
        
    }

    IEnumerator TrafficLightCycle()
    {
        while (true)
        {
            color = "green";
            material.color = Color.green;
            yield return new WaitForSeconds(greenLightDuration);
            color = "blinking";
            material.color = Color.yellow;
            yield return new WaitForSeconds(blinkingLightDuration);
            color = "red";
            material.color = Color.red;
            yield return new WaitForSeconds(redLightDuration);
            
        }
    }
}
