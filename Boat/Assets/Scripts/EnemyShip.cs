using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShip : Ship {

	private List<GameObject> playerShipsOBJ = new List<GameObject>();


	// Use this for initialization
	void Start () {
		playerShipsOBJ = GameObject.FindGameObjectsWithTag ("Player");
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
