using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using System;

public class TrainTrack : MonoBehaviour
{
    public List<Vector3> WayPoints;

    public GameObject BaseWayPoint;
    public float GRID_SIZE = 0.1f;
    public List<TrainTrack> ConnectedTracks;

    public List<TrackSwitch> ConnectedSwitches;

    public int MaxConnectedTracks;

    public bool isInSwitch = false;

    void Start()
    {
        // Search game objects
        foreach (Transform child in transform)
        {
            if (child.name == "WayPoint" && child.gameObject != BaseWayPoint)
            {
                //Debug.Log("Waypoint found");
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
        if (transform.gameObject.name == "LeftToRight" || transform.gameObject.name == "StraightToRight" || transform.gameObject.name == "StraightToLeft")
        {
            isInSwitch = true;
        }
    }

    public void TryToFindAdjacent()
    {

        CleanupConnectedTracks();

        // No tracks to find here
        if ((ConnectedTracks.Count + ConnectedSwitches.Count) >= MaxConnectedTracks)
        {
            return;
        }

        // No reference point
        if (WayPoints.Count == 0)
        {
            return;
        }

        if (isInSwitch)
        {
            TrackSwitch parentSwitch = gameObject.transform.parent.GetComponent<TrackSwitch>();
            parentSwitch.TryToFindAdjacent();
            ConnectedTracks = new List<TrainTrack>(parentSwitch.ConnectedTracks);
            ConnectedSwitches = new List<TrackSwitch>(parentSwitch.ConnectedSwitches);
            return;
        }

        TrainTrack[] trainTracks = FindObjectsOfType<TrainTrack>();
        foreach (TrainTrack externalTrainTrack in trainTracks)
        {
            if (System.Object.Equals(this, externalTrainTrack))
            {
                //Debug.Log("Self");
                continue;
            }

            if (externalTrainTrack.isInSwitch || isInSwitch)
            {
                continue;
            }

            if (externalTrainTrack.gameObject.layer == LayerMask.NameToLayer("Ignore Raycast"))
            {
                //Debug.Log("Visualization");
                continue;
            }

            // 0.23 as Sqrt(0.1 ** 2 + 0.2 ** 2) = 0.22 
            // This occurs when the item is flipped 180 degrees in comparison to this one 
            if (Vector3.Distance(transform.position, externalTrainTrack.transform.position) > 0.23f)
            {
                //Debug.Log("Not adjacent");
                continue;
            }

            List<float> distances = new List<float>
            {
                Vector3.Distance(WayPoints.Last(), externalTrainTrack.WayPoints[0]),
                Vector3.Distance(WayPoints[0], externalTrainTrack.WayPoints.Last()),
                Vector3.Distance(WayPoints[0], externalTrainTrack.WayPoints[0]),
                Vector3.Distance(WayPoints.Last(), externalTrainTrack.WayPoints.Last())
            };

            // Get lowest distance to of any entry or exit of current to any entry of exit of other
            float distance = distances.ToArray().Min();

            if (distance < 0.01f)
            {
                AddTrackToConnected(externalTrainTrack);
                externalTrainTrack.AddTrackToConnected(this);
            }
        }
        //Debug.Log("Now connected to " + ConnectedTracks.Count);

        TrackSwitch[] trackSwitches = FindObjectsOfType<TrackSwitch>();
        foreach (TrackSwitch externalTrackSwitch in trackSwitches)
        {

            if (externalTrackSwitch.gameObject.layer == LayerMask.NameToLayer("Ignore Raycast"))
            {
                //Debug.Log("Visualization");
                continue;
            }

            // 0.23 as Sqrt(0.1 ** 2 + 0.2 ** 2) = 0.22 
            // This occurs when the item is flipped 180 degrees in comparison to this one 
            if (Vector3.Distance(transform.position, externalTrackSwitch.transform.position) > 0.23f)
            {
                //Debug.Log("Not adjacent");
                continue;
            }

            // IDEA:
            // Get connected trackswitches and request the active traintrack for a site when needed
            float distance = Mathf.Min(
                Vector3.Distance(WayPoints[0], externalTrackSwitch.Left.transform.position),
                Vector3.Distance(WayPoints.Last(), externalTrackSwitch.Left.transform.position),
                Vector3.Distance(WayPoints[0], externalTrackSwitch.Right.transform.position),
                Vector3.Distance(WayPoints.Last(), externalTrackSwitch.Right.transform.position),
                Vector3.Distance(WayPoints[0], externalTrackSwitch.Straight.transform.position),
                Vector3.Distance(WayPoints.Last(), externalTrackSwitch.Straight.transform.position)
            );

            if (distance < 0.01f)
            {
                if (!ConnectedSwitches.Contains(externalTrackSwitch))
                {
                    ConnectedSwitches.Add(externalTrackSwitch);
                }
                if (!externalTrackSwitch.ConnectedTracks.Contains(this))
                {
                    externalTrackSwitch.ConnectedTracks.Add(this);
                }
                // TODO: When train is in switch it needs to be able to find adjacent tracks and other switches
            }
        }
    }

    public void CleanupConnectedTracks()
    {
        foreach (TrainTrack trainTrack in ConnectedTracks)
        {
            try
            {
                string _ = trainTrack.transform.gameObject.name;
            }
            catch (Exception _)
            {
                // Remove deleted item
                ConnectedTracks.Remove(trainTrack);
            }
        }
        foreach (TrackSwitch trainSwitch in ConnectedSwitches)
        {
            try
            {
                string _ = trainSwitch.transform.gameObject.name;
            }
            catch (Exception _)
            {
                // Remove deleted item
                ConnectedSwitches.Remove(trainSwitch);
            }
        }
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
