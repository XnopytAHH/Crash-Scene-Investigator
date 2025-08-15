/*
* Author: Lim En Xu Jayson
* Date: 15/8/2025
* Description: FSM for busybody NPC behavior
*/
using UnityEngine;
using System.Collections;
using UnityEngine.AI;
using UnityEngine.VFX;

public class BusyBodyBehaviour : MonoBehaviour
{
    /// <summary>
    /// Reference to the NavMeshAgent component.
    /// </summary>
    NavMeshAgent pedestrianAgent;
    /// <summary>
    /// Reference to the target transform (the next endpoint).
    /// </summary>
    Transform targetTransform;
    /// <summary>
    /// Reference to the current state of the NPC.
    /// </summary>

    public string currentState;
    /// <summary>
    /// Array of transform points representing the NPC's path.
    /// </summary>

    [SerializeField] Transform[] endPoint;
    /// <summary>
    /// Index of the current endpoint in the array.
    /// </summary>
    public int endPointIndex = 0;
    /// <summary>
    /// Indicates whether the NPC is waiting for a traffic light to change.
    /// </summary>

    bool waitingForLight = false;
    /// <summary>
    /// Reference to the evidence object the NPC is tracking
    /// </summary>

    [SerializeField] GameObject targetEvidence = null;
    /// <summary>
    /// Reference to the evidence object the NPC is currently holding.
    /// </summary>

    public GameObject heldEvidence = null;
    /// <summary>
    /// Reference to the position where the evidence is held.
    /// </summary>
    [SerializeField] Transform holdPosition = null;
    /// <summary>
    /// Coroutine for managing the NPC's state.
    /// </summary>
    Coroutine stateRoutine;
    /// <summary>
    /// Reference to the visual effect played when the NPC is holding evidence.
    /// </summary>
    [SerializeField] VisualEffect evidenceEffect;

    /// <summary>
    /// Initializes the NPC's behavior.
    /// </summary>
    void Awake()
    {
        pedestrianAgent = GetComponent<NavMeshAgent>();
        UpdateTargetTransform();
        ChangeState("Idle");
    }

     /// <summary>
    /// Changes the current state of the NPC.
    /// </summary>
    void ChangeState(string newState)
    {
        if (currentState == newState)
            return;

        currentState = newState;

        if (stateRoutine != null)
            StopCoroutine(stateRoutine);

        stateRoutine = StartCoroutine(newState);
    }
    /// <summary>
    /// Updates the target transform for the NPC's movement.
    /// </summary>
    void UpdateTargetTransform()
    {
        if (endPoint.Length == 0) return;
        endPointIndex = Mathf.Clamp(endPointIndex, 0, endPoint.Length - 1);
        targetTransform = endPoint[endPointIndex];
    }

    /// <summary>
    /// Coroutine for the Idle state.
    /// </summary>
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

    /// <summary>
    /// Coroutine for the Walking state.
    /// </summary>
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

    /// <summary>
    /// Coroutine for the Blocking state.
    /// </summary>
    IEnumerator Blocking()
    {
        pedestrianAgent.isStopped = false; // ensure movement is enabled
        StartCoroutine(BlockingTimer());
        while (currentState == "Blocking")
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
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
    /// <summary>
    /// Coroutine for the Blocking Timer.
    /// </summary>
    IEnumerator BlockingTimer()
    {
        yield return new WaitForSeconds(10f);
        ChangeState("Stealing");
    }

    /// <summary>
    /// Coroutine for the Stealing state.
    /// </summary>
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
                heldEvidence = targetEvidence;
                targetEvidence.GetComponent<Rigidbody>().isKinematic = true;
                targetEvidence.GetComponent<Outline>().enabled = true;
                targetEvidence.GetComponent<Outline>().OutlineColor= Color.red;
                targetEvidence.transform.position = holdPosition.position;
                targetEvidence.transform.SetParent(holdPosition);
                evidenceEffect.Play();
                ChangeState("Idle");
            }

            yield return null;
        }
    }

    /// <summary>
    /// Drops the currently held evidence item.
    /// </summary>
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
