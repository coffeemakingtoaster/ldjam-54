using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlacementSystem : MonoBehaviour
{
    [SerializeField]
    GameObject mouseIndicator;
    [SerializeField]
    private InputManager inputManager;
    [SerializeField]
    private Grid grid;
    [SerializeField]
    private ObjectsDatabaseSO database;

    [SerializeField]
    private KeyCode turnRight;
    [SerializeField]
    private KeyCode turnLeft;

    private int selectedObjectIndex = -1;

    [SerializeField]
    private GameObject gridVisualization;

    private GridData floorData, furnitureData;

    [SerializeField]
    private PreviewSystem preview;

    private Vector3Int lastDetectedPosition = Vector3Int.zero;

    private List<GameObject> placedGameObject = new List<GameObject>();

    private int rotation;

    

    private void Start()
    {
        StopPlacement();
        floorData = new();
        furnitureData = new();
    }

    public void StartPlacement(int ID)
    {
        StopPlacement();
        

        selectedObjectIndex = database.objectsData.FindIndex(data => data.ID == ID);
        if (selectedObjectIndex < 0)
        {
            Debug.LogError($"No ID found{ID}");
            return;
        }
        gridVisualization.SetActive(true);
        preview.StartShowingPlacementPreview(database.objectsData[selectedObjectIndex].Prefab, database.objectsData[selectedObjectIndex].Size);
        preview.resetRotation(rotation);
        inputManager.OnClicked += PlaceStructure;
        inputManager.OnExit += StopPlacement;
    }

    private void PlaceStructure()
    {
        if (inputManager.IsPointerOverUI())
        {
            return;
        }
        Vector3 mousePosition = inputManager.GetSelectedMapPosition();
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);
        //print(gridPosition);

        bool placementValidity = CheckPlacementValidity(gridPosition, selectedObjectIndex);
        if (placementValidity == false)
        {
            return;
        }

        GameObject newObject = Instantiate(database.objectsData[selectedObjectIndex].Prefab);
        
        newObject.transform.RotateAround(newObject.transform.Find("Center").transform.position, Vector3.up, rotation);
        float addx = 0;
        float addz = 0;
        Vector2Int newSize = database.objectsData[selectedObjectIndex].Size;
        if (rotation == 90)
        {
            newSize = new Vector2Int(newSize.y, newSize.x);
            addz = 0.1f * database.objectsData[selectedObjectIndex].Size.x;
        }
        if (rotation == 180)
        {
            addx = 0.1f * database.objectsData[selectedObjectIndex].Size.x;
            addz = 0.1f * database.objectsData[selectedObjectIndex].Size.y;
        }
        if (rotation == 270)
        {
            newSize = new Vector2Int(newSize.y, newSize.x);
            addx = 0.1f * database.objectsData[selectedObjectIndex].Size.y;

        }
        Vector3 worldPos = grid.CellToWorld(gridPosition);
        newObject.transform.position = new Vector3(worldPos.x+addx,worldPos.y,worldPos.z+addz);
        placedGameObject.Add(newObject);
        GridData selectedData = furnitureData;
        //GridData selectedData = database.objectsData[selectedObjectIndex].ID == 0 ? floorData : furnitureData;
        selectedData.AddObject(gridPosition, newSize, database.objectsData[selectedObjectIndex].ID, placedGameObject.Count - 1, database.objectsData[selectedObjectIndex].Prefab);
        //rotation = 0;
        //preview.resetRotation();
        preview.UpdatePosition(grid.CellToWorld(gridPosition), false,rotation, database.objectsData[selectedObjectIndex].Size);
    }

    private bool CheckPlacementValidity(Vector3Int gridPosition, int selectedObjectIndex)
    {
        GridData selectedData = database.objectsData[selectedObjectIndex].ID == 0 ? floorData : furnitureData;
        return selectedData.CanPlaceObjectAt(gridPosition, database.objectsData[selectedObjectIndex].Size);
    }

    private void StopPlacement()
    {
        selectedObjectIndex = -1;
        gridVisualization.SetActive(false);
        
        preview.StopShowingPreview();
        inputManager.OnClicked -= PlaceStructure;
        inputManager.OnExit -= StopPlacement;
        lastDetectedPosition = Vector3Int.zero;
    }

    private void Update()
    {
        
        if (selectedObjectIndex < 0)
        {
            return;
        }
        

        Vector3 mousePosition = inputManager.GetSelectedMapPosition();
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);

        
        if (Input.GetKeyDown(turnRight))
        {
            Debug.Log(mousePosition);
            bool placementValidity = CheckPlacementValidity(gridPosition, selectedObjectIndex);
            preview.rotate(90);
            
            rotation += 90;
            if(rotation == 360)
            {
                rotation = 0;
            }
            preview.UpdatePosition(grid.CellToWorld(gridPosition), placementValidity,rotation, database.objectsData[selectedObjectIndex].Size);
            
        }
        
        if (lastDetectedPosition != gridPosition)
        {
            
            bool placementValidity = CheckPlacementValidity(gridPosition, selectedObjectIndex);

            mouseIndicator.transform.position = mousePosition;
            preview.UpdatePosition(grid.CellToWorld(gridPosition), placementValidity,rotation, database.objectsData[selectedObjectIndex].Size);
            lastDetectedPosition = gridPosition;
        }

        
        
    }
}
