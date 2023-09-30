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

    public void Start()
    {
        activeProcess.awake(gameObject);
    }
    public void Update()
    {
        
        activeProcess.process(ref InInventory, ref OutInventory,ref OutInventorySize,ref InInventorySize);
    }

}
