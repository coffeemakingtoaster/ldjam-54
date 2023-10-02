using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class TrackSwitch : MonoBehaviour
{
    public TrainTrack RightToLeft;
    public TrainTrack StraightToRight;

    public TrainTrack LeftToRight;

    public TrainTrack StraightToLeft;


    public void Start(){
        LeftToRight.CanBeTraversedBothWays = false;
        RightToLeft.CanBeTraversedBothWays = false;
        // The default value, idk
        SetActive(StraightToLeft);
    }


    //public void OnMouseDown(){
    //    ToggleActive();
    //}

    private void ToggleActive(){
        if (StraightToLeft.enabled){
            SetActive(StraightToRight);
            return;
        }
        SetActive(StraightToLeft);
    }

    private void SetActive(TrainTrack trainTrack){
        if (trainTrack == StraightToLeft){

            // Straight
            LeftToRight.PreDeleteHook();
            LeftToRight.transform.gameObject.SetActive(false);
            RightToLeft.transform.gameObject.SetActive(true);
            RightToLeft.TryToFindAdjacent();


            StraightToRight.PreDeleteHook();
            StraightToRight.transform.gameObject.SetActive(false);
            StraightToLeft.transform.gameObject.SetActive(true);
            StraightToLeft.CanBeTraversedBothWays = false;
        }else{

            // Straight
            RightToLeft.PreDeleteHook();
            RightToLeft.transform.gameObject.SetActive(false);
            LeftToRight.transform.gameObject.SetActive(true);
            LeftToRight.TryToFindAdjacent();

            StraightToLeft.PreDeleteHook();
            StraightToLeft.transform.gameObject.SetActive(false);
            StraightToRight.transform.gameObject.SetActive(true);
            StraightToRight.CanBeTraversedBothWays = false;
        }
    }
}
