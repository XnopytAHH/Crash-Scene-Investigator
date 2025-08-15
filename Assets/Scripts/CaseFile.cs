/*
* Author: Lim En Xu Jayson
* Date: 15/8/2025
* Description: Code to manage the case file UI and interactions.
*/
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
    /// culpritDropdown is a reference to the TMP_Dropdown component that displays the list of culprits.
    /// </summary>
    [SerializeField] private TMP_Dropdown culpritDropdown;
    /// <summary>
    /// accidentPhoto is a reference to the Image that displays the accident photo.
    /// </summary>
    [SerializeField] private Image accidentPhoto;
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
        // Set the description text of the evidence object
        evidenceObject.GetComponent<EvidencePhoto>().evidenceImage = evidence.evidenceImage; // Assuming EvidencePhoto has a Texture2D field for the image
        evidenceObject.GetComponent<EvidencePhoto>().evidenceName = evidence.evidenceName; // Assuming EvidencePhoto has a string field for the name
        evidenceObject.GetComponent<EvidencePhoto>().descriptionText = evidence.evidenceDescription; // Assuming EvidencePhoto has a TextMeshProUGUI component for description
        // Logic to add evidence to the case file
        Debug.Log("Evidence added: " + evidence.evidenceName);
    }
    /// <summary>
    /// Clears all evidence from the case file.
    /// </summary>
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
    /// <summary>
    /// Updates the details of the case file.
    /// </summary>
    public void UpdateDetails(string title, string date, string culprit1, string culprit2, Sprite accidentPhoto)
    {
        ClearEvidence(); // Clear existing evidence
        fileTitle.text = title; // Update the case file title
        dateText.text = date; // Update the case file date
        culpritDropdown.options.Clear();
        culpritDropdown.options.Add(new TMP_Dropdown.OptionData("Select Culprit")); // Add a default option
        culpritDropdown.options.Add(new TMP_Dropdown.OptionData(culprit1));
        culpritDropdown.options.Add(new TMP_Dropdown.OptionData(culprit2));
        culpritDropdown.value = 0; // Set the default selected option
        Debug.Log("Case file details updated.");

    }
    /// <summary>
    /// Update runs every frame, checking which button should be active depending on the scene.
    /// </summary>
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
        else if (SceneManager.GetActiveScene().name == "Tutorial")
        {
            toOffice.gameObject.SetActive(true); // Hide the toOffice button
            toLevel.gameObject.SetActive(false); // Hide the toLevel button
        }
        else
        {
            toOffice.gameObject.SetActive(false); // Hide the toOffice button
            toLevel.gameObject.SetActive(false); // Hide the toLevel button
        }
    }
}
