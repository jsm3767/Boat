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

    protected float TurnPlaySpeed = 2.0f; //In seconds

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

        speedMultiplier *= 2.0f; //Remove and change based on individual boats in derived classes

        while( t < 1 )
        {
            t += Time.deltaTime * (Time.timeScale / TurnPlaySpeed);
            if (t > 0.5)
            {
                transform.position = Vector3.Lerp(startLocation, startLocation + (transform.forward * speedMultiplier), Mathf.SmoothStep(0.0f, 1, (t - 0.5f) * 2));
            }
            yield return 0;
        }
    }

    IEnumerator Rotate()
    {
        if( turnDirection == TurnDirection.None)
        {
            yield break;
        }

        float t = 0.0f;

        float speedMultiplier = 0.0f;

        switch( turnDirection )
        {
            case TurnDirection._45L:
                speedMultiplier = -1.0f;
                break;
            case TurnDirection._90L:
                speedMultiplier = -2.0f;
                break;
            case TurnDirection._45R:
                speedMultiplier = 1.0f;
                break;
            case TurnDirection._90R:
                speedMultiplier = 2.0f;
                break;
            case TurnDirection.None:
                break;
        }

        speedMultiplier *= Time.timeScale / TurnPlaySpeed;

        Quaternion originalRotation = transform.rotation;

        while( t < 0.5 )
        {
            t += Time.deltaTime * ( Time.timeScale / TurnPlaySpeed );
            //45*t * multiplier degrees
            transform.rotation = originalRotation;
            transform.Rotate( Vector3.up * 90.0f * Mathf.SmoothStep( 0.0f, 1, t*2 ) * speedMultiplier, Space.World );
            yield return 0;
        }
    }

    public void PlayTurn()
    {
        StartCoroutine( MoveForward() );
        StartCoroutine( Rotate() );
        //StartCoroutine( Fire() );
    }

    private void Start()
    {
        //Debug
        turnDirection = (TurnDirection)Random.Range(0,4);
        shipSpeed = (ShipSpeed)Random.Range( -1, 2 );
        Debug.Log( turnDirection );

        PlayTurn();
    }

    private void Update()
    {
        turnDirection = (TurnDirection)Random.Range(0, 4);
        shipSpeed = (ShipSpeed)Random.Range(-1, 2);
    }


}
