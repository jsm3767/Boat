using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionIndicator : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ToggleEnabled()
    {
        gameObject.GetComponent<MeshRenderer>().enabled = !gameObject.GetComponent<MeshRenderer>().enabled;
    }
}
