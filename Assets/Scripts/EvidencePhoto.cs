using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class EvidencePhoto : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    GameObject zoomedEvidencePrefab;
    public string descriptionText;
    public Texture2D evidenceImage; // Image of the evidence
    public string evidenceName; // Name of the evidence
    GameObject zoomedEvidence;
    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("Mouse entered evidence photo: " + evidenceName);
        // Show the zoomed evidence view
        zoomedEvidence = Instantiate(zoomedEvidencePrefab);
        zoomedEvidence.transform.SetParent(GameObject.FindWithTag("CaseFileUI").transform); // Set the parent to the canvas
        zoomedEvidence.transform.SetAsLastSibling();
        zoomedEvidence.transform.position = gameObject.transform.position; // Position it at the same location as the evidence photo
        zoomedEvidence.transform.GetChild(0).GetComponent<Image>().sprite = Sprite.Create(evidenceImage, new Rect(0, 0, evidenceImage.width, evidenceImage.height), new Vector2(0.5f, 0.5f));
        zoomedEvidence.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = evidenceName;
        zoomedEvidence.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = descriptionText;
    }
    void Update()
    {
        if (zoomedEvidence != null)
        {
            zoomedEvidence.transform.position = gameObject.transform.position; // Keep it at the same location as the evidence photo
        }
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        if (zoomedEvidence != null)
        {
            Destroy(zoomedEvidence);
        }
    }
}
