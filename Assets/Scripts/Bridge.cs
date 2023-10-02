using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bridge : MonoBehaviour
{
    public Material newMaterial; // Drag and drop the new material in the Unity Editor
    private Material originalMaterial;
    public Vector2Int start;
    public Vector2Int end;
    int funds;
    GameStats gameStats;

    private void Start()
    {

        gameStats = FindObjectOfType<GameStats>();
        funds = gameStats.getCurrentFunds();
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
        funds = gameStats.getCurrentFunds();
        Debug.Log(funds);
        if (funds < 300)
        {
            return;
        }

        gameStats.addFunds(-300);
        for (int i = 0; i < MapArray.map.GetLength(0); i++)
        {
            for (int j = 0; j < MapArray.map.GetLength(1); j++)
            {
                if (34-i >= start.x && 34-i <= end.x && 49-j >= start.y && 49-j <= end.y)
                {
                    Debug.Log(i);
                    Debug.Log(j);
                    MapArray.map[i, j] = 1;
                }
                
            }
        }
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
