using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class NPCBehavior : MonoBehaviour
{
    
    [SerializeField]
    public Dialogue[] dialogueLines; // Array of Dialogue objects containing the lines for this NPC
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Dialogue getNPCLines(int dialogueIndex)
    {
        if (dialogueIndex < dialogueLines.Length)
        {
            return dialogueLines[dialogueIndex];
        }
        return null;
    }
    void Update()
    {
        Vector3 directiontoplayer = (GameObject.FindWithTag("Player").transform.position - gameObject.transform.position)*-1;
        directiontoplayer.y = 0; // Keep the NPC facing horizontally
        gameObject.transform.rotation = Quaternion.LookRotation(directiontoplayer);
    }
}
