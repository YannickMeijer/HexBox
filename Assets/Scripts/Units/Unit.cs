using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
	public int unitMoveDuration;
	public int movementInTiles;
	public int movementCount;
	public int tilesMoved;
	public int health;
	public int sight;
	int actualSpeed;

    public Queue<HexagonTile> movement;

	public HexagonTile targetHex;

    public bool movementAllowed;

    PathFinding pathFinding;
    TurnTimer timer;

    void Start()
    {
        movementAllowed = false;
        tilesMoved = 0;
        timer = GameObject.FindGameObjectWithTag("TurnTimer").GetComponent<TurnTimer>();
        timer.moveBegin += StartMovement;
        pathFinding = GetComponent<PathFinding>();
        movement = new Queue<HexagonTile>();
    }

    void Update()
    {
        movementCount = movement.Count;
        unitMoveDuration = timer.unitMoveDuration;
        MovementUpdate();
    }

    void CombatUpdate()
    {

    }

    void MovementUpdate()
    {
        actualSpeed = unitMoveDuration / movementInTiles;
        if (movementAllowed)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetHex.transform.position, actualSpeed * Time.deltaTime);
            ExecuteMovement();
        }
    }

    void ExecuteMovement()
    {
        if (movement.Count != 0 && tilesMoved != movementInTiles)
        {
            if (transform.position == targetHex.transform.position)
            {
                transform.parent = targetHex.transform;
                targetHex = movement.Dequeue();
                tilesMoved += 1;
            }
        }
        else if(transform.position == targetHex.transform.position)
            movementAllowed = false;
    }

    public void StartMovement()
    {
        movementAllowed = true;
        tilesMoved = 0;
    }

    public bool InSight(HexagonTile p)
    {
        return true;
    }

    public void FindPath(HexagonTile goalTile)
    {
        movement = pathFinding.FindPath(targetHex, goalTile, this);
    }
}