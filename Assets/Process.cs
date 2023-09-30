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
    
    
    public void awake(GameObject machine)
    {
        
        animator = machine.GetComponent<Animator>();
        for(int i = 0; i < inputItems.Length; i++)
        {
            
            Input.Add(inputItems[i], inputItemCount[i]);
            inputCount += inputItemCount[i];
            
        }
        for (int i = 0; i < outputItems.Length; i++)
        {
            Output.Add(outputItems[i], outputItemCount[i]);
            outputCount += outputItemCount[i];
        }
    }
    public void process(ref Dictionary<GameObject, int> InInventory,ref Dictionary<GameObject, int> OutInventory,ref int OutInventorySize,ref int InInventorySize)
    {
        if(OutInventory.Count < (OutInventorySize - outputCount) && !isProcessing && gotInputs(InInventory))
        {
            isProcessing = true;
            foreach (KeyValuePair<GameObject, int> input in Input)
            {
                foreach (KeyValuePair<GameObject, int> inInv in InInventory)
                {
                    if (inInv.Key == input.Key)
                    {
                        InInventory[inInv.Key] = inInv.Value - input.Value;
                    }
                }
            }
            InInventorySize -= inputCount;
            animator.SetTrigger(name);
        }
    }

    private bool gotInputs(Dictionary<GameObject, int> InInventory)
    {
        
        Dictionary<GameObject, bool> itemBools = new();
        
        foreach (KeyValuePair<GameObject, int> input in Input)
        {
            itemBools.Add(input.Key, false);
            foreach (KeyValuePair<GameObject, int> inv in InInventory)
            {
                if(inv.Key == input.Key && inv.Value >= input.Value)
                {
                    itemBools[input.Key] = true;
                }
            }
        }
        foreach (KeyValuePair<GameObject, bool> itemBool in itemBools)
        {
            if (!itemBool.Value)
            {
                return false;
            }
            
        }
        return true;

    }

    public void finishProcess(ref Dictionary<GameObject, int> InInventory, ref Dictionary<GameObject, int> OutInventory, ref int OutInventorySize)
    {
        foreach(KeyValuePair<GameObject, int> output in Output)
        {
            bool keyPresent = false;
            foreach(KeyValuePair<GameObject,int> outInv in OutInventory)
            {
                if(outInv.Key == output.Key)
                {
                    OutInventory[outInv.Key] = outInv.Value + output.Value;
                    keyPresent = true;
                }
                
            }
            if (!keyPresent)
            {
                OutInventory.Add(output.Key, output.Value);
            }
        }
        OutInventorySize += outputCount;

        isProcessing = false;

    }
}
