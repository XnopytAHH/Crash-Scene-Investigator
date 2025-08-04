using System.ComponentModel;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class CaseFile : MonoBehaviour
{
    [SerializeField] private GameObject evidencePrefab;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void AddEvidence(Evidence evidence)
    {
        GameObject evidenceObject = Instantiate(evidencePrefab); // Instantiate the evidence prefab
        evidenceObject.name = evidence.evidenceName; // Set the name of the evidence object
        GameObject evidenceCanvas = GameObject.Find("EvidenceCanvas"); // Find the evidence canvas
        evidenceObject.transform.SetParent(evidenceCanvas.transform); // Set the parent to the case file
        evidenceObject.transform.localScale = Vector3.one; // Reset the scale of the evidence object
        evidenceObject.transform.localPosition = Vector3.zero; // Reset the position of the evidence object
        // Set the image of the evidence object
        Image evidenceImage = evidenceObject.transform.GetChild(0).GetComponent<Image>();
        evidenceImage.sprite = Sprite.Create(evidence.evidenceImage, new Rect(0, 0, evidence.evidenceImage.width, evidence.evidenceImage.height), new Vector2(0.5f, 0.5f));
        // Set the name text of the evidence object
        TextMeshProUGUI evidenceNameText = evidenceObject.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        evidenceNameText.text = evidence.evidenceName;
        // Logic to add evidence to the case file
        Debug.Log("Evidence added: " + evidence.evidenceName);
    }
}
