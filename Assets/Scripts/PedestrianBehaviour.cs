/*
* Author: Emilie Tee Jing Hui
* Date: 16/8/2025
* Description: FSM behaviour for pedestrian NPCs
*/
using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using TMPro;
using System;
using Unity.VisualScripting;

public class PedestrianBehaviour : MonoBehaviour
{
    /// <summary>
    /// Reference to the main camera transform
    /// </summary>
    Transform MainCamera;
    /// <summary>
    /// Reference to the player UI canvas transform
    /// </summary>
    Transform playerUICanvas;
    /// <summary>
    /// Reference to the textbox prefab for displaying dialogue
    /// </summary>
    [SerializeField] GameObject textboxPrefab;
    /// <summary>
    /// Reference to the NavMeshAgent component for pathfinding
    /// </summary>

    NavMeshAgent pedestrianAgent;
    /// <summary>
    /// Reference to the target transform for the pedestrian
    /// </summary>
    Transform targetTransform;
    /// <summary>
    /// The current state of the pedestrian NPC
    /// </summary>
    public string currentState;
    /// <summary>
    /// Reference to the array of possible end points for the pedestrian
    /// </summary>

    [SerializeField] Transform[] endPoint;
    /// <summary>
    /// The index of the current end point for the pedestrian
    /// </summary>
    public int endPointIndex = 0;
    /// <summary>
    /// Indicates whether the pedestrian is waiting for the traffic light to change.
    /// </summary>

    bool waitingForLight = false;
    /// <summary>
    /// Indicates whether the pedestrian is currently talking.
    /// </summary>
    bool isTalking = false;
    /// <summary>
    /// Reference to the array of background NPC lines for dialogue.
    /// </summary>
    public string[] backgroundNPCLines;
    /// <summary>
    /// Reference to the dialogue box prefab for displaying NPC dialogue.
    /// </summary>
    GameObject myDialogue= null;
    /// <summary>
    /// Reference to the text box position for displaying dialogue.
    /// </summary>
    [SerializeField] float textOffset = 1.5f;
    /// <summary>
    /// Reference to the text box position for displaying dialogue.
    /// </summary>
    [SerializeField] 
    Transform textBoxPosition;
    
    /// <summary>
    /// atLight is a boolean that indicates whether the pedestrian is currently at a traffic light.
    /// </summary>
    bool atLight = false;
    /// <summary>
    /// Initializes the pedestrian behaviour.
    /// </summary>
    void Awake()
    {
        MainCamera = Camera.main.transform;

        backgroundNPCLines = new string[] {
            "Sorry! I can't talk right now. I'm busy!",
            "I'm just a background character.",
            "...",
            "Hey! I'm walking here!"
        };

        pedestrianAgent = GetComponent<NavMeshAgent>();
        pedestrianAgent.speed = UnityEngine.Random.Range(2f, 4f);

        StartCoroutine(SwitchState("Idle"));
    }
    /// <summary>
    /// Switches the current state of the pedestrian.
    /// </summary>
    

    IEnumerator SwitchState(string newState)
    {
        if (currentState == newState) yield break;
        currentState = newState;
        StartCoroutine(newState);
    }
    /// <summary>
    /// Coroutine for the idle state of the pedestrian.
    /// </summary>
    IEnumerator Idle()
    {
        while (currentState == "Idle")
        {
            pedestrianAgent.ResetPath();
            if (!(Mathf.Floor(transform.position.x) == Mathf.Floor(endPoint[endPointIndex].position.x) && Mathf.Floor(transform.position.z) == Mathf.Floor(endPoint[endPointIndex].position.z)))
            {
                if (!isTalking && !waitingForLight)
                {
                    yield return new WaitForSeconds(2f);
                    StartCoroutine(SwitchState("Walking"));
                    yield break;
                }

                yield return null;
            }
            else
            {
                Debug.Log("Pedestrian reached endpoint: " + endPoint[endPointIndex].name);
                endPointIndex +=1;
                if (endPointIndex >= endPoint.Length)
                {
                    endPointIndex = 0;
                    pedestrianAgent.Warp(endPoint[0].position);
                }
                yield return null;

            }
        }
    }
    /// <summary>
    /// Coroutine for the walking state of the pedestrian.
    /// </summary>
    IEnumerator Walking()
    {
        while (currentState == "Walking")
        {
            pedestrianAgent.SetDestination(endPoint[endPointIndex].position);

            if ((Mathf.Floor(transform.position.x) == Mathf.Floor(endPoint[endPointIndex].position.x) && Mathf.Floor(transform.position.z) == Mathf.Floor(endPoint[endPointIndex].position.z)))
            {
                StartCoroutine(SwitchState("Idle"));
                yield break;
            }

            yield return null;
        }
    }
    /// <summary>
    /// Called when the pedestrian enters a trigger collider.
    /// </summary>

