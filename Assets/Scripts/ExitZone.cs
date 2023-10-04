using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ExitZone : MonoBehaviour
{

    GameStats gameStats;

    void Start(){
        gameStats = FindAnyObjectByType<GameStats>();
    }

    void OnTriggerEnter(Collider other){
        TrainWagon wagon = other.gameObject.GetComponent<TrainWagon>();
        TrainLocomotive locomotive = other.gameObject.GetComponent<TrainLocomotive>();
        // If not train or wagon return
        if ( wagon == null && locomotive == null){
            return;
        }

        if (locomotive != null){
            //Debug.LogWarning("teleporting");
            locomotive.transform.position = transform.position;
        }
        if (wagon != null){
            if ( wagon.payload != null && wagon.payload.GetComponent<SellValue>() != null){
                gameStats.addFunds(wagon.payload.GetComponent<SellValue>().Value);
            }
        }
        Destroy(other.gameObject, 1);
    }
}
