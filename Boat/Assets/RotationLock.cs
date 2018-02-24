using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationLock : MonoBehaviour {

    Quaternion rotation;
    void Awake()
    {
        transform.rotation = Quaternion.Euler( 75, 0, 0 );
        rotation = transform.rotation;
    }
    void LateUpdate()
    {
        transform.rotation = rotation;
    }
}
