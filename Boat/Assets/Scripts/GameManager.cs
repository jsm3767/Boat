using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    public Slider speedSlider;
    public Slider directionSlider;
    private bool ignorechanges = false;
   
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
        playerShips = new List<GameObject>(GameObject.FindGameObjectsWithTag("Player"));

        speedSlider.onValueChanged.AddListener(delegate {setSelectedShipSpeed((int)speedSlider.value); });
        directionSlider.onValueChanged.AddListener(delegate {setSelectedShipDirection((int)directionSlider.value); });

        WaveManager.Instance.SpawnWave();
    }

    // Update is called once per frame
    void Update()
    {
        if (WaveManager.Instance.CountAliveShips() == 0)
        {
            WaveManager.Instance.waveIndex++;
            WaveManager.Instance.SpawnWave();
        }

    }

    public void DoTurn()
    {
        foreach (GameObject ship in playerShips)
        {
            ship.SendMessage("PlayTurn");
        }
        setSlidersToSelectedShip();
    }

    private void setSlidersToSelectedShip()
    {
        if (selectedShip.GetComponent<Ship>().shipSpeed == ShipSpeed.None)
            speedSlider.value = 0;
        else if (selectedShip.GetComponent<Ship>().shipSpeed == ShipSpeed.HalfMast)
            speedSlider.value = 1;
        else if (selectedShip.GetComponent<Ship>().shipSpeed == ShipSpeed.FullMast)
            speedSlider.value = 2;

        if (selectedShip.GetComponent<Ship>().turnDirection == TurnDirection._90L)
            directionSlider.value = -2;
        else if (selectedShip.GetComponent<Ship>().turnDirection == TurnDirection._45L)
            directionSlider.value = -1;
        else if (selectedShip.GetComponent<Ship>().turnDirection == TurnDirection.None)
            directionSlider.value = 0;
        else if (selectedShip.GetComponent<Ship>().turnDirection == TurnDirection._45R)
            directionSlider.value = 1;
        else if (selectedShip.GetComponent<Ship>().turnDirection == TurnDirection._90R)
            directionSlider.value = 2;
    }

    private void setSelectedShipSpeed(int value)
    {
        selectedShip.GetComponent<Ship>().setShipSpeed(value);

    }

    private void setSelectedShipDirection(int value)
    {
        selectedShip.GetComponent<Ship>().setTurnDirection(value);
    }
    private void setSelectedShipLeftFire()
    {
        selectedShip.GetComponent<Ship>().setLeftFire();
    }
    private void setSelectedShipRightFire()
    {
        selectedShip.GetComponent<Ship>().setRightFire();
    }

    private void setSelectedShipNoFire()
    {
        selectedShip.GetComponent<Ship>().setCancelFire();
    }


    public void GoToNext()
    {
        int currentIndex = 0;

        if (GameManager.Instance.SelectedShip != null)
        {
            for (int index = 0; index < GameManager.Instance.PlayerShips.Count; index++)
            {
                if (GameManager.Instance.PlayerShips[index].GetInstanceID() == GameManager.Instance.SelectedShip.GetInstanceID())
                {
                    currentIndex = index;
                }
            }

            currentIndex = (currentIndex + 1) % GameManager.Instance.PlayerShips.Count;
        }

        GameManager.Instance.SelectedShip = GameManager.Instance.PlayerShips[currentIndex];

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
        setSlidersToSelectedShip();
    }

    public void RemovePlayerShipFromList(GameObject toRemove)
    {
        PlayerShips.Remove(toRemove);
    }
}
