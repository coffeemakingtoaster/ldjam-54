using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Process : ScriptableObject
{
    private bool isProcessing;

    //You might never process the wounds left by those whose love you still remember
    public float timeToProcess;
    public GameObject[] inputItems;
    public GameObject[] outputItems;
    public int[] inputItemCount;
    public int[] outputItemCount;


    public Dictionary<GameObject, int> Input = new();
    public Dictionary<GameObject, int> Output = new();
    private int inputCount = 0;
    private int outputCount = 0;
    private Animator animator;


    public void awake()
    {

        inputCount = 0;
        outputCount = 0;

        for (int i = 0; i < inputItems.Length; i++)
        {

            Input.Add(inputItems[i], inputItemCount[i]);
            inputCount += inputItemCount[i];

        }
        for (int i = 0; i < outputItems.Length; i++)
        {
            Output.Add(outputItems[i], outputItemCount[i]);
            outputCount += outputItemCount[i];
        }
        Debug.Log(outputCount);
    }
    public void process(ref Dictionary<GameObject, int> InInventory, ref Dictionary<GameObject, int> OutInventory, ref int OutInventorySize, GameObject machine, ref int currentInInvSize, ref int currentOutInvSize, ref bool isProcessing)
    {
        Debug.Log(1);
        Debug.Log(currentOutInvSize <= (OutInventorySize - outputCount));
        Debug.Log(!this.isProcessing);
        Debug.Log(this.gotInputs(InInventory));

        //change to correct
        if (currentOutInvSize <= (OutInventorySize - outputCount) && !this.isProcessing && this.gotInputs(InInventory))
        {
            Debug.LogWarning("Processing");
            Debug.Log(gotInputs(InInventory));
            animator = machine.GetComponent<Animator>();

            isProcessing = true;
            foreach (KeyValuePair<GameObject, int> input in Input)
            {
                Debug.Log("Looping Lo");
                InInventory[input.Key] -= input.Value;

            }
            currentInInvSize -= inputCount;

            animator.SetTrigger(name);
        }
    }

    private bool gotInputs(Dictionary<GameObject, int> InInventory)
    {
        foreach (var key in Input.Keys)
        {
            // A needed item does not exist
            if (!InInventory.ContainsKey(key))
            {
                //Debug.LogWarning("Does not have key: "+ key);
                return false;
            }
            // An item does not have a sufficient amount
            if (InInventory[key] < Input[key])
            {
                //Debug.LogWarning("Amount not sufficient: "+ InInventory[key].ToString() + " | "+Input[key].ToString());
                return false;
            }
        }
        return true;
    }

    public void finishProcess(ref Dictionary<GameObject, int> InInventory, ref Dictionary<GameObject, int> OutInventory, ref int currentOutInvSize)
    {

        foreach (KeyValuePair<GameObject, int> output in Output)
        {
            if (OutInventory.ContainsKey(output.Key))
            {
                OutInventory[output.Key] += output.Value;
            }
            else
            {
                OutInventory.Add(output.Key, output.Value);
            }



        }
        currentOutInvSize += outputCount;

        this.isProcessing = false;
    }
}
