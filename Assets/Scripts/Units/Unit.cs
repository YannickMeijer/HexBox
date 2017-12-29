using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public int health, defense, attack, sight, contactDamage, movementInTiles, unitMoveDuration;
    int actualSpeed, terrainSpdMod;
    public Queue<HexagonTile> movement;
    public HexagonTile targetHex;
    public bool movementAllowed;
    int tilesMoved;
    public HexagonTile currentHex;
    PathFinding pathFinding;
    TurnTimer timer;

    Player player;

    void Start()
    {
        movementAllowed = false;
        tilesMoved = 0;
        timer = GameObject.FindGameObjectWithTag("TurnTimer").GetComponent<TurnTimer>();
        timer.moveBegin += StartMovement;
        player = GameObject.Find("LocalPlayer").GetComponent<Player>();
        GetComponent<MouseHelper>().OnClick += () => player.selected = this;
        pathFinding = GetComponent<PathFinding>();
        movement = new Queue<HexagonTile>();
    }

    void Update()
    {
        unitMoveDuration = timer.unitMoveDuration;
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
                transform.parent = currentHex.transform;
                pathFinding.FindPath(currentHex, targetHex);
                currentHex = movement.Dequeue();
                tilesMoved += 1;
            }
        }
        else
            movementAllowed = false;
    }

    public void StartMovement()
    {
        movementAllowed = true;
        tilesMoved = 0;
    }

    public void FindPath(HexagonTile goalTile)
    {
        Debug.Log(goalTile);
        pathFinding.FindPath(currentHex, goalTile);
    }
}