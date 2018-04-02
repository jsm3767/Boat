using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyShip : Ship
{

    private GameObject[] playerShipsOBJ;
    public GameObject closestPlayer;
    private bool shootTurn = false;

    // Use this for initialization
    void Start()
    {
        playerShipsOBJ = GameObject.FindGameObjectsWithTag("Player");
        turnDirection = TurnDirection.None;
        shipSpeed = ShipSpeed.FullMast;
        healthBar = GetComponentInChildren<Image>();

    }

    //If I'm correct we don't have to recalculate every frame so just moved this to playturn instead of 
    //having it in update. does base.PlayTurn after calculating
    protected override void PlayTurn()
    {
        // Find Closest Player
        closestPlayer = findClosestPlayer();

        // CHeck if they are within range for cannon attack this turn
        shootTurn = false;
        if (Vector3.Distance(closestPlayer.transform.position, this.transform.position) < 4)
        {
            shootTurn = true;
        }

        // yes - fire

        // Change direction to front of player and figure out what side to fire on
        // only turn if angle to player is more than turn amount.
        // ie angle < 45 no turn
        // angle > 45 but < 90 turn 45

        // vector to player
        Vector3 toTarget = closestPlayer.transform.position - this.transform.position;
        // get angle diff between lines
        float angle = Mathf.Acos(Vector3.Dot(this.transform.forward, toTarget) / toTarget.magnitude);
        //Debug.Log(Mathf.Rad2Deg*angle);
        //now dtermine what side it's on.
        float angleToRight = Mathf.Acos(Vector3.Dot(this.transform.right, toTarget) / toTarget.magnitude);
        if (angleToRight * Mathf.Rad2Deg < 90)
        {
            if (Mathf.Rad2Deg * angle < 90)
            {
                turnDirection = TurnDirection._45R;
            }
            else
            {
                turnDirection = TurnDirection._90R;
            }
        }
        else
        {
            if (Mathf.Rad2Deg * angle < 90)
            {
                turnDirection = TurnDirection._45L;
            }
            else
            {
                turnDirection = TurnDirection._90L;
            }
        }
        if (Mathf.Rad2Deg * angle < 45)
        {
            turnDirection = TurnDirection.None;
            if (shootTurn)
            {
                shipSpeed = ShipSpeed.HalfMast;
            }
            else
            {
                shipSpeed = ShipSpeed.FullMast;
            }
        }
        else
        {
            shipSpeed = ShipSpeed.HalfMast;
        }
        if (turnsToReload != 0)
            turnsToReload--;
        StartCoroutine(Rotate());
    }


    private GameObject findClosestPlayer()
    {
        float dist = float.MaxValue;
        GameObject closest = null;
        foreach (GameObject g in playerShipsOBJ)
        {
            if (Vector3.Distance(g.transform.position, this.transform.position) < dist)
            {
                dist = Vector3.Distance(g.transform.position, this.transform.position);
                closest = g;
            }
        }
        return closest;
    }

    protected override void OnTriggerEnter(Collider other)
    {
        if(turnsToReload == 0)
        {
            if(other.gameObject.tag == "Player")
            {
                turnsToReload = 5;
                other.GetComponent<Ship>().getHit();
            }
        }
    }
}
