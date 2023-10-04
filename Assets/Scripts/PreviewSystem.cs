using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreviewSystem : MonoBehaviour
{
    [SerializeField]
    private float previewYOffset = 0.01f;

    [SerializeField]
    private GameObject cellIndicator;
    private GameObject previewObject;

    [SerializeField]
    private Material previewMaterialsPrefab;
    private Material previewMaterialInstance;

    private Renderer cellIndicatorRenderer;

    private int currentPrevRot = 0;

    private void Start()
    {
        previewMaterialInstance = new Material(previewMaterialsPrefab);
        cellIndicator.SetActive(false);
        cellIndicatorRenderer = cellIndicator.GetComponentInChildren<Renderer>();
    }

    public void StartShowingPlacementPreview(GameObject prefab, Vector2Int size)
    {
        previewObject = Instantiate(prefab);
        previewObject.layer = LayerMask.NameToLayer("Ignore Raycast");
        PreparePreview(previewObject);
        PrepareCursor(size);
        rotate(currentPrevRot);
        cellIndicator.SetActive(true);
        //resetRotation();
    }

    private void PrepareCursor(Vector2Int size)
    {
        if (size.x > 0 || size.y > 0)
        {
        // cellIndicator.transform.localScale = new Vector3(size.x, 1, size.y);
            cellIndicatorRenderer.material.mainTextureScale = size;
        }
    }

    private void PreparePreview(GameObject previewObject)
    {
        Renderer[] renderers = previewObject.GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in renderers)
        {
            Material[] materials = renderer.materials;
            for (int i = 0; i < materials.Length; i++)
            {
                materials[i] = previewMaterialInstance;
            }
            renderer.materials = materials;
        }
        

    }

    public void StopShowingPreview()
    {
        cellIndicator.SetActive(false);
        Destroy(previewObject);
    }

    public void UpdatePosition(Vector3 position, bool validity, int rotation,Vector2Int size)
    {
        MovePreview(position,rotation,size);
        MoveCursor(position);
        ApplyFeedback(validity);
    }

    public void resetRotation(int rotation)
    {
        //Debug.Log(rotation);
        //previewObject.transform.RotateAround(previewObject.transform.Find("Center").transform.position, Vector3.up, -currentPrevRot);
        currentPrevRot = rotation;
    }
    public void rotate(int rotation)
    {
        currentPrevRot += 90;
        if (currentPrevRot == 360)
        {
            currentPrevRot = 0;
        }
        
        previewObject.transform.RotateAround(previewObject.transform.Find("Center").transform.position, Vector3.up, rotation);
    }

    private void MovePreview(Vector3 position,int rotation,Vector2Int size)
    {
        float addx = 0;
        float addz = 0;
        
        if(rotation == 90)
        {
            addz = 0.1f *size.x;
        }
        if (rotation == 180)
        {
            addx = 0.1f * size.x;
            addz = 0.1f * size.y;
        }
        if (rotation == 270)
        {
            addx = 0.1f * size.y;
            
        }
        
        previewObject.transform.position = new Vector3(position.x+addx, position.y, position.z+addz);
        
    }

    private void MoveCursor(Vector3 position)
    {
        cellIndicator.transform.position = position;
    }

    private void ApplyFeedback(bool validity)
    {
        Color c = validity ? Color.green : Color.red;
        cellIndicatorRenderer.material.color = c;
        c.a = 0.5f;
        previewMaterialInstance.color = c;
    }
}
