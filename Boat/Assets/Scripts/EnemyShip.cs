using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShip : Ship {

	private GameObject[] playerShipsOBJ;
	private GameObject closestPlayer;
	private bool shootTurn = false;

	// Use this for initialization
	void Start () {
		playerShipsOBJ = GameObject.FindGameObjectsWithTag ("Player");
		turnDirection = TurnDirection.None;
		shipSpeed = ShipSpeed.FullMast;
	}
	
	// Update is called once per frame
	void Update () {
		// Find Closest Player
		closestPlayer = findClosestPlayer ();

		// CHeck if they are within range for cannon attack this turn
		shootTurn = false;
		if (Vector3.Distance (closestPlayer.transform.position, this.transform.position) < 4) {
			shootTurn = true;
		}

		// yes - fire

		// Change direction to front of player and figure out what side to fire on
			// only turn if angle to player is more than turn amount.
			// ie angle < 45 no turn
				// angle > 45 but < 90 turn 45
	}

	private GameObject findClosestPlayer(){
		float dist = float.MaxValue;
		GameObject closest = null;
		foreach (GameObject g in playerShipsOBJ) {
			if (Vector3.Distance (g.transform.position, this.transform.position) < dist) {
				dist = Vector3.Distance (g.transform.position, this.transform.position);
				closest = g;
			}
		}
		return closest;
	}
}