    void OnTriggerEnter(Collider other)
    {
        atLight = true;
        if (other.CompareTag("TrafficLight") && other.GetComponent<PedestrianTrafficLightBehaviour>() != null)
        {
            var light = other.GetComponent<PedestrianTrafficLightBehaviour>();
            if (light.color == "red" || light.color == "flashing")
            {
                
                waitingForLight = true;
                StartCoroutine(SwitchState("Idle"));
            }
        }
    }
    /// <summary>
    /// Called when the pedestrian exits a trigger collider.
    /// </summary>
    void OnTriggerExit(Collider other)
    {
        atLight = false;
    }
    /// <summary>
    /// Called when the pedestrian is within a trigger collider.
    /// </summary>
    void OnTriggerStay(Collider other)
    {

        var light = other.GetComponent<PedestrianTrafficLightBehaviour>();
        if (light != null && light.color == "green")
        {
            waitingForLight = false;
            StartCoroutine(SwitchState("Walking"));
        }
    }
    /// <summary>
    /// Coroutine for showing dialogue state
    /// </summary>
    public IEnumerator ShowDialogue()
    {
        if (isTalking) yield break; // Prevent overlapping dialogues
        if (!atLight)
        {
            StartCoroutine(SwitchState("Idle"));
        }
        isTalking = true;
        playerUICanvas = GameObject.FindWithTag("UI").transform;
        int randomIndex = UnityEngine.Random.Range(0, backgroundNPCLines.Length);
        GameObject myDialogue = Instantiate(textboxPrefab, playerUICanvas);
        myDialogue.GetComponent<Textbox>().textboxCreated(); // Ensure the textbox is initialized
        myDialogue.transform.SetParent(playerUICanvas, false);
        myDialogue.GetComponentInChildren<TextMeshProUGUI>().text = backgroundNPCLines[randomIndex];
        myDialogue.transform.SetSiblingIndex(0); // Ensure dialogue is on top of other UI elements
        Debug.Log("Updating dialogue position");
        StartCoroutine(stopTalking(myDialogue));
        while (isTalking)
        {
            Vector3 screenPoint = Camera.main.WorldToScreenPoint(textBoxPosition.position);
            
            float distance = Vector3.Distance(MainCamera.position, gameObject.transform.position);
            float scale = 4/distance; // Scale based on distance from camera
            myDialogue.transform.localScale = new Vector3(scale, scale, scale);

            myDialogue.transform.position = screenPoint;
            // disable the dialogue if the screen point is off the screen
            if (screenPoint.x < 0 || screenPoint.x > Screen.width ||
                screenPoint.y < 0 || screenPoint.y > Screen.height)
            {
                myDialogue.SetActive(false);
            }
            else
            {
                myDialogue.SetActive(true);
            }
            
            if (screenPoint.z < 0)
            {
                myDialogue.SetActive(false);
            }
            yield return null;
        }

        Destroy(myDialogue);
        yield break;
    }
    /// <summary>
    /// Coroutine for stopping dialogue.
    /// </summary>
    IEnumerator stopTalking(GameObject myDialogue)
    {
        yield return new WaitForSeconds(3f);
        isTalking = false;
        Destroy(myDialogue);
    }
    

}
