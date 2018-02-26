using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    protected GameManager() { }

    private GameObject selectedShip;

    private List<GameObject> shipsObj;
    private List<Ship> ships;
    public GameObject SelectedShip
    {
        get { return selectedShip; }
        set { this.selectedShip = value; }
    }

    // Use this for initialization
    void Start()
    {
        shipsObj = new List<GameObject>(GameObject.FindGameObjectsWithTag("Ship"));
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void NextTurn()
    {
        foreach(GameObject ship in shipsObj)
        {
            ship.GetComponent<Ship>().PlayTurn();
        }
    }
}
