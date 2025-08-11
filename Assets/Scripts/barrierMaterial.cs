
using UnityEngine;
using UnityEngine.UIElements;


public class barrierMaterial : MonoBehaviour
{
    public Transform player;

    public Material mat;
    private Color baseColor;
    private Collider[] wallCollider;
    private float distance;
    void Start()
    {
        wallCollider = GetComponentsInChildren<MeshCollider>();
    }

    void Update()
    {
        for (int i = 0; i < wallCollider.Length; i++)
        {
            Vector3 closestpoint = wallCollider[i].ClosestPoint(player.position);
            float newdistance = Vector3.Distance(closestpoint, player.position);
            if (i == 0 || newdistance < distance)
            {
                distance = newdistance;
            }
        }
        if (distance >= mat.GetFloat("_RevealRadius"))
        {
            mat.SetFloat("_PlayerDistance", distance);
        }
        else
        {
            mat.SetFloat("_PlayerDistance", 0f);
        }
    }
}