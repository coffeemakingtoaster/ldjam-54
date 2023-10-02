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

    public bool isProcessing = false;
    
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
        activeProcess.process(ref InInventory, ref OutInventory, ref OutInventorySize, gameObject, ref CurrentInInvSize, ref CurrentOutInvSize, ref isProcessing);

        // activeprocess.output.firstitemfromdictionary
    }

    public void endAnim()
    {
        isProcessing = false;
        Debug.Log("Endanim");
        activeProcess.finishProcess(ref InInventory, ref OutInventory, ref CurrentOutInvSize);
    }

    public void AddItemInToInventory(GameObject item)
    {
        if (this.CurrentInInvSize >= this.InInventorySize)
        {
            return;
        }
        if (InInventory.ContainsKey(item))
        {
            InInventory[item] += 1;
        }
        else
        {
            InInventory.Add(item,1);
        }
        this.CurrentInInvSize += 1;
    }

    public GameObject GetItemFromOutIventory()
    {
        foreach (var key in OutInventory.Keys)
        {
            if (OutInventory[key] > 0){
                OutInventory[key] -= 1;
                CurrentOutInvSize -= 1;
                return key;
            }
        }
        return null;
    }

    public bool HasInInventoryCapacity()
    {
        return this.CurrentInInvSize < this.InInventorySize;
    }

    public bool HasOutInventoryContents()
    {
        Debug.Log("Checking out inventory");
        return this.CurrentOutInvSize > 0;
    }
}
