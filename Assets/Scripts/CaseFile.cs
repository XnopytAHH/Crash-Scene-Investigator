using System.ComponentModel;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;
public class CaseFile : MonoBehaviour
{
    /// <summary>
    /// evidencePrefab is a reference to the prefab that will be used to display evidence in the case file.
    /// </summary>
    [SerializeField] private GameObject evidencePrefab;
    /// <summary>
    /// FileTitle is a reference to the TextMeshProUGUI component that displays the title of the case file.
    /// </summary>
    [SerializeField] private TextMeshProUGUI fileTitle;
    /// <summary>
    /// DateText is a reference to the TextMeshProUGUI component that displays the date of the case file.
    /// </summary>
    [SerializeField] private TextMeshProUGUI dateText;
    /// <summary>
    /// toOffice is a reference to the button that will take the player back to the office scene.
    /// </summary>
    [SerializeField] private Button toOffice;
    /// <summary>
    /// toLevel is a reference to the button that will take the player to the current level.
    /// </summary>
    [SerializeField] private Button toLevel;
    /// <summary>
    /// AddEvidence is a method that adds evidence to the case file.
    /// </summary>
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
    public void ClearEvidence()
    {
        GameObject evidenceCanvas = GameObject.Find("EvidenceCanvas"); // Find the evidence canvas
        if (evidenceCanvas.transform.childCount != 0)
        {
            foreach (Transform child in evidenceCanvas.transform) // Iterate through all children of the evidence canvas
            {
                Destroy(child.gameObject); // Destroy each child object
            }
        }
        Debug.Log("All evidence cleared from the case file.");
    }
    public void UpdateDetails(string title, string date)
    {
        ClearEvidence(); // Clear existing evidence
        fileTitle.text = title; // Update the case file title
        dateText.text = date; // Update the case file date
        Debug.Log("Case file details updated.");

    }
    void Update()
    {
        if (SceneManager.GetActiveScene().name == "office" && !GameManager.Instance.beenToLevel) // Check if the active scene is the office
        {
           
            toOffice.gameObject.SetActive(false); // Hide the toOffice button
            toLevel.gameObject.SetActive(true); // Show the toLevel button
            
        }
        else if (SceneManager.GetActiveScene().name == "Level " + GameManager.Instance.currentLevel)
        {
            
            toOffice.gameObject.SetActive(true); // Show the toOffice button
            toLevel.gameObject.SetActive(false); // Hide the toLevel button
           
        }
        else
        {
            toOffice.gameObject.SetActive(false); // Hide the toOffice button
            toLevel.gameObject.SetActive(false); // Hide the toLevel button
        }
    }
}
