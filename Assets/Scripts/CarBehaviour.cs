using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using TMPro;

public class CarBehaviour : MonoBehaviour
{

    NavMeshAgent carAgent;
    Transform targetTransform;
    string currentState;

    [SerializeField] Transform endPoint;

    bool waitingForLight = false;
    bool waitingForPlayer = false;
    bool waitingForCar = false;
    bool isTalking = false;

    [SerializeField] float textOffset = 1.5f; // Offset for the

    /// <summary>
    /// atLight is a boolean that indicates whether the pedestrian is currently at a traffic light.
    /// </summary>
    bool atLight = false;
    [SerializeField]
    AudioClip honkSound;
    AudioSource audioSource;
    Transform startPos;
    void Awake()
    {

        startPos = transform;
        carAgent = GetComponent<NavMeshAgent>();
        carAgent.speed = 7;

        StartCoroutine(SwitchState("Idle"));
    }

    IEnumerator SwitchState(string newState)
    {
        if (currentState == newState) yield break;
        currentState = newState;
        StartCoroutine(newState);
    }

    IEnumerator Idle()
    {
        while (currentState == "Idle")
        {
            carAgent.ResetPath();
            carAgent.velocity = Vector3.zero;
            if (!isTalking && !waitingForLight &&
                (transform.position.x != endPoint.position.x || transform.position.z != endPoint.position.z))
            {
                if (!waitingForPlayer && !waitingForCar && !waitingForLight)
                {
                    StartCoroutine(SwitchState("Driving"));
                    yield break;
                }
                else
                {
                    AudioSource.PlayClipAtPoint(honkSound, transform.position, 1f);
                    yield return new WaitForSeconds(2f);
                }

            }

            yield return null;
        }
    }
    IEnumerator Driving()
    {
        while (currentState == "Driving")
        {
            if (carAgent.transform.position.x == endPoint.position.x && carAgent.transform.position.z == endPoint.position.z)
            {
                carAgent.transform.position = startPos.position;
                StartCoroutine(SwitchState("Idle"));
                yield break;
            }
            carAgent.SetDestination(endPoint.position);
            yield return null;
        }
    }
    void OnTriggerEnter(Collider other)
    {
        atLight = true;
        if (other.CompareTag("TrafficLight"))
        {
            var light = other.GetComponent<TrafficLightBehaviour>();
            if (light.color == "red")
            {
                waitingForLight = true;
                StartCoroutine(SwitchState("Idle"));
            }
        }
        
    }
    void OnTriggerStay(Collider other)
    {

        if (other.CompareTag("Player"))
        {
            waitingForPlayer = true;
            StartCoroutine(SwitchState("Idle"));
        }
        if (other.CompareTag("Car"))
        {
            waitingForCar = true;
            StartCoroutine(SwitchState("Idle"));
        }
        if (other.CompareTag("TrafficLight"))
        {
            var light = other.GetComponent<TrafficLightBehaviour>();
            if (light.color == "red")
            {
                waitingForLight = true;
                StartCoroutine(SwitchState("Idle"));
            }
            if (light.color == "green")
            {
                waitingForLight = false;
            }
        }
    }
    void OnTriggerExit(Collider other)
    {

        if (other.CompareTag("Player"))
        {

            waitingForPlayer = false;
            StartCoroutine(SwitchState("Driving"));
        }
    }
}
