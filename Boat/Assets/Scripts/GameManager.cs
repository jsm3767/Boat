using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    protected GameManager() { }

    [SerializeField]
    private GameObject selectedShip;
    //private GameObject[] playerShips;
    private List<GameObject> shipButtons;
    //private GameObject[] playerShipsIcons;

    private List<GameObject> playerShips;

    private float cameraMaxZoom;
    private float cameraMinZoom;
    private float cameraCurrentZoom = 4.0f;

    private float moveToShipCameraZOffset = -2.3f;


    public float CameraCurrentZoom
    {
        get { return cameraCurrentZoom; }
    }

    public GameObject SelectedShip
    {
        get { return selectedShip; }
        set { this.selectedShip = value; }
    }

    public float MoveToShipCameraZOffset
    {
        get
        {
            return moveToShipCameraZOffset;
        }

        set
        {
            moveToShipCameraZOffset = value;
        }
    }

    public List<GameObject> PlayerShips
    {
        get
        {
            return playerShips;
        }

        set
        {
            playerShips = value;
        }
    }

    // Use this for initialization
    void Start()
    {
        playerShips = new List<GameObject>(GameObject.FindGameObjectsWithTag("Ship"));
        shipButtons = new List<GameObject>(GameObject.FindGameObjectsWithTag("CanvasButtons"));
    }

    // Update is called once per frame
    void Update()
    {
      
    }

    public void DoTurn()
    {
        foreach (GameObject ship in playerShips)
        {
            ship.SendMessage("PlayTurn");
        }
    }
}
