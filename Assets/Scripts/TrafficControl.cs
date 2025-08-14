using System.Collections;
using NUnit.Framework.Constraints;
using UnityEngine;

public class TrafficControl : MonoBehaviour
{

    [SerializeField] private float nsGreenTotal = 30f;
    [SerializeField] private float ewGreenTotal = 30f;


    [SerializeField] private float pedestrianWalk = 15f;
    [SerializeField] private float pedestrianFlashing = 17f;


    [SerializeField] private float yellowDuration = 3f;
    [SerializeField] private float allRedDuration = 1f;

    GameObject[] trafficLights;
    [SerializeField]
    string startsOn;
    void Awake()
    {
        Debug.Log(gameObject.name);
        
    }
    
    public IEnumerator InitializeTrafficLights()
    {
        if (startsOn == "NS")
        {
            trafficLights = GameObject.FindGameObjectsWithTag("TrafficLight");
            foreach (GameObject light in trafficLights)
            {
                if (light.GetComponent<TrafficLightBehaviour>() != null && light.GetComponent<TrafficLightBehaviour>().trafficLightDirection == "EW")
                {
                    light.GetComponent<TrafficLightBehaviour>().SetColor("red");
                }
                if (light.GetComponent<TrafficLightBehaviour>() != null && light.GetComponent<TrafficLightBehaviour>().trafficLightDirection == "NS")
                {
                    light.GetComponent<TrafficLightBehaviour>().SetColor("green");
                }

                if (light.GetComponent<PedestrianTrafficLightBehaviour>() != null && light.GetComponent<PedestrianTrafficLightBehaviour>().trafficLightDirection == "EW")
                {
                    light.GetComponent<PedestrianTrafficLightBehaviour>().SetColor("red");
                }
                if (light.GetComponent<PedestrianTrafficLightBehaviour>() != null && light.GetComponent<PedestrianTrafficLightBehaviour>().trafficLightDirection == "NS")
                {
                    StartCoroutine(NSpedestrianGreen());
                }
                yield return null;
            }
            StartCoroutine(NSGreen());
        }
        else if (startsOn == "EW")
        {
            trafficLights = GameObject.FindGameObjectsWithTag("TrafficLight");
            foreach (GameObject light in trafficLights)
            {
                if (light.GetComponent<TrafficLightBehaviour>() != null && light.GetComponent<TrafficLightBehaviour>().trafficLightDirection == "NS")
                {
                    light.GetComponent<TrafficLightBehaviour>().SetColor("red");
                }
                if (light.GetComponent<TrafficLightBehaviour>() != null && light.GetComponent<TrafficLightBehaviour>().trafficLightDirection == "EW")
                {
                    light.GetComponent<TrafficLightBehaviour>().SetColor("green");
                }

                if (light.GetComponent<PedestrianTrafficLightBehaviour>() != null && light.GetComponent<PedestrianTrafficLightBehaviour>().trafficLightDirection == "EW")
                {
                    StartCoroutine(EWpedestrianGreen());
                }
                if (light.GetComponent<PedestrianTrafficLightBehaviour>() != null && light.GetComponent<PedestrianTrafficLightBehaviour>().trafficLightDirection == "NS")
                {
                    light.GetComponent<PedestrianTrafficLightBehaviour>().SetColor("red");
                }
                yield return null;
            }
            StartCoroutine(EWGreen());
        }
        else
        {
            Debug.LogError("Invalid starting direction specified: " + startsOn);
        }

        yield return null;
    }
    IEnumerator NSGreen()
    {
        foreach (GameObject light in trafficLights)
        {
            if (light.GetComponent<TrafficLightBehaviour>() != null && light.GetComponent<TrafficLightBehaviour>().trafficLightDirection == "NS")
            {
                light.GetComponent<TrafficLightBehaviour>().SetColor("green");
            }
        }
        StartCoroutine(NSpedestrianGreen());
        yield return new WaitForSeconds(nsGreenTotal);
        StartCoroutine(NSYellow());
    }
    IEnumerator EWGreen()
    {
        foreach (GameObject light in trafficLights)
        {
            if (light.GetComponent<TrafficLightBehaviour>() != null && light.GetComponent<TrafficLightBehaviour>().trafficLightDirection == "EW")
            {
                light.GetComponent<TrafficLightBehaviour>().SetColor("green");
            }
        }
        StartCoroutine(EWpedestrianGreen());
        yield return new WaitForSeconds(ewGreenTotal);
        StartCoroutine(EWYellow());
    }
    IEnumerator NSRed()
    {
        foreach (GameObject light in trafficLights)
        {
            if (light.GetComponent<TrafficLightBehaviour>() != null && light.GetComponent<TrafficLightBehaviour>().trafficLightDirection == "NS")
            {
                light.GetComponent<TrafficLightBehaviour>().SetColor("red");
            }
        }
        yield return new WaitForSeconds(allRedDuration);
        StartCoroutine(EWGreen());
    }
    IEnumerator EWRed()
    {
        foreach (GameObject light in trafficLights)
        {
            if (light.GetComponent<TrafficLightBehaviour>() != null && light.GetComponent<TrafficLightBehaviour>().trafficLightDirection == "EW")
            {
                light.GetComponent<TrafficLightBehaviour>().SetColor("red");
            }
        }
        yield return new WaitForSeconds(allRedDuration);
        StartCoroutine(NSGreen());
    }
    IEnumerator NSYellow()
    {
        foreach (GameObject light in trafficLights)
        {
            if (light.GetComponent<TrafficLightBehaviour>() != null && light.GetComponent<TrafficLightBehaviour>().trafficLightDirection == "NS")
            {
                light.GetComponent<TrafficLightBehaviour>().SetColor("orange");
            }
        }
        yield return new WaitForSeconds(yellowDuration);
        StartCoroutine(NSRed());
    }
    IEnumerator EWYellow()
    {
        foreach (GameObject light in trafficLights)
        {
            if (light.GetComponent<TrafficLightBehaviour>() != null && light.GetComponent<TrafficLightBehaviour>().trafficLightDirection == "EW")
            {
                light.GetComponent<TrafficLightBehaviour>().SetColor("orange");
            }
        }
        yield return new WaitForSeconds(yellowDuration);
        StartCoroutine(EWRed());
    }
    IEnumerator NSpedestrianGreen()
    {
        foreach (GameObject light in trafficLights)
        {
            if (light.GetComponent<PedestrianTrafficLightBehaviour>() != null && light.GetComponent<PedestrianTrafficLightBehaviour>().trafficLightDirection == "NS")
            {
                light.GetComponent<PedestrianTrafficLightBehaviour>().SetColor("green");
            }
        }
        yield return new WaitForSeconds(pedestrianWalk);
        StartCoroutine(NSpedestrianFlash());

    }
    IEnumerator EWpedestrianGreen()
    {
        foreach (GameObject light in trafficLights)
        {
            if (light.GetComponent<PedestrianTrafficLightBehaviour>() != null && light.GetComponent<PedestrianTrafficLightBehaviour>().trafficLightDirection == "EW")
            {
                light.GetComponent<PedestrianTrafficLightBehaviour>().SetColor("green");
            }
        }
        yield return new WaitForSeconds(pedestrianWalk);
        StartCoroutine(EWpedestrianFlash());
    }
    IEnumerator NSpedestrianFlash()
    {
        foreach (GameObject light in trafficLights)
        {
            if (light.GetComponent<PedestrianTrafficLightBehaviour>() != null && light.GetComponent<PedestrianTrafficLightBehaviour>().trafficLightDirection == "NS")
            {
                light.GetComponent<PedestrianTrafficLightBehaviour>().SetColor("flashing");
            }
        }
        yield return new WaitForSeconds(pedestrianFlashing);
        StartCoroutine(NSpedestrianRed());
    }
    IEnumerator EWpedestrianFlash()
    {
        foreach (GameObject light in trafficLights)
        {
            if (light.GetComponent<PedestrianTrafficLightBehaviour>() != null && light.GetComponent<PedestrianTrafficLightBehaviour>().trafficLightDirection == "EW")
            {
                light.GetComponent<PedestrianTrafficLightBehaviour>().SetColor("flashing");
            }
        }
        yield return new WaitForSeconds(pedestrianFlashing);
        StartCoroutine(EWpedestrianRed());
    }
    IEnumerator NSpedestrianRed()
    {
        foreach (GameObject light in trafficLights)
        {
            if (light.GetComponent<PedestrianTrafficLightBehaviour>() != null && light.GetComponent<PedestrianTrafficLightBehaviour>().trafficLightDirection == "NS")
            {
                light.GetComponent<PedestrianTrafficLightBehaviour>().SetColor("red");
            }
        }
        yield return null;
    }
    IEnumerator EWpedestrianRed()
    {
        foreach (GameObject light in trafficLights)
        {
            if (light.GetComponent<PedestrianTrafficLightBehaviour>() != null && light.GetComponent<PedestrianTrafficLightBehaviour>().trafficLightDirection == "EW")
            {
                light.GetComponent<PedestrianTrafficLightBehaviour>().SetColor("red");
            }
        }
        yield return null;
    }

}


