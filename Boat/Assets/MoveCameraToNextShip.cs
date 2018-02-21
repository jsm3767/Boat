using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCameraToNextShip : MonoBehaviour
{
    private GameObject[] playerShips;
    private GameObject currentShip;
    private float xOffset = -2.3f;

    // Use this for initialization
    void Start()
    {
        playerShips = GameObject.FindGameObjectsWithTag("PlayerShip");
    }

    // Update is called once per frame
    
    public void GoToNext()
    {
        int currentIndex = 0;
        if (currentShip != null)
        {
            for( int index = 0; index < playerShips.Length; index++ )
            {
                if (playerShips[index].GetInstanceID() == currentShip.GetInstanceID())
                {
                    currentIndex = index;
                }
            }

            currentIndex = (currentIndex + 1) % playerShips.Length;
        }

        playerShips[currentIndex].GetComponentInChildren<Canvas>().enabled = true;
        Camera.main.transform.position = new Vector3(playerShips[currentIndex].transform.position.x + xOffset, 5.0f, playerShips[currentIndex].transform.position.z);
    }

}
