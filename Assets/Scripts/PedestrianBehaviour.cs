using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using TMPro;

public class PedestrianBehaviour : MonoBehaviour
{
    Transform MainCamera;
    Transform playerUICanvas;
    [SerializeField] GameObject textboxPrefab;

    NavMeshAgent pedestrianAgent;
    Transform targetTransform;
    string currentState;

    [SerializeField] Transform endPoint;

    bool waitingForLight = false;
    bool isTalking = false;

    public string[] backgroundNPCLines;
    GameObject myDialogue= null;
    [SerializeField] float textOffset = 1.5f; // Offset for the
    [SerializeField] 
    Transform textBoxPosition;
    
    /// <summary>
    /// atLight is a boolean that indicates whether the pedestrian is currently at a traffic light.
    /// </summary>
    bool atLight = false;
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
        pedestrianAgent.speed = Random.Range(2f, 4f);

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
            pedestrianAgent.ResetPath();
            if (!isTalking && !waitingForLight &&
                (transform.position.x != endPoint.position.x || transform.position.z != endPoint.position.z))
            {
                yield return new WaitForSeconds(2f);
                StartCoroutine(SwitchState("Walking"));
                yield break;
            }

            yield return null;
        }
    }

    IEnumerator Walking()
    {
        while (currentState == "Walking")
        {
            pedestrianAgent.SetDestination(endPoint.position);

            if (Vector3.Distance(transform.position, endPoint.position) < 0.5f)
            {
                StartCoroutine(SwitchState("Idle"));
                yield break;
            }

            yield return null;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        atLight = true;
        if (other.CompareTag("TrafficLight"))
        {
            var light = other.GetComponent<TrafficLightBehaviour>();
            if (light.color == "red" || light.color == "blinking")
            {
                
                waitingForLight = true;
                StartCoroutine(SwitchState("Idle"));
            }
        }
    }
    void OnTriggerExit(Collider other)
    {
        atLight = false;
    }

    void OnTriggerStay(Collider other)
    {

        var light = other.GetComponent<TrafficLightBehaviour>();
        if (light != null && light.color == "green")
        {
            waitingForLight = false;
            StartCoroutine(SwitchState("Walking"));
        }
    }

    public IEnumerator ShowDialogue()
    {
        if (isTalking) yield break; // Prevent overlapping dialogues
        if (!atLight)
        {
            StartCoroutine(SwitchState("Idle"));
        }
        isTalking = true;
        playerUICanvas = GameObject.FindWithTag("UI").transform;
        int randomIndex = Random.Range(0, backgroundNPCLines.Length);
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
            yield return null;
        }

        Destroy(myDialogue);
        yield break;
    }

    IEnumerator stopTalking(GameObject myDialogue)
    {
        yield return new WaitForSeconds(3f);
        isTalking = false;
        Destroy(myDialogue);
    }
    

}
