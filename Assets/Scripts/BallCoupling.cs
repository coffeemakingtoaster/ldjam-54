using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallCoupling : MonoBehaviour
{
    public Vector3 GetPosition(){
        return transform.position;
    }

    public Quaternion GetRotation(){
        // Return parent orientation as point does not have a (usable) rotation 
        return gameObject.transform.parent.transform.rotation;
    }
}
