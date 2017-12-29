using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public int health, defense, attack, sight, contactDamage, movementInTiles, unitMoveDuration;
    int actualSpeed, terrainSpdMod;
    public Queue<HexagonTile> movement;
    bool movementAllowed;
    int tilesMoved;
    HexagonTile currentHex, targetHex;
    PathFinding pathFinding;
    TurnTimer timer;

    //Temporary Remove when actions are inside 1 class
    public Hand hand;


    void Start()
    {
        movementAllowed = false;
        tilesMoved = 0;
        GameObject.FindGameObjectWithTag("TurnTimer").GetComponent<TurnTimer>().moveBegin += StartMovement;
    }

    void Update()
    {
        MovementUpdate();
    }

    void CombatUpdate()
    {

    }

    void MovementUpdate()
    {
        actualSpeed = unitMoveDuration / movementInTiles;
        if (movementAllowed && currentHex != targetHex)
        {
            transform.position = Vector3.MoveTowards(transform.position, currentHex.transform.position, actualSpeed * Time.deltaTime);
            ExecuteMovement();
        }
    }

    void ExecuteMovement()
    {
        if (movement.Count != 0 && tilesMoved != movementInTiles)
        {
            if (transform.position == currentHex.transform.position)
            {
                pathFinding.FindPath(currentHex, targetHex);
                currentHex = movement.Dequeue();
                tilesMoved += 1;
            }
        }
        else
            movementAllowed = false;
    }

    void StartMovement()
    {
        movementAllowed = true;
        tilesMoved = 0;
    }

}