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
    public TurnDirection turnDirection = TurnDirection.None;
    public ShipSpeed shipSpeed = ShipSpeed.None;
    public LoadedCannon loadedCannon = LoadedCannon.None;

    protected float TurnPlaySpeed = 2.0f; //In seconds, move to gamemanager

    protected float baseSpeed = 2.0f;
    public Collider leftCollider;
    public Collider rightCollider;
    public int turnsToReload = 0;
    public int health = 3;
    public ParticleSystem leftsmoke;
    public ParticleSystem rightsmoke;

    public bool HasActions
    {
        get
        {
            if (turnDirection == TurnDirection.None
                && shipSpeed == ShipSpeed.None
                && loadedCannon == LoadedCannon.None)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }

    public void setShipSpeed(int newSpeed)
    {
        if (newSpeed == 0)
            shipSpeed = ShipSpeed.None;
        if (newSpeed == 1)
            shipSpeed = ShipSpeed.HalfMast;
        if (newSpeed == 2)
            shipSpeed = ShipSpeed.FullMast;
    }

    public void setTurnDirection(int newDirection)
    {
        if (newDirection == -2)
            turnDirection = TurnDirection._90L;
        if (newDirection == -1)
            turnDirection = TurnDirection._45L;
        if (newDirection == 0)
            turnDirection = TurnDirection.None;
        if (newDirection == 1)
            turnDirection = TurnDirection._45R;
        if (newDirection == 2)
            turnDirection = TurnDirection._90R;
    }

    public void setRightFire()
    {
        loadedCannon = LoadedCannon.Right;
    }

    public void setLeftFire()
    {
        loadedCannon = LoadedCannon.Left;
    }

    public void setCancelFire()
    {
        loadedCannon = LoadedCannon.None;
    }

    //Movement/attack functions
    protected IEnumerator MoveForward()
    {
        if (shipSpeed == ShipSpeed.None)
        {
            ResetShip();
            yield break;
        }

        float t = 0.0f;
        Vector3 startLocation = transform.position;
        float speedMultiplier = 0.0f;

        switch (shipSpeed)
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


        while (t < 1)
        {
            t += Time.deltaTime * (Time.timeScale / TurnPlaySpeed);
            transform.position = Vector3.Lerp(startLocation, startLocation + (transform.forward * speedMultiplier * baseSpeed), Mathf.SmoothStep(0.0f, 1, t));
            yield return 0;
        }

        ResetShip();

    }

    //Called after moving forward and rotating
    private void ResetShip()
    {
        shipSpeed = ShipSpeed.None;
        loadedCannon = LoadedCannon.None;
        turnDirection = TurnDirection.None;
        rightCollider.enabled = false;
        leftCollider.enabled = false;
    }

    protected IEnumerator Rotate()
    {
        if (turnDirection == TurnDirection.None)
        {
            StartCoroutine(MoveForward());
            yield break;
        }

        float t = 0.0f;

        float speedMultiplier = 0.0f;

        switch (turnDirection)
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
        while (t < 1)
        {
            t += Time.deltaTime * (Time.timeScale / TurnPlaySpeed);
            //45*t * multiplier degrees
            transform.rotation = originalRotation;
            transform.Rotate(Vector3.up * 45.0f * Mathf.SmoothStep(0.0f, 1, t) * speedMultiplier, Space.World);
            yield return 0;
        }


        StartCoroutine(MoveForward());
    }

    protected IEnumerator Fire()
    {
        if (loadedCannon == LoadedCannon.None)
        {
            rightCollider.enabled = false;
            leftCollider.enabled = false;
            StartCoroutine(Rotate());
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

    protected virtual void PlayTurn()
    {
        //example usage of gamemanager
        //GameManager.Instance.SelectedShip
        if (turnsToReload == 0)
            StartCoroutine(Fire());
        else
        {
            turnsToReload--;
            StartCoroutine(Rotate());
        }
    }

    private void Start()
    {
        //TODO: remove
        //Debug testing things

        leftsmoke.GetComponent<ParticleSystem>().enableEmission = false;
        rightsmoke.GetComponent<ParticleSystem>().enableEmission = false;

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
        turnDirection = (TurnDirection)System.Enum.Parse(typeof(TurnDirection), direction);
    }
    public void SetLoadedCannon(string cannon)
    {
        loadedCannon = (LoadedCannon)System.Enum.Parse(typeof(LoadedCannon), cannon);
    }
    public void SetShipSpeed(string speed)
    {
        shipSpeed = (ShipSpeed)System.Enum.Parse(typeof(ShipSpeed), speed);
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (turnsToReload == 0 && !(loadedCannon == LoadedCannon.None))
        {
            if (other.gameObject.tag == "Enemy")
            {
                if (loadedCannon == LoadedCannon.Left)
                    leftsmoke.GetComponent<ParticleSystem>().enableEmission = true;
                else if (loadedCannon == LoadedCannon.Right)
                    rightsmoke.GetComponent<ParticleSystem>().enableEmission = true;

                other.GetComponent<EnemyShip>().getHit();
                turnsToReload = 3;
                StartCoroutine(stopSmoke());
            }
        }
    }

    IEnumerator stopSmoke()
    {

        yield return new WaitForSeconds(.4f);
        leftsmoke.GetComponent<ParticleSystem>().enableEmission = false;
        rightsmoke.GetComponent<ParticleSystem>().enableEmission = false;
    }

    public void getHit()
    {
        health--;
        if (health == 0)
            Sink();
    }
    protected virtual void Sink()
    {
        Destroy(gameObject, 5);
        if(gameObject.tag=="Player")
            GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().RemovePlayerShipFromList(gameObject);
        float t = 0.0f;
        while (t < 1)
        {
            t += Time.deltaTime * (Time.timeScale / TurnPlaySpeed);
            Vector3 startpos = gameObject.transform.position;
            Vector3 endpos = startpos;
            endpos.y -= 1;
            transform.position = Vector3.Lerp(startpos, endpos, Mathf.SmoothStep(0.0f, 1, t));
        }
    }
}
