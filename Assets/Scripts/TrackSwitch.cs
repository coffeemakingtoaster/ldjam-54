using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class TrackSwitch : MonoBehaviour
{
    public Dictionary<GameObject, TrainTrack> tracks = new Dictionary<GameObject, TrainTrack>();

    public GameObject[] entryPoints;

 /*
    void Start()
    {
        Debug.LogWarning("Start");
        // Set exit point for every entrypoint
        for (int i = 1; i <= entryPoints.Length; i++)
        {
            TrainTrack track = gameObject.AddComponent<TrainTrack>();
            track.entryPoint = entryPoints[i - 1];
            track.exitPoint = entryPoints[i % 2];
            track.isInSwitch = true;
            tracks[entryPoints[i - 1]] = track;
            Debug.LogWarning(track);
        }
        Debug.Log(tracks.Keys.ToArray().Length);
    }

    // Cannot flip direction

    /*
        private GameObject GetClosestEntrypoint(Vector3 position)
        {
            float shortestDistance = 0.1f;
            GameObject closestEntrypoint = null;
            foreach (GameObject entrypoint in tracks.Keys)
            {
                float currentDistance = Vector3.Distance(entrypoint.transform.position, position);
                if (currentDistance < shortestDistance)
                {
                    closestEntrypoint = entrypoint;
                    shortestDistance = currentDistance;
                }
            }
            return closestEntrypoint;
        }

        public TrainTrack GetNextTrainTrack(Vector3 previousTrackExitPosition)
        {
            return tracks[GetClosestEntrypoint(previousTrackExitPosition)];
        }

        public TrainTrack GetPreviousTrainTrack(Vector3 previousTrackExitPosition)
        {
            return tracks[GetClosestEntrypoint(previousTrackExitPosition)];
        }

    public void TryToConnectToTrainTracks()
    {
        Debug.LogWarning("Connecting");
        TrainTrack[] trainTracks = FindObjectsOfType<TrainTrack>();
        foreach (TrainTrack externalTrainTrack in trainTracks)
        {
            if (System.Object.Equals(this, externalTrainTrack))
            {
                Debug.Log("Self");
                continue;
            }

            // If is further than 0.1 away => no adjacent grid tile
            if (Vector3.Distance(transform.position, externalTrainTrack.transform.position) > 0.11f)
            {
                Debug.Log("Not adjacent");
                continue;
            }


            if (externalTrainTrack.gameObject.layer == LayerMask.NameToLayer("Ignore Raycast"))
            {
                Debug.Log("Visualization");
                continue;
            }

            foreach (TrainTrack traintrack in tracks.Values)
            {
                Debug.LogWarning(traintrack);
                // Start oftrack close to entry of swicth => This is previous
                Debug.Log(Vector3.Distance(externalTrainTrack.entryPoint.transform.position, traintrack.entryPoint.transform.position));
                if (Vector3.Distance(externalTrainTrack.entryPoint.transform.position, traintrack.entryPoint.transform.position) < 0.01f)
                {
                    Debug.Log("A");
                    externalTrainTrack.previousTrainTrack = traintrack;
                }
                Debug.LogWarning("Too far away");
                // Exit of track close to entry of switch => This is next
                Debug.Log(Vector3.Distance(externalTrainTrack.entryPoint.transform.position, traintrack.entryPoint.transform.position));
                if (Vector3.Distance(externalTrainTrack.exitPoint.transform.position, traintrack.entryPoint.transform.position) < 0.01f)
                {
                    Debug.Log("B");
                    Debug.Log(traintrack);
                    Debug.Log(externalTrainTrack);
                    externalTrainTrack.nextTrainTrack = traintrack;
                }
                Debug.Log("Too far away");
            }
        }
    }
    */
}
