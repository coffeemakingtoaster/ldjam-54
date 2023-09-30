using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Machine : MonoBehaviour
{
    public Dictionary<GameObject, int> InInventory = new();
    public int InInventorySize;
    public int OutInventorySize;
    public Dictionary<GameObject, int> OutInventory = new();
    public Process[] processes;
    public Process activeProcess;
    public GameObject InInventoryPoint;
    public GameObject OutInventoryPoint;

    public bool isProcessing;
    
    //public GameObject tomato;

    public int CurrentInInvSize = 0;
    public int CurrentOutInvSize = 0;

    public void Start()
    {
      //InInventory.Add(tomato, 4);
      //CurrentInInvSize = 4;
    }
    public void Update()
    {
        
        activeProcess.process(ref InInventory, ref OutInventory,ref OutInventorySize,gameObject,ref CurrentInInvSize,ref CurrentOutInvSize,ref isProcessing);
    }

    public void endAnim()
    {
        isProcessing = false;
        Debug.Log("Endanim");
        activeProcess.finishProcess(ref InInventory, ref OutInventory, ref CurrentOutInvSize);
    }
}
