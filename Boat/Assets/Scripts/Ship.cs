using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TurnDirection
{
    _45L,
    _90L,
    _45R,
    _90R,
    None = -1
}

public enum ShipSpeed
{
    None = -1,
    HalfMast = 0,
    FullMast = 1
}

public enum LoadedCannon
{
    Port = 0,
    Left = 0,

    Starboard = 1,
    Right = 1,

    None = -1
}

public class Ship : MonoBehaviour
{
    //Gamemanager checks if the ship has actions for the current turn
    //protected bool hasActions;

    //Store turn's current actions
    protected TurnDirection turnDirection = TurnDirection.None;
    protected ShipSpeed shipSpeed = ShipSpeed.None;
    protected LoadedCannon loadedCannon = LoadedCannon.None;

    protected float TurnPlaySpeed = 2.0f; //In seconds, move to gamemanager

    protected float baseSpeed = 2.0f;
    public Collider leftCollider;
    public Collider rightCollider;

    public bool HasActions
    {
        get
        {
            if( turnDirection == TurnDirection.None
                && shipSpeed == ShipSpeed.None
                && loadedCannon == LoadedCannon.None )
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }

    //Movement/attack functions
    IEnumerator MoveForward()
    {
        if(shipSpeed == ShipSpeed.None)
        {
            ResetShip();
            yield break;
        }

        float t = 0.0f;
        Vector3 startLocation = transform.position;
        float speedMultiplier = 0.0f;

        switch( shipSpeed )
        {
            case ShipSpeed.None:
                speedMultiplier = 0.0f;
                break;
            case ShipSpeed.HalfMast:
                speedMultiplier = 1.0f;
                break;
            case ShipSpeed.FullMast:
                speedMultiplier = 2.0f;
                break;
        }
        

        while( t < 1 )
        {
            t += Time.deltaTime * (Time.timeScale / TurnPlaySpeed);
            transform.position = Vector3.Lerp( startLocation, startLocation + ( transform.forward * speedMultiplier * baseSpeed ) , Mathf.SmoothStep(0.0f,1, t ) );
            yield return 0;
        }

        ResetShip();

    }

    //Called after moving forward and rotating
    private void ResetShip()
    {
        Debug.Log(loadedCannon);
        shipSpeed = ShipSpeed.None;
        loadedCannon = LoadedCannon.None;
        turnDirection = TurnDirection.None;
        rightCollider.enabled = false;
        leftCollider.enabled = false;
    }

    IEnumerator Rotate()
    {
        if( turnDirection == TurnDirection.None)
        {
            StartCoroutine(MoveForward());
            yield break;
        }

        float t = 0.0f;

        float speedMultiplier = 0.0f;

        switch( turnDirection )
        {
            case TurnDirection._45L:
                speedMultiplier = -2.0f;
                break;
            case TurnDirection._90L:
                speedMultiplier = -4.0f;
                break;
            case TurnDirection._45R:
                speedMultiplier = 2.0f;
                break;
            case TurnDirection._90R:
                speedMultiplier = 4.0f;
                break;
            case TurnDirection.None:
                break;
        }

        speedMultiplier *= Time.timeScale / TurnPlaySpeed;

        Quaternion originalRotation = transform.rotation;

        //t < X where X=1 will take turnplayspeed time 
        while( t < 1 )
        {
            t += Time.deltaTime * ( Time.timeScale / TurnPlaySpeed );
            //45*t * multiplier degrees
            transform.rotation = originalRotation;
            transform.Rotate( Vector3.up * 45.0f * Mathf.SmoothStep( 0.0f, 1, t ) * speedMultiplier, Space.World );
            yield return 0;
        }


        StartCoroutine( MoveForward() );
    }

    IEnumerator Fire()
    {
        if(loadedCannon == LoadedCannon.None)
        {
            rightCollider.enabled = false;
            leftCollider.enabled = false;
            yield break;
        }
        switch (loadedCannon)
        {
            case LoadedCannon.Left:
                rightCollider.enabled = false;
                leftCollider.enabled = true;
                break;
            case LoadedCannon.Right:
                rightCollider.enabled = true;
                leftCollider.enabled = false;
                break;
            case LoadedCannon.None:
                rightCollider.enabled = false;
                leftCollider.enabled = false;
                break;
        }

        StartCoroutine(Rotate());
    }

    Vector3 TestCalculation()
    {
        return new Vector3();
    }

    public void PlayTurn()
    {
        //example usage of gamemanager
        //GameManager.Instance.SelectedShip
        StartCoroutine(Fire());
        
    }

    private void Start()
    {
        //TODO: remove
        //Debug testing things


        //turnDirection = (TurnDirection)Random.Range(0,4);
        //turnDirection = TurnDirection._45L;
        //shipSpeed = (ShipSpeed)Random.Range( -1, 2 );
        //shipSpeed = ShipSpeed.FullMast;

        //Debug.Log( turnDirection );

        //TestCalculation();

        //PlayTurn();
    }

    public void SetTurnDirection(string direction)
    {
        turnDirection = (TurnDirection)System.Enum.Parse(typeof(TurnDirection),direction);
    }
    public void SetLoadedCannon(string cannon)
    {
        loadedCannon = (LoadedCannon)System.Enum.Parse(typeof(LoadedCannon), cannon);
    }
    public void SetShipSpeed(string speed)
    {
        shipSpeed = (ShipSpeed)System.Enum.Parse(typeof(ShipSpeed), speed); 
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Enemy")
           Debug.Log("test");
    }

}
