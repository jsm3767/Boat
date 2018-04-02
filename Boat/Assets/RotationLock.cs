using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationLock : MonoBehaviour {

    Quaternion rotation;
    Canvas canvas;
    void Awake()
    {
        canvas = GetComponent<Canvas>();
        canvas.transform.rotation = Quaternion.Euler(30, 0, 0);
    }
    void Update()
    {
        canvas.transform.rotation = Quaternion.Euler(30, 0, 0);
    }
}
