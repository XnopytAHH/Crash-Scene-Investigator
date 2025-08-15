/*
* Author: Hazel Wang Sim Yee
* Date: 15/8/2025
* Description: FSM for car NPC behavior
*/
using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using TMPro;

public class CarBehaviour : MonoBehaviour
{
    /// <summary>
    /// Reference to the NavMeshAgent component.
    /// </summary>
    NavMeshAgent carAgent;
    /// <summary>
    /// Reference to the target transform (the next endpoint).
    /// </summary>
    Transform targetTransform;
    /// <summary>
    /// Reference to the current state of the car.
    /// </summary>
    string currentState;
    /// <summary>
    /// Reference to the car's renderer.
    /// </summary>
    Renderer carRenderer;
    /// <summary>
    /// Reference to the car's endpoint transform.
    /// </summary>
    [SerializeField] Transform endPoint;
    /// <summary>
    /// Reference to the car's starting point transform.
    /// </summary>
    [SerializeField] Transform startPoint;
    /// <summary>
    /// Indicates whether the car is waiting for a traffic light to change.
    /// </summary>
    bool waitingForLight = false;
    /// <summary>
    /// Indicates whether the car is waiting for the player.
    /// </summary>
    bool waitingForPlayer = false;
    /// <summary>
    /// Indicates whether the car is waiting for another car.
    /// </summary>
    bool waitingForCar = false;
    

    /// <summary>
    /// Indicates whether the car is currently at a traffic light.
    /// </summary>
    bool atLight = false;
    /// <summary>
    /// Reference to the car's honk sound.
    /// </summary>
    [SerializeField]
    AudioClip honkSound;
    /// <summary>
    /// isTalking is a reference to if the car is talking. Leftover code from an experiment that did not work
    /// </summary>
    bool isTalking = false;
    /// <summary>
    /// Reference to the car's audio source.
    /// </summary>
    AudioSource audioSource;
    /// <summary>
    /// Initializes the car's behavior.
    /// </summary>
    void Awake()
    {
        carAgent = GetComponent<NavMeshAgent>();
        carAgent.speed = Random.Range(7f,8.5f );
        carRenderer = transform.GetChild(0).GetComponent<Renderer>();
        carRenderer.material.color = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
        StartCoroutine(SwitchState("Idle"));
    }
    /// <summary>
    /// Switches the car's state.
    /// </summary>
    IEnumerator SwitchState(string newState)
    {
        if (currentState == newState) yield break;
        currentState = newState;
        StartCoroutine(newState);
    }
    /// <summary>
    /// Coroutine for the Idle state.
    /// </summary>
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
    /// <summary>
    /// Coroutine for the Driving state.
    /// </summary>
    IEnumerator Driving()
    {
        while (currentState == "Driving")
        {
            if (Mathf.Approximately(carAgent.transform.position.x, endPoint.position.x) && Mathf.Approximately(carAgent.transform.position.z, endPoint.position.z))
            {
                Debug.Log("Car has reached the endpoint.");

                carAgent.Warp(startPoint.position);
                carRenderer.material.color = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
                carAgent.speed = Random.Range(6f,8.5f );
                yield return new WaitForSeconds(Random.Range(1f, 5f));
                StartCoroutine(SwitchState("Idle"));
                yield break;
            }
            carAgent.SetDestination(endPoint.position);
            yield return null;
        }
    }
    /// <summary>
    /// Called when the car enters a trigger collider.
    /// </summary>
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
    /// <summary>
    /// Called when the car is within a trigger collider.
    /// </summary>
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
    /// <summary>
    /// Called when the car exits a trigger collider.
    /// </summary>
    void OnTriggerExit(Collider other)
    {

        if (other.CompareTag("Player"))
        {

            waitingForPlayer = false;
            StartCoroutine(SwitchState("Driving"));
        }
    }
}
