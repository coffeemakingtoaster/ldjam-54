using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Machine : MonoBehaviour
{
    public Dictionary<GameObject, int> InInventory = new();
    public int InInventorySize;
    public int OutInventorySize;
    public Dictionary<GameObject, int> OutInventory = new();
    public Process[] processes;
    private Process activeProcess;
    public GameObject InInventoryPoint;
    public GameObject OutInventoryPoint;
    public GameObject FoodPoint;

    public bool isProcessing = false;
    public bool isDisplaying = false;
    
    //public GameObject tomato;

    public int CurrentInInvSize = 0;
    public int CurrentOutInvSize = 0;

    private GameObject display;

    AudioSource audioSource;

    public AudioClip clip;

    public void Start()
    {
        if (clip){
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = clip;
        }
        //InInventory.Add(tomato, 4);
        //CurrentInInvSize = 4;
    }
    public void Update()
    {

        if (activeProcess == null){
            activeProcess = GetViableProcess();
            if (activeProcess == null){
                return;
            }
        }
        activeProcess.process(ref InInventory, ref OutInventory, ref OutInventorySize, gameObject, ref CurrentInInvSize, ref CurrentOutInvSize, ref isProcessing);
        //Debug.Log(activeProcess.inputItems[0]);
        if (isProcessing && activeProcess.inputItems.Length > 0  && !isDisplaying)
        {
            Debug.Log("Instantiating");
            audioSource.Play();
            display = Instantiate(activeProcess.inputItems[0], gameObject.transform.position+FoodPoint.transform.localPosition, Quaternion.identity);
            isDisplaying = true;
        }
    }

    public void endAnim()
    {
        isProcessing = false;
        isDisplaying = false;
        //Debug.Log("Endanim");
        Destroy(display);
        activeProcess.finishProcess(ref InInventory, ref OutInventory, ref CurrentOutInvSize);
        activeProcess = null;
        audioSource.Stop();
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

    // Asses which process can be started based on contents of inInventory
    public Process GetViableProcess(){
        foreach(Process p in processes){
            if (p.gotInputs(InInventory)){
                return p;
            }
        }
        return null;
    }
}
