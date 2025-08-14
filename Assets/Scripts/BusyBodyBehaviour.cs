
using UnityEngine;
using System.Collections;
using UnityEngine.AI;


public class BusyBodyBehaviour : MonoBehaviour
{
    NavMeshAgent pedestrianAgent;
    Transform targetTransform;

    public string currentState;

    [SerializeField] Transform[] endPoint;
    public int endPointIndex = 0;

    bool waitingForLight = false;

    [SerializeField] GameObject targetEvidence = null;

    public GameObject heldEvidence = null;
    [SerializeField] Transform holdPosition = null;

    Coroutine stateRoutine;

    void Awake()
    {
        pedestrianAgent = GetComponent<NavMeshAgent>();
        UpdateTargetTransform();
        ChangeState("Idle");
    }


    void ChangeState(string newState)
    {
        if (currentState == newState)
            return;

        currentState = newState;

        if (stateRoutine != null)
            StopCoroutine(stateRoutine);

        stateRoutine = StartCoroutine(newState);
    }

    void UpdateTargetTransform()
    {
        if (endPoint.Length == 0) return;
        endPointIndex = Mathf.Clamp(endPointIndex, 0, endPoint.Length - 1);
        targetTransform = endPoint[endPointIndex];
    }


    IEnumerator Idle()
    {
        pedestrianAgent.isStopped = true;

        while (currentState == "Idle")
        {
            pedestrianAgent.ResetPath();

            // Not at endpoint
            if (!(Mathf.Approximately(transform.position.x, targetTransform.position.x) && Mathf.Approximately(transform.position.z, targetTransform.position.z)))
            {
                if (!waitingForLight)
                {
                    Debug.Log("Switching to walk after 2s");
                    yield return new WaitForSeconds(2f);
                    ChangeState("Walking");
                }
            }
            else
            {
                endPointIndex = (endPointIndex + 1);
                if (endPointIndex >= endPoint.Length)
                {
                    endPointIndex = 0;
                    if (heldEvidence == null && targetEvidence != null)
                    {
                        ChangeState("Blocking");
                    }
                }
                UpdateTargetTransform();
            }

            yield return null;
        }
    }

    IEnumerator Walking()
    {
        pedestrianAgent.isStopped = false;

        while (currentState == "Walking")
        {
            if (targetTransform != null)
                pedestrianAgent.SetDestination(targetTransform.position);
            // Arrived at endpoint
            if (Mathf.Approximately(transform.position.x, targetTransform.position.x) && Mathf.Approximately(transform.position.z, targetTransform.position.z))
                ChangeState("Idle");

            yield return null;
        }
    }

    IEnumerator Blocking()
    {
        pedestrianAgent.isStopped = false; // ensure movement is enabled
        StartCoroutine(BlockingTimer());
        while (currentState == "Blocking")
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            Debug.Log("Blocking state active");
            if (player != null)
            {
                // Midpoint between player and evidence
                if (player.transform.position == null || targetEvidence == null)
                {
                    Debug.LogWarning("Player or target evidence not found, returning to Idle state.");
                    ChangeState("Idle");
                    yield break;
                }
                Vector3 midpoint = (player.transform.position + targetEvidence.transform.position) * 0.5f;

                // Offset toward the evidence (so NPC guards it)
                Vector3 toEvidence = (targetEvidence.transform.position - player.transform.position).normalized;
                Vector3 blockPos = midpoint + toEvidence * 0.5f;
               
                pedestrianAgent.SetDestination(blockPos);
                yield return null;
            }
        }
    
    
}

    IEnumerator BlockingTimer()
    {
        yield return new WaitForSeconds(10f);
        ChangeState("Stealing");
    }

    IEnumerator Stealing()
    {
        while (currentState == "Stealing")
        {
            if (targetEvidence == null)
            {
                ChangeState("Idle");
                yield break;
            }

            pedestrianAgent.SetDestination(targetEvidence.transform.position);

            if (Mathf.Approximately(transform.position.x, targetEvidence.transform.position.x) && Mathf.Approximately(transform.position.z, targetEvidence.transform.position.z))
            {
                Debug.Log("Stealing evidence: " + targetEvidence.name);
                heldEvidence = targetEvidence;
                targetEvidence.GetComponent<Rigidbody>().isKinematic = true;
                targetEvidence.GetComponent<Outline>().enabled = true;
                targetEvidence.GetComponent<Outline>().OutlineColor= Color.red;
                targetEvidence.transform.position = holdPosition.position;
                targetEvidence.transform.SetParent(holdPosition);

                ChangeState("Idle");
            }

            yield return null;
        }
    }
    
    public void DropItem()
    {
        if (heldEvidence != null)
        {
            heldEvidence.GetComponent<Rigidbody>().isKinematic = false;
            heldEvidence.transform.SetParent(null);
            targetEvidence = null;
            heldEvidence = null;
        }
    }
}
