using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//move to gamemanager
//Get currentShip from gameManager
public class MoveCameraToNextShip : MonoBehaviour
{


    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    
    public void GoToNext()
    {
        int currentIndex = 0;

        if (GameManager.Instance.SelectedShip != null)
        {
            GameManager.Instance.SelectedShip.GetComponentInChildren<Canvas>().enabled = false;
            
            for ( int index = 0; index < GameManager.Instance.PlayerShips.Count; index++ )
            {
                if (GameManager.Instance.PlayerShips[index].GetInstanceID() == GameManager.Instance.SelectedShip.GetInstanceID())
                {
                    currentIndex = index;
                }
            }
            
            currentIndex = (currentIndex + 1) % GameManager.Instance.PlayerShips.Count;
        }
        //turn off previous
        GameManager.Instance.SelectedShip.GetComponentInChildren<SelectionIndicator>().ToggleEnabled();

        GameManager.Instance.SelectedShip = GameManager.Instance.PlayerShips[currentIndex];

        //turn on new
        GameManager.Instance.SelectedShip.GetComponentInChildren<SelectionIndicator>().ToggleEnabled();
        GameManager.Instance.PlayerShips[currentIndex].GetComponentInChildren<Canvas>().enabled = true;

        //start coroutine move camera
        StartCoroutine(SmoothCameraMove(.3f, 
            new Vector3(GameManager.Instance.PlayerShips[currentIndex].transform.position.x, 
            GameManager.Instance.CameraCurrentZoom, 
            GameManager.Instance.PlayerShips[currentIndex].transform.position.z + GameManager.Instance.MoveToShipCameraZOffset)));
        
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
