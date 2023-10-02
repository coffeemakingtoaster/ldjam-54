using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainSpawner : MonoBehaviour
{
    public float SpawnRate = 10f;

    public GameObject InitialCargo;

    public int WagonAmount = 4;

    public bool hasLocomotiveInsideIt = false;

    public GameObject SpawnPoint;

    public GameObject SecondarySpawnPoint;

    public GameObject LocomotivePrefab;

    public GameObject WagonPrefab;

    public TrainTrack firstTrainTrack;

    private IEnumerator coroutine;


    public void Start()
    {
        coroutine = SpawnTrain();
        StartCoroutine(coroutine);
    }

    IEnumerator SpawnTrain()
    {
        while (true)
        {
            if (!hasLocomotiveInsideIt)
            {
                GameObject locomotive = Instantiate(LocomotivePrefab, SpawnPoint.transform.position, Quaternion.Euler(0,180,0));
                locomotive.GetComponent<TrainLocomotive>().currentTrainTrack = firstTrainTrack;
                Vector3 offset = SecondarySpawnPoint.transform.position - SpawnPoint.transform.position;
                Debug.Log(offset);
                GameObject previous = locomotive;
                for (int i = 0; i < WagonAmount; i++){
                    BallCoupling endCoupling = previous.transform.Find("EndCoupling").gameObject.GetComponent<BallCoupling>();
                    Debug.Log(endCoupling.GetPosition());
                    previous = Instantiate(WagonPrefab, locomotive.transform.position + offset *  i , Quaternion.identity);
                    previous.GetComponent<TrainWagon>().PreviousBallCoupling = endCoupling;
                    if (InitialCargo){
                        previous.GetComponent<TrainWagon>().payload = InitialCargo;
                    }
                }
            } else{
                Debug.Log("Not spawning train as previous train has not left yet");
            }
            yield return new WaitForSeconds(SpawnRate);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        TrainLocomotive locomotive = other.gameObject.GetComponent<TrainLocomotive>();
        if (locomotive == null)
        {
            return;
        }
        hasLocomotiveInsideIt = true;
    }

    void OnTriggerExit(Collider other)
    {
        TrainLocomotive locomotive = other.gameObject.GetComponent<TrainLocomotive>();
        if (locomotive == null)
        {
            return;
        }
        hasLocomotiveInsideIt = false;
    }

}
