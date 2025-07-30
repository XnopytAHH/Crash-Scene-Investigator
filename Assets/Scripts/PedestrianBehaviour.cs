using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using UnityEngine.Rendering;
using System.Diagnostics;
using Unity.VisualScripting;

public class PedestrianBehaviour : MonoBehaviour
{
    NavMeshAgent pedestrianAgent;
    Transform targetTransform;
    string currentState;
    [SerializeField]
    Transform endPoint;
    bool waitingForLight = false;
    void Awake()
    {
        pedestrianAgent = GetComponent<NavMeshAgent>();
        StartCoroutine(SwitchState("Idle"));
    }
    IEnumerator SwitchState(string newState)
    {
        if (currentState == newState) yield break;
        currentState = newState;
        pedestrianAgent.isStopped = false;
        StartCoroutine(newState);
    }
    IEnumerator Idle()
    {
        while (currentState == "Idle")
        {

            pedestrianAgent.isStopped = true;
            if (waitingForLight)
            {
                yield return null;
            }
            else if (transform.position.x != endPoint.position.x &&
                   transform.position.z != endPoint.position.z)
            {
                yield return new WaitForSeconds(2f);
                StartCoroutine(SwitchState("Walking"));
            }
            else
            {
                yield break;
            }


        }
    }
    IEnumerator Walking()
    {
        while (currentState == "Walking")
        {
            pedestrianAgent.isStopped = false;
            pedestrianAgent.SetDestination(endPoint.position);
            while (transform.position.x != endPoint.position.x &&
                   transform.position.z != endPoint.position.z)
            {
                yield return null;
            }
            StartCoroutine(SwitchState("Idle"));
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("TrafficLight") && (other.GetComponent<TrafficLightBehaviour>().color == "red" || other.GetComponent<TrafficLightBehaviour>().color == "blinking"))
        {
            // If the traffic light is red, stop the pedestrian
            pedestrianAgent.isStopped = true;
            waitingForLight = true;
            StartCoroutine(SwitchState("Idle"));
        }

    }
    void OnTriggerStay(Collider other)
    {
        if (other.GetComponent<TrafficLightBehaviour>().color == "green")
        {
            // If the traffic light is green, allow the pedestrian to cross
            pedestrianAgent.isStopped = false;
            waitingForLight = false;
            StartCoroutine(SwitchState("Walking"));
        }
        
    }
}
