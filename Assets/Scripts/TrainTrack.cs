using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class TrainTrack : MonoBehaviour
{
    public List<Vector3> WayPoints;

    public GameObject BaseWayPoint;
    public float GRID_SIZE = 0.1f;
    public List<TrainTrack> ConnectedTracks;

    public int MaxConnectedTracks;

    public bool CanBeTraversedBothWays = true;

    public List<TrainTrack> BlackListedTrainTracks;

    void Start()
    {
        // Search game objects
        foreach (Transform child in transform)
        {
            if (child.name == "WayPoint" && child.gameObject != BaseWayPoint)
            {
                Debug.Log("Waypoint found");
                WayPoints.Add(child.transform.position);
            }
        }
        WayPoints.Insert(0, BaseWayPoint.transform.position);
        // Order by furthest away from basepoint
        // Allows locomotive to simply traverse the list in (reversed) order
        if (WayPoints.Count > 2)
        {
            WayPoints.OrderBy((vec) => Vector3.Distance(vec, BaseWayPoint.transform.position));
        }
        CanBeTraversedBothWays = true;
        Debug.Log(transform.gameObject.name);
        if (transform.gameObject.name == "LeftToRight" || transform.gameObject.name == "RightToLeft")
        {
            CanBeTraversedBothWays = false;
        }
    }

    public void TryToFindAdjacent()
    {
        // No tracks to find here
        if (ConnectedTracks.Count >= MaxConnectedTracks)
        {
            return;
        }

        // No reference point
        if (WayPoints.Count == 0)
        {
            return;
        }

        TrainTrack[] trainTracks = FindObjectsOfType<TrainTrack>();
        foreach (TrainTrack externalTrainTrack in trainTracks)
        {
            if (System.Object.Equals(this, externalTrainTrack))
            {
                Debug.Log("Self");
                continue;
            }

            if (BlackListedTrainTracks.Contains(externalTrainTrack))
            {
                Debug.LogWarning("Blacklisted");
                continue;
            }

            if (!externalTrainTrack.CanBeTraversedBothWays){
                externalTrainTrack.TryToFindAdjacent();
                return;
            }

            if (externalTrainTrack.gameObject.layer == LayerMask.NameToLayer("Ignore Raycast"))
            {
                Debug.Log("Visualization");
                continue;
            }

            // 0.23 as Sqrt(0.1 ** 2 + 0.2 ** 2) = 0.22 
            // This occurs when the item is flipped 180 degrees in comparison to this one 
            if (Vector3.Distance(transform.position, externalTrainTrack.transform.position) > 0.23f)
            {
                Debug.Log("Not adjacent");
                continue;
            }

            List<float> distances = new List<float>(){
                Vector3.Distance(WayPoints.Last(), externalTrainTrack.WayPoints[0]),
            };

            if (!CanBeTraversedBothWays)
            {
                // Add to previous
                // Dont add to next
                // Check if external track is BEFORE or AFTER current one way
                // Before -> Add to external connected tracks, dont add to own
                // After -> Add to own connected tracks, dont add to external
                float startDistance = Mathf.Min(
                        Vector3.Distance(WayPoints[0], externalTrainTrack.WayPoints[0]),
                        Vector3.Distance(WayPoints[0], externalTrainTrack.WayPoints[1])
                    );
                float endDistance = Mathf.Min(
                        Vector3.Distance(WayPoints[1], externalTrainTrack.WayPoints[0]),
                        Vector3.Distance(WayPoints[1], externalTrainTrack.WayPoints[1])
                    );
                bool isCheckingPrevious = startDistance < endDistance;
                float dst = Mathf.Min(startDistance, endDistance);
                if(dst < 0.01f){
                    if(isCheckingPrevious){
                        externalTrainTrack.AddTrackToConnected(this);
                    }else{
                        AddTrackToConnected(externalTrainTrack);
                    }
                }
                continue;
            }

            distances.Add(Vector3.Distance(WayPoints[0], externalTrainTrack.WayPoints.Last()));
            distances.Add(Vector3.Distance(WayPoints[0], externalTrainTrack.WayPoints[0]));
            distances.Add(Vector3.Distance(WayPoints.Last(), externalTrainTrack.WayPoints.Last()));

            // Get lowest distance to of any entry or exit of current to any entry of exit of other
            float distance = distances.ToArray().Min();

            if (distance < 0.01f)
            {
                AddTrackToConnected(externalTrainTrack);
                externalTrainTrack.AddTrackToConnected(this);
            }
        }
        Debug.Log("Now connected to " + ConnectedTracks.Count);
    }

    public void ValidateConnectedTracks()
    {
        // TODO: implement me
        return;
    }

    public void PreDeleteHook()
    {
        // Remove self from other items
        foreach (TrainTrack connectedTrainTrack in ConnectedTracks)
        {
            connectedTrainTrack.ConnectedTracks.Remove(this);
        }
    }


    public void AddTrackToConnected(TrainTrack otherTrack)
    {
        // No duplicates
        if (ConnectedTracks.Contains(otherTrack))
        {
            return;
        }
        ConnectedTracks.Add(otherTrack);
    }
}
