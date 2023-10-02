using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bridge : MonoBehaviour
{
    public Material newMaterial; // Drag and drop the new material in the Unity Editor
    private Material originalMaterial;

    private void Start()
    {
        // Store the original material of the object
        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            originalMaterial = renderer.material;
        }
        else
        {
            Debug.LogError("Renderer component not found!");
        }
    }

    private void OnMouseDown()
    {
        ChangeMaterial();
    }

    private void ChangeMaterial()
    {
        Renderer renderer = GetComponent<Renderer>();

        if (renderer != null && newMaterial != null)
        {
            renderer.material = newMaterial;
        }
        else
        {
            Debug.LogError("Renderer or new material is not set!");
        }
    }

    public void ResetMaterial()
    {
        // Reset the material to the original material
        Renderer renderer = GetComponent<Renderer>();

        if (renderer != null)
        {
            renderer.material = originalMaterial;
        }
        else
        {
            Debug.LogError("Renderer component not found!");
        }
    }
}
