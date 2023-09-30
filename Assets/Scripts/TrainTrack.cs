using System;
using UnityEngine;

public class TrainTrack : MonoBehaviour
{
    public TrainTrack nextTrainTrack;
    public TrainTrack previousTrainTrack;

    public GameObject entryPoint;

    public GameObject exitPoint;

    public GameObject rotatePoint;

    public bool isCurve;
    public float GRID_SIZE = 0.1f;

    public bool flipsDirection = false;

    public void TryToConnectToTrainTracks()
    {
        TrainTrack[] trainTracks = FindObjectsOfType<TrainTrack>();
        foreach (TrainTrack externalTrainTrack in trainTracks)
        {
            if (System.Object.Equals(this, externalTrainTrack))
            {
                Debug.Log("Self");
                continue;
            }

            Debug.Log("Own: Entrypoint: \t" + entryPoint.transform.position.ToString() + "\t ExitPoint: " + exitPoint.transform.position.ToString());
            Debug.Log("Other: Entrypoint: \t" + transform.TransformPoint(externalTrainTrack.entryPoint.transform.position).ToString() + "\t ExitPoint: " + transform.TransformPoint(externalTrainTrack.exitPoint.transform.position).ToString());
            Debug.Log(Vector3.Distance(entryPoint.transform.position, externalTrainTrack.exitPoint.transform.position));
            if (Vector3.Distance(entryPoint.transform.position, externalTrainTrack.exitPoint.transform.position) < 0.01f)
            {
                // Is not already connected to something else
                if (externalTrainTrack.nextTrainTrack == null)
                {
                    Debug.Log("Connecting forward");
                    previousTrainTrack = externalTrainTrack;
                    externalTrainTrack.nextTrainTrack = this;
                }
                continue;
            }

            // Is traintrack before the current?                
            Debug.Log(Vector3.Distance(exitPoint.transform.position, externalTrainTrack.entryPoint.transform.position));
            if (Vector3.Distance(exitPoint.transform.position, externalTrainTrack.entryPoint.transform.position) < 0.01f)
            {
                // Is not already connected to something else
                if (externalTrainTrack.previousTrainTrack == null)
                {
                    Debug.Log("Connecting backwards");
                    nextTrainTrack = externalTrainTrack;
                    externalTrainTrack.previousTrainTrack = this;
                }
                continue;
            }

            // Are traintracks butt to butt?
            Debug.Log(Vector3.Distance(exitPoint.transform.position, externalTrainTrack.exitPoint.transform.position));
            if (Vector3.Distance(exitPoint.transform.position, externalTrainTrack.exitPoint.transform.position) < 0.01f)
            {
                if (externalTrainTrack.nextTrainTrack == null)
                {
                    Debug.Log("Flipping");
                    nextTrainTrack = externalTrainTrack;
                    externalTrainTrack.nextTrainTrack = this;
                    flipsDirection = true;
                    externalTrainTrack.flipsDirection = true;
                }
            }

            // Are traintracks mouth to mouth?
            Debug.Log(Vector3.Distance(entryPoint.transform.position, externalTrainTrack.entryPoint.transform.position));
            if (Vector3.Distance(entryPoint.transform.position, externalTrainTrack.entryPoint.transform.position) < 0.01f)
            {
                if (externalTrainTrack.previousTrainTrack == null)
                {
                    Debug.Log("Flipping");
                    previousTrainTrack = externalTrainTrack;
                    externalTrainTrack.previousTrainTrack = this;
                    flipsDirection = true;
                    externalTrainTrack.flipsDirection = true;
                }
            }

            Debug.Log("No Match");
        }
    }

    public TrainTrack GetNextTrainTrack()
    {
        return nextTrainTrack;
    }

    public TrainTrack GetPreviousTrainTrack(){
        return previousTrainTrack;
    }

    public void SetPreviousTrainTrack(TrainTrack track)
    {
        previousTrainTrack = track;
    }
}
