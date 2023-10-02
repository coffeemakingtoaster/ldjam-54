using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class TrackSwitch : MonoBehaviour
{
    public TrainTrack LeftToRight;
    public TrainTrack StraightToRight;

    public TrainTrack StraightToLeft;

    public GameObject Right;

    public GameObject Left;

    public GameObject Straight;

    public List<TrainTrack> ConnectedTracks;

    public List<TrackSwitch> ConnectedSwitches;

    public void Start()
    {
        // The default value, idk
        SetActive(StraightToLeft);
    }


    //public void OnMouseDown(){
    //    ToggleActive();
    //}

    private void ToggleActive()
    {
        if (StraightToLeft.enabled)
        {
            SetActive(StraightToRight);
            return;
        }
        SetActive(StraightToLeft);
    }

    private void SetActive(TrainTrack trainTrack)
    {
        if (trainTrack == StraightToLeft)
        {
            StraightToRight.transform.gameObject.SetActive(false);
            StraightToLeft.transform.gameObject.SetActive(true);
        }
        else
        {
            StraightToLeft.transform.gameObject.SetActive(false);
            StraightToRight.transform.gameObject.SetActive(true);
        }
    }

    public void TryToFindAdjacent(bool isRecursion = false)
    {

        TrainTrack[] trainTracks = FindObjectsOfType<TrainTrack>();
        foreach (TrainTrack externalTrainTrack in trainTracks)
        {
            if (System.Object.Equals(this, externalTrainTrack))
            {
                Debug.Log("Self");
                continue;
            }

            if (externalTrainTrack.isInSwitch)
            {
                continue;
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

            List<float> distances = new List<float>
            {
                Vector3.Distance(Left.transform.position, externalTrainTrack.WayPoints[0]),
                Vector3.Distance(Left.transform.position, externalTrainTrack.WayPoints.Last()),
                Vector3.Distance(Right.transform.position, externalTrainTrack.WayPoints[0]),
                Vector3.Distance(Right.transform.position, externalTrainTrack.WayPoints.Last()),
                Vector3.Distance(Straight.transform.position, externalTrainTrack.WayPoints[0]),
                Vector3.Distance(Straight.transform.position, externalTrainTrack.WayPoints.Last())
            };

            // Get lowest distance to of any entry or exit of current to any entry of exit of other
            float distance = distances.ToArray().Min();

            if (distance < 0.01f)
            {
                if (!ConnectedTracks.Contains(externalTrainTrack))
                {
                    ConnectedTracks.Add(externalTrainTrack);
                }
                if (!externalTrainTrack.ConnectedSwitches.Contains(this))
                {
                    externalTrainTrack.ConnectedSwitches.Add(this);
                }
            }
        }
        Debug.Log("Now connected to " + ConnectedTracks.Count);

        TrackSwitch[] trackSwitches = FindObjectsOfType<TrackSwitch>();
        foreach (TrackSwitch externalTrackSwitch in trackSwitches)
        {

            if(System.Object.Equals(this, externalTrackSwitch)){
                continue;
            }

            if (externalTrackSwitch.gameObject.layer == LayerMask.NameToLayer("Ignore Raycast"))
            {
                Debug.Log("Track only Visualization");
                continue;
            }

            // 0.23 as Sqrt(0.1 ** 2 + 0.2 ** 2) = 0.22 
            // This occurs when the item is flipped 180 degrees in comparison to this one 
            if (Vector3.Distance(transform.position, externalTrackSwitch.transform.position) > 0.23f)
            {
                Debug.Log("Track Not adjacent");
                continue;
            }

            // IDEA:
            // Get connected trackswitches and request the active traintrack for a site when needed
            float distance = Mathf.Min(
                Vector3.Distance(externalTrackSwitch.getClosestActiveTrainTrack(Left.transform.position).WayPoints[0], Left.transform.position),
                Vector3.Distance(externalTrackSwitch.getClosestActiveTrainTrack(Left.transform.position).WayPoints.Last(), Left.transform.position),
                Vector3.Distance(externalTrackSwitch.getClosestActiveTrainTrack(Right.transform.position).WayPoints[0], Right.transform.position),
                Vector3.Distance(externalTrackSwitch.getClosestActiveTrainTrack(Right.transform.position).WayPoints.Last(), Right.transform.position),
                Vector3.Distance(externalTrackSwitch.getClosestActiveTrainTrack(Straight.transform.position).WayPoints[0], Straight.transform.position),
                Vector3.Distance(externalTrackSwitch.getClosestActiveTrainTrack(Straight.transform.position).WayPoints.Last(), Straight.transform.position)
            );

            if (distance < 0.01f)
            {
                if (ConnectedSwitches.Contains(externalTrackSwitch))
                {
                    continue;
                }
                ConnectedSwitches.Add(externalTrackSwitch);
                if (!externalTrackSwitch.ConnectedSwitches.Contains(this))
                {
                    externalTrackSwitch.ConnectedSwitches.Add(this);
                }
            }
            else{
                Debug.Log("Track not close enough");
            }
        }
    }



    public TrainTrack getClosestActiveTrainTrack(Vector3 trainPosition)
    {
        GameObject closestObj = new List<GameObject>() { Left, Right, Straight }.OrderBy((obj) => Vector3.Distance(trainPosition, obj.transform.position)).First();
        Debug.LogWarning("Closest => "+closestObj.transform.name);
        if (closestObj == Straight)
        {
            if (StraightToRight.transform.gameObject.activeSelf)
            {
                return StraightToRight;
            }
            return StraightToLeft;
        }
        if (closestObj == Left)
        {
            if (StraightToLeft.transform.gameObject.activeSelf)
            {
                return StraightToLeft;
            }
        }
        if (closestObj == Right)
        {
            if (StraightToRight.transform.gameObject.activeSelf)
            {
                return StraightToRight;
            }
        }
        return LeftToRight;
    }
}
