using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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

    private Vector2Int newSize;
    private float addx = 0;
    private float addz = 0;

    private GameStats gameStats;

    

    private void Start()
    {
        StopDelete();
        StopPlacement();
        floorData = new();
        furnitureData = new();
        gameStats = FindObjectOfType<GameStats>();
    }


    public void StartDelete()
    {
        StopDelete();
        StopPlacement();
        gridVisualization.SetActive(true);
        inputManager.OnClicked += DeleteStructure;
        inputManager.OnExit += StopDelete;
    }
    public void StartPlacement(int ID)
    {
        StopPlacement();
        StopDelete();

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

    private bool CanAfford(int objIndex){
        return database.objectsData[objIndex].Price <= gameStats.getCurrentFunds();
    }

    private void DeleteStructure()
    {
        if (inputManager.IsPointerOverUI())
        {
            return;
        }
        GameObject structure = inputManager.GetStructure();
        furnitureData.RemoveObjectAtGridPosition(structure.GetComponent<Stats>().gridPos, structure.GetComponent<Stats>().size);
        Destroy(structure);
    }

    private void PlaceStructure()
    {
        if (inputManager.IsPointerOverUI())
        {
            return;
        }
        Vector3 mousePosition = inputManager.GetSelectedMapPosition();
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);
        

        calcSize();

        bool placementValidity = CheckPlacementValidity(gridPosition, selectedObjectIndex,newSize);
        if (placementValidity == false)
        {
            return;
        }

        bool canAfford =  CanAfford(selectedObjectIndex);
        if (!canAfford){
            return;
        }


        GameObject newObject = Instantiate(database.objectsData[selectedObjectIndex].Prefab);
        
        newObject.transform.RotateAround(newObject.transform.Find("Center").transform.position, Vector3.up, rotation);
        
        Vector3 worldPos = grid.CellToWorld(gridPosition);
        newObject.transform.position = new Vector3(worldPos.x+addx,worldPos.y,worldPos.z+addz);
        placedGameObject.Add(newObject);
        GridData selectedData = furnitureData;
        //GridData selectedData = database.objectsData[selectedObjectIndex].ID == 0 ? floorData : furnitureData;
        newObject.GetComponent<Stats>().gridPos = gridPosition;
        newObject.GetComponent<Stats>().size = newSize;
        selectedData.AddObject(gridPosition, newSize, database.objectsData[selectedObjectIndex].ID, placedGameObject.Count - 1, database.objectsData[selectedObjectIndex].Prefab);
        //rotation = 0;
        //preview.resetRotation();
        preview.UpdatePosition(grid.CellToWorld(gridPosition), false,rotation, database.objectsData[selectedObjectIndex].Size);
        // Reduce player funds
        gameStats.spendFunds(database.objectsData[selectedObjectIndex].Price);
    }

    private bool CheckPlacementValidity(Vector3Int gridPosition, int selectedObjectIndex,Vector2Int size)
    {
        GridData selectedData = database.objectsData[selectedObjectIndex].ID == 0 ? floorData : furnitureData;
        return selectedData.CanPlaceObjectAt(gridPosition, size, database.objectsData[selectedObjectIndex].ID);
    }

    private void StopPlacement()
    {
        selectedObjectIndex = -1;
        gridVisualization.SetActive(false);
        
        preview.StopShowingPreview();
        inputManager.OnClicked -= PlaceStructure;
        inputManager.OnExit -= StopPlacement;
        
    }

    private void StopDelete()
    {
        
        gridVisualization.SetActive(false);

        preview.StopShowingPreview();
        inputManager.OnClicked -= DeleteStructure;
        inputManager.OnExit -= StopDelete;

    }

    private void calcSize()
    {
        addx = 0;
        addz = 0;
        newSize = database.objectsData[selectedObjectIndex].Size;
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
    }
    private void Update()
    {
        
        if (selectedObjectIndex < 0)
        {
            return;
        }
        

        Vector3 mousePosition = inputManager.GetSelectedMapPosition();
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);
        //Debug.Log(gridPosition);

        
        if (Input.GetKeyDown(turnRight))
        {
        
            calcSize();
            bool placementValidity = CheckPlacementValidity(gridPosition, selectedObjectIndex,newSize) && CanAfford(selectedObjectIndex);
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
            calcSize();
            bool placementValidity = CheckPlacementValidity(gridPosition, selectedObjectIndex,newSize) && CanAfford(selectedObjectIndex);;

            mouseIndicator.transform.position = mousePosition;
            preview.UpdatePosition(grid.CellToWorld(gridPosition), placementValidity,rotation, database.objectsData[selectedObjectIndex].Size);
            lastDetectedPosition = gridPosition;
        }

        
        
    }
}
