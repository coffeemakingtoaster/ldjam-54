using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridData
{
    Dictionary<Vector3Int, PlacementData> placedObjects = new();

    public void AddObject(Vector3Int gridPosition, Vector2Int objectSize, int ID, int placedObjectIndex, GameObject placedObject)
    {
        List<Vector3Int> positionToOccupy = CalculatePositions(gridPosition, objectSize);
        PlacementData data = new PlacementData(positionToOccupy, ID, placedObjectIndex, placedObject);
        foreach (var position in positionToOccupy)
        {
            if (placedObjects.ContainsKey(position))
            {
                throw new Exception($"Dictionary already contains this cell position: {position}");
            }
            placedObjects[position] = data;
        }
    }

    private List<Vector3Int> CalculatePositions(Vector3Int gridPosition, Vector2Int objectSize)
    {
        List<Vector3Int> returnVal = new();
        for (int x = 0; x < objectSize.x; x++)
        {
            for (int y = 0; y < objectSize.y; y++)
            {
                returnVal.Add(gridPosition + new Vector3Int(x, 0, y));
            }
        }
        return returnVal;
    }

    public bool CanPlaceObjectAt(Vector3Int gridPosition, Vector2Int objectSize)
    {
        List<Vector3Int> positionToOccupy = CalculatePositions(gridPosition, objectSize);
        foreach (var pos in positionToOccupy)
        {
            
            if (MapArray.map[34 - pos.x,49 - pos.z] == 0)
            {
                return false;
            }
            if (placedObjects.ContainsKey(pos))
            {
                return false;
            }
        }
        return true;
    }

    public void RemoveObjectAtGridPosition(Vector3Int gridPosition,Vector2Int size)
    {
        List<Vector3Int> positionToOccupy = CalculatePositions(gridPosition, size);
        foreach (var position in positionToOccupy)
        {
            placedObjects.Remove(position);
        }

    }
    public GameObject GetObjectAtGridPosition(Vector3Int gridPosition)
    {
        List<Vector3Int> positionToOccupy = CalculatePositions(gridPosition, new Vector2Int(1,1));
        foreach (var pos in positionToOccupy)
        {
            if (placedObjects.ContainsKey(pos))
            {
                return placedObjects[pos].PlacedObject;
            }
        }
        return null;
    }
}
public class PlacementData
{
    public List<Vector3Int> occupiedPositions;

    public PlacementData(List<Vector3Int> occupiedPositions, int iD, int placedObjectIndex, GameObject placedObject)
    {
        this.occupiedPositions = occupiedPositions;
        ID = iD;
        PlacedObjectIndex = placedObjectIndex;
        PlacedObject = placedObject;
    }

    public int ID { get; private set; }
    public int PlacedObjectIndex { get; private set; }

    public GameObject PlacedObject { get; private set; }
}