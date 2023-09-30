using System.Collections;
using System.Collections.Generic;
using UnityEditor.UI;
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

    void Start()
    {
        if (nextTrainTrack == null)
        {
            this.CheckForNext();
        }
    }

    void Update()
    {

    }

    void CheckForNext()
    {
        TrainTrack[] trainTracks = FindObjectsOfType<TrainTrack>();

        foreach (TrainTrack trainTrack in trainTracks)
        {
            if (Object.Equals(this, trainTrack)){
                Debug.Log("Self");
                continue;
            }
            // Trains cannot turn on the spot
            if (Object.Equals(previousTrainTrack, trainTrack)){
                continue;
            }
            // Is too far away?
            if (Vector3.Distance(this.transform.position, trainTrack.transform.position) > GRID_SIZE)
            {
                Debug.Log("Too far away");
                Debug.Log(Vector3.Distance(this.transform.position, trainTrack.transform.position));
                continue;
            }
            Debug.Log("Found it!");
            nextTrainTrack = trainTrack;
            nextTrainTrack.SetPreviousTrainTrack(this);
            return;
        }
        Debug.Log("None found");
    }

    public TrainTrack GetNextTrainTrack()
    {
        return nextTrainTrack;
    }

    public void SetPreviousTrainTrack(TrainTrack track){
        previousTrainTrack = track;
    }
}
