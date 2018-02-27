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
        

        while( t < 1 )
        {
            t += Time.deltaTime * (Time.timeScale / TurnPlaySpeed);
            transform.position = Vector3.Lerp( startLocation, startLocation + ( transform.forward * speedMultiplier * baseSpeed ) , Mathf.SmoothStep(0.0f,1, t ) );
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

        //t < X where X=1 will take turnplayspeed time 
        while( t < 1 )
        {
            t += Time.deltaTime * ( Time.timeScale / TurnPlaySpeed );
            //45*t * multiplier degrees
            transform.rotation = originalRotation;
            transform.Rotate( Vector3.up * 45.0f * Mathf.SmoothStep( 0.0f, 1, t ) * speedMultiplier, Space.World );
            yield return 0;
        }
    }

    Vector3 TestCalculation()
    {
        Vector3 result = new Vector3();
        Transform initial = new GameObject().transform;
        initial.position = transform.position;
        initial.rotation = transform.rotation;
        
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

        float turnSpeedMultiplier = 0.0f;

        switch( turnDirection )
        {
            case TurnDirection._45L:
                turnSpeedMultiplier = -1.0f;
                break;
            case TurnDirection._90L:
                turnSpeedMultiplier = -2.0f;
                break;
            case TurnDirection._45R:
                turnSpeedMultiplier = 1.0f;
                break;
            case TurnDirection._90R:
                turnSpeedMultiplier = 2.0f;
                break;
            case TurnDirection.None:
                break;
        }

        turnSpeedMultiplier *= Time.timeScale / TurnPlaySpeed;

        Quaternion originalRotation = transform.rotation;

        //t < X where X=1 will take turnplayspeed time 


        if( turnDirection == TurnDirection.None)
        {
            result = initial.position + ( initial.forward * speedMultiplier * baseSpeed );
        }
        else if( shipSpeed == ShipSpeed.None)
        {
            result = initial.position;
        }
        else
        {
            Vector3 startLocation = initial.position;

            float t0 = 0.0f;
            float t1 = 0.0f;

            while( t0 < 1 )
            {
                t0 += Time.deltaTime * ( Time.timeScale / TurnPlaySpeed );
                //45*t * multiplier degrees
                initial.rotation = originalRotation;
                initial.Rotate( Vector3.up * 45.0f * Mathf.SmoothStep( 0.0f, 1, t0 ) * speedMultiplier, Space.World );

                t1 += Time.deltaTime * ( Time.timeScale / TurnPlaySpeed );
                initial.position = Vector3.Lerp( startLocation, startLocation + ( initial.forward * speedMultiplier * baseSpeed ), Mathf.SmoothStep( 0.0f, 1, t1 ) );
            }
            result = initial.position;

        }
        
        Debug.Log( result.x );
        
        Debug.Log( result.z );

        return result;
    }

    public void PlayTurn()
    {

        StartCoroutine( Rotate() );
        StartCoroutine( MoveForward() );
        //StartCoroutine( Fire() );
    }

    private void Start()
    {
        //Debug
        //turnDirection = (TurnDirection)Random.Range(0,4);
        turnDirection = TurnDirection._45L;
        //shipSpeed = (ShipSpeed)Random.Range( -1, 2 );
        shipSpeed = ShipSpeed.FullMast;

        Debug.Log( turnDirection );

        TestCalculation();

        PlayTurn();
    }


}
