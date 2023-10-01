using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public enum LoadingZoneMode
{
    IN = 0,
    OUT = 1
};

public class LoadingZone : MonoBehaviour
{
    public LoadingZoneMode mode;
    private void OnTriggerEnter(Collider other)
    {
        TrainWagon wagon = other.gameObject.GetComponent<TrainWagon>();
        if (!wagon)
        {
            return;
        }
        Machine machine = transform.parent.GetComponent<Machine>();
        if (this.mode == LoadingZoneMode.IN)
        {
            if (!machine.HasInInventoryCapacity())
            {
                return;
            }
            GameObject retrievedPayload = wagon.TryToRetrievePayload();
            if (!retrievedPayload)
            {
                return;
            }
            machine.AddItemInToInventory(retrievedPayload);
        } else {
            if (!machine.HasOutInventoryContents()){
                return;
            }
            if (wagon.payload != null){
                return;
            }
            Debug.Log("Adding item to wagon");
            GameObject item = machine.GetItemFromOutIventory();
            Debug.LogWarning("Adding item "+item.name);
            wagon.TryToAddPayload(item);
        }
    }
}
