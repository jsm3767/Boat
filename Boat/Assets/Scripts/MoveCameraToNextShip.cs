using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Get currentShip from gameManager
public class MoveCameraToNextShip : MonoBehaviour
{
    private GameObject[] playerShips;
    private GameObject currentShip = null;
    private float zOffset = -2.3f;

    // Use this for initialization
    void Start()
    {
        playerShips = GameObject.FindGameObjectsWithTag("Ship");
    }

    // Update is called once per frame
    
    public void GoToNext()
    {
        int currentIndex = 0;
        if (currentShip != null)
        {
            currentShip.GetComponentInChildren<Canvas>().enabled = false;

            for ( int index = 0; index < playerShips.Length; index++ )
            {
                if (playerShips[index].GetInstanceID() == currentShip.GetInstanceID())
                {
                    currentIndex = index;
                }
            }
            
            currentIndex = (currentIndex + 1) % playerShips.Length;
        }

        currentShip = playerShips[currentIndex];
        playerShips[currentIndex].GetComponentInChildren<Canvas>().enabled = true;

        //start coroutine move camera
        StartCoroutine(SmoothCameraMove(.3f, new Vector3(playerShips[currentIndex].transform.position.x, 5.0f, playerShips[currentIndex].transform.position.z + zOffset)));
        
    }


    IEnumerator SmoothCameraMove(float transitionDuration, Vector3 target)
    {
        Vector3 startLocation = Camera.main.transform.position;
        float t = 0.0f;
        while (t < 1.0f)
        {
            t += Time.deltaTime * (Time.timeScale / transitionDuration);
            Camera.main.transform.position = Vector3.Lerp(startLocation, target, t);
            yield return 0;
        }
    }
}
