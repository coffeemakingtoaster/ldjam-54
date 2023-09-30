using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetupScript : MonoBehaviour
{
    public Process[] SOs;
    void Start()
    {
        foreach(Process so in SOs)
        {
            so.awake();
        }
    }
}
