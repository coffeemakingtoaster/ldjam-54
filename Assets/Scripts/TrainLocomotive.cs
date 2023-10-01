using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TrainLocomotive : MonoBehaviour
{
    public TrainTrack currentTrainTrack;
    public float SPEED = 5f;
    public bool isDrivingInReverse = false;

    List<Vector3> unvisitedWaypoints = new List<Vector3>();
    List<Vector3> visitedWaypoints = new List<Vector3>();

    void Update()
    {
        // Dont move or do anything if there is no currentTrack
        if (currentTrainTrack == null)
        {
            return;
        }

        //  Current track was just set
        if (currentTrainTrack != null && unvisitedWaypoints.Count == 0 && visitedWaypoints.Count == 0)
        {
            Debug.Log("Setting waypoints");
            SetWaypointsFromCurrentTrack();
        }

        // This occurs when there is no following track
        if (unvisitedWaypoints.Count < 1)
        {
            SetNextTrainTrack();
            return;
        }

        Vector3 targetPoint = unvisitedWaypoints[0];
        if (Vector3.Distance(transform.position, targetPoint) < 0.01f)
        {
            // Set waypoint to visited
            visitedWaypoints.Add(targetPoint);
            unvisitedWaypoints.RemoveAt(0);

            // Reached the end of track 
            // Therefore => Need new track
            if (unvisitedWaypoints.Count == 0)
            {
                Debug.Log("Finding next");
                SetNextTrainTrack();
            }
        }
        else
        {
            //  if (isInTrackTile && nextTrainTrack.isCurve)
            //  {
            //      transform.RotateAround(nextTrainTrack.rotatePoint.transform.position, Vector3.up, -50f * Time.deltaTime);
            //      rotatedAngle += Mathf.Abs(-10f * Time.deltaTime);
            //  }
            //  else
            //  {
            Vector3 lookVector = (targetPoint - transform.position).normalized;
            Quaternion toRotation = Quaternion.LookRotation(lookVector);
            transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, Time.deltaTime);
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, targetPoint, SPEED * Time.deltaTime);
            //}
        }
    }

    private void SetWaypointsFromCurrentTrack()
    {
        float startDistance = Vector3.Distance(transform.position, currentTrainTrack.WayPoints[0]);
        float endDistance = Vector3.Distance(transform.position, currentTrainTrack.WayPoints.Last());
        visitedWaypoints = new List<Vector3>();
        unvisitedWaypoints = new List<Vector3>(currentTrainTrack.WayPoints);
        //Debug.Log("Startdistance: "+startDistance.ToString() + " Enddistance: " + endDistance.ToString());
        // start is closer => no reverse
        if (startDistance > endDistance)
        {
            unvisitedWaypoints.Reverse();
            return;
        }
        currentTrainTrack.TryToFindAdjacent();
    }

    void SetNextTrainTrack()
    {
        Debug.Log("Finding next track");
        float shortestDistance = 0.05f;
        TrainTrack closestTrainTrack = null;
        List<Vector3> futureWaypoints = new List<Vector3>();

        currentTrainTrack.TryToFindAdjacent();

        foreach (TrainTrack possibleTrainTrack in currentTrainTrack.ConnectedTracks)
        {
            // This would just create a loop
            if (possibleTrainTrack.Equals(currentTrainTrack))
            {
                continue;
            }

            float startDistance = Vector3.Distance(transform.position, possibleTrainTrack.WayPoints[0]);
            float endDistance = Vector3.Distance(transform.position, possibleTrainTrack.WayPoints.Last());
            Debug.Log("Startdistance: " + startDistance.ToString() + " Enddistance: " + endDistance.ToString());
            // Shortest distance to first waypoint of track            
            if (startDistance < shortestDistance)
            {
                shortestDistance = startDistance;
                closestTrainTrack = possibleTrainTrack;
                futureWaypoints = new List<Vector3>(possibleTrainTrack.WayPoints);
            }

            // Shortest distance is to last waypoint of track => reverse
            if (endDistance < shortestDistance)
            {
                shortestDistance = startDistance;
                closestTrainTrack = possibleTrainTrack;
                futureWaypoints = new List<Vector3>(possibleTrainTrack.WayPoints);
                futureWaypoints.Reverse();
            }
        }

        if (closestTrainTrack == null)
        {
            Debug.LogWarning("No valid next track for train");
            return;
        }
        currentTrainTrack = closestTrainTrack;
        // Clear all visited waypoints
        visitedWaypoints = new List<Vector3>();
        unvisitedWaypoints = futureWaypoints;
        currentTrainTrack.TryToFindAdjacent();
    }
}
