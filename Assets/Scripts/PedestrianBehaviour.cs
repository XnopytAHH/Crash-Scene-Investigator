using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using UnityEngine.Rendering;
using System.Diagnostics;
using Unity.VisualScripting;
using UnityEngine.UIElements;
using TMPro;
public class PedestrianBehaviour : MonoBehaviour
{
    Transform MainCamera;
    [SerializeField]
    Transform playerUICanvas;
    [SerializeField]
    GameObject textboxPrefab;
    NavMeshAgent pedestrianAgent;
    Transform targetTransform;
    string currentState;
    [SerializeField]
    Transform endPoint;
    bool waitingForLight = false;
    public string[] backgroundNPCLines;
    void Awake()
    {
        MainCamera = Camera.main.transform;

        backgroundNPCLines = new string[] { "Sorry! I can't talk right now. I'm busy!", "I'm just a background character.", "...", "Hey! I'm walking here!" };
        pedestrianAgent = GetComponent<NavMeshAgent>();
        pedestrianAgent.speed = Random.Range(2f, 4f);
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
    public IEnumerator ShowDialogue()
    {
        // This method can be used to show dialogue when the pedestrian is interacted with
        // For now, it just prints a random line from the backgroundNPCLines array
        int randomIndex = Random.Range(0, backgroundNPCLines.Length);
        GameObject myDialogue = Instantiate(textboxPrefab, playerUICanvas);
        myDialogue.transform.SetParent(playerUICanvas, false);
        myDialogue.GetComponentInChildren<TextMeshProUGUI>().text = backgroundNPCLines[randomIndex];
        UnityEngine.Debug.Log(backgroundNPCLines[randomIndex]);
        yield return new WaitForSeconds(3f); // Show the dialogue for 3 seconds
        Destroy(myDialogue); // Destroy the dialogue box after showing it
        yield return null; // Ensure the coroutine yields to allow other processes to run
    }
}
