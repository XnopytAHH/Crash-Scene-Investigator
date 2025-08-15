/*
* Author: Lim En Xu Jayson
* Date: 15/8/2025
* Description: barrierMaterial handles the material properties of the barriers in the game.
*/
using UnityEngine;
using UnityEngine.UIElements;


public class barrierMaterial : MonoBehaviour
{
    /// <summary>
    /// Reference to the player's transform.
    /// </summary>
    public Transform player;
    /// <summary>
    /// Reference to the material applied to the barriers.
    /// </summary>
    public Material mat;
    /// <summary>
    /// Reference to the base color of the material.
    /// </summary>
    private Color baseColor;
    /// <summary>
    /// Reference to the colliders of the wall. It is a list to accommodate all 4 walls.
    /// </summary>
    private Collider[] wallCollider;
    /// <summary>
    /// Reference to the distance between the player and the closest wall.
    /// </summary>
    private float distance;
    /// <summary>
    /// Start method to initialize the wall colliders.
    /// </summary>
    void Start()
    {
        wallCollider = GetComponentsInChildren<MeshCollider>();
    }
    /// <summary>
    /// Update method to check the distance between the player and the walls every frame.
    /// </summary>
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