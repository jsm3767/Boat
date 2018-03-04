using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    protected GameManager() { }

    [SerializeField]
    private GameObject selectedShip;

    private List<GameObject> ships;

    private float cameraMaxZoom;
    private float cameraMinZoom;
    private float cameraCurrentZoom = 4.0f;

    public float CameraCurrentZoom
    {
        get { return cameraCurrentZoom; }
    }

    public GameObject SelectedShip
    {
        get { return selectedShip; }
        set { this.selectedShip = value; }
    }

    // Use this for initialization
    void Start()
    {
        ships = new List<GameObject>(GameObject.FindGameObjectsWithTag("Ship"));
    }

    // Update is called once per frame
    void Update()
    {
      
    }

    public void DoTurn()
    {
        foreach (GameObject ship in ships)
        {
            ship.SendMessage("PlayTurn");
        }
    }
}
