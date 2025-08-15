/*
* Author: Lim En Xu Jayson
* Date: 16/8/2025
* Description: Manages traffic light behavior. Syncs all traffic lights in the scene
*/
using System.Collections;
using UnityEngine;

public class TrafficControl : MonoBehaviour
{
    /// <summary>
    /// Duration of the green light for north-south traffic.
    /// </summary>
    [SerializeField] private float nsGreenTotal = 30f;
    /// <summary>
    /// Duration of the green light for east-west traffic.
    /// </summary>
    [SerializeField] private float ewGreenTotal = 30f;

    /// <summary>
    /// Duration of the pedestrian walk signal.
    /// </summary>
    [SerializeField] private float pedestrianWalk = 15f;
    /// <summary>
    /// Duration of the pedestrian flashing signal.
    /// </summary>
    [SerializeField] private float pedestrianFlashing = 17f;

    /// <summary>
    /// Duration of the yellow light.
    /// </summary>
    [SerializeField] private float yellowDuration = 3f;
    /// <summary>
    /// Duration of the all-red light.
    /// </summary>
    [SerializeField] private float allRedDuration = 1f;
    /// <summary>
    /// Array of all traffic light GameObjects.
    /// </summary>
    GameObject[] trafficLights;
    /// <summary>
    /// The direction that the traffic lights start on.
    /// </summary>
    [SerializeField]
    string startsOn;
    
    /// <summary>
    /// Initializes all traffic lights in the level
    /// </summary>
   
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
    /// <summary>
    /// Sets the traffic lights to green for the NS direction.
    /// </summary>
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
    /// <summary>
    /// Sets the traffic lights to green for the EW direction.
    /// </summary>
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
    /// <summary>
    /// Sets the traffic lights to red for the NS direction.
    /// </summary>
    
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
    /// <summary>
    /// Sets the traffic lights to red for the EW direction.
    /// </summary>
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
    /// <summary>
    /// Sets the traffic lights to yellow for the NS direction.
    /// </summary>
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
    /// <summary>
    /// Sets the traffic lights to yellow for the EW direction.
    /// </summary>
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
    /// <summary>
    /// Sets the pedestrian traffic lights to green for the NS direction.
    /// </summary>
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
    /// <summary>
    /// Sets the pedestrian traffic lights to green for the EW direction.
    /// </summary>
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
    /// <summary>
    /// Sets the pedestrian traffic lights to flashing for the NS direction.
    /// </summary>
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

    /// <summary>
    /// Sets the pedestrian traffic lights to flashing for the EW direction.
    /// </summary>
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
    /// <summary>
    /// Sets the pedestrian traffic lights to red for the NS direction.
    /// </summary>
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
    /// <summary>
    /// Sets the pedestrian traffic lights to red for the EW direction.
    /// </summary>
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


