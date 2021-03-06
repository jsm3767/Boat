﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    int cameraSpeed = 3;
	public float speed = 1f;	
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.W))
        {
            transform.position += new Vector3(0,0,cameraSpeed * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.S))
        {
            transform.position -= new Vector3(0, 0, cameraSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.position -= new Vector3(cameraSpeed * Time.deltaTime, 0, 0);
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.position += new Vector3(cameraSpeed * Time.deltaTime, 0,0);
        }

		if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved)
        {
			Vector2 touchDeltaPosition = Input.GetTouch(0).deltaPosition;

			transform.position += new Vector3(
                -touchDeltaPosition.x * speed * Time.deltaTime, 
                0.0f,
                -touchDeltaPosition.y * speed * Time.deltaTime);

		}
    }
}
