/*
* Author: Lim En Xu Jayson
* Date: 16/8/2025
* Description: Handles the behavior of evidence photos collected in game.
*/
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class EvidencePhoto : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    /// <summary>
    /// Prefab for the zoomed evidence view.
    /// </summary>
    [SerializeField]
    GameObject zoomedEvidencePrefab;
    /// <summary>
    /// Evidence description text to prompt the player for clues
    /// </summary>
    public string descriptionText;
    /// <summary>
    /// Image of the evidence captured by evidence camera.
    /// </summary>
    public Texture2D evidenceImage; 
    /// <summary>
    /// Name of the evidence.
    /// </summary>
    public string evidenceName;
    /// <summary>
    /// Reference to the zoomed evidence view GameObject created upon hovering
    /// </summary>
    GameObject zoomedEvidence;
    /// <summary>
    /// Detects if the mouse hovers over a evidence photo, triggers spawning of the zoomed evidence view.
    /// </summary>
    public void OnPointerEnter(PointerEventData eventData)
    {
        
        // Show the zoomed evidence view
        zoomedEvidence = Instantiate(zoomedEvidencePrefab);
        zoomedEvidence.transform.SetParent(GameObject.FindWithTag("CaseFileUI").transform); // Set the parent to the canvas
        zoomedEvidence.transform.SetAsLastSibling();
        zoomedEvidence.transform.position = gameObject.transform.position; // Position it at the same location as the evidence photo
        zoomedEvidence.transform.GetChild(0).GetComponent<Image>().sprite = Sprite.Create(evidenceImage, new Rect(0, 0, evidenceImage.width, evidenceImage.height), new Vector2(0.5f, 0.5f));
        zoomedEvidence.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = evidenceName;
        zoomedEvidence.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = descriptionText;
    }
    /// <summary>
    /// Updates the position of the zoomed evidence view to follow the evidence photo.
    /// </summary>
    void Update()
    {
        if (zoomedEvidence != null)
        {
            zoomedEvidence.transform.position = gameObject.transform.position; // Keep it at the same location as the evidence photo
        }
    }
    /// <summary>
    /// Detects if the mouse exits the evidence photo, triggers destruction of the zoomed evidence view.
    /// </summary>
    public void OnPointerExit(PointerEventData eventData)
    {
        if (zoomedEvidence != null)
        {
            Destroy(zoomedEvidence);
        }
    }
}
