using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ExitZone : MonoBehaviour
{

    void OnTriggerEnter(Collider other){
        TrainWagon wagon = other.gameObject.GetComponent<TrainWagon>();
        TrainLocomotive locomotive = other.gameObject.GetComponent<TrainLocomotive>();
        // If not train or wagon return
        if ( wagon == null && locomotive == null){
            return;
        }
        if (wagon != null){
           // Add funds 
           return;
        }
        Destroy(other.gameObject);
    }
}
