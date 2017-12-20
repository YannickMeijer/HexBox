using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitCard : Card
{
    public int health, defense, attack, sight, contactDamage, moveSpeed;
    public List<HexagonTile> movement;
    bool acted, performActions;
    int moved;

    protected override void Start()
    {
        base.Start();
        acted = false;
        moved = 0;
        moveSpeed = 2;
    }

    protected override void Update()
    {
        // TODO: on turntimer htting zero, move equal to movespeed down movement list.
        if (timer.unitProgress && !acted)
        {
            performActions = true;
            acted = true;
        }
        else
            acted = false;

        ExecuteActions();
    }

    void ExecuteActions()
    {
        if (Location == CardLocation.PLAY)
        {
            if (movement.Count != 0 && transform.position == currentHex.transform.position && performActions)
            {
                currentHex = movement[0];
                movement.RemoveAt(0);
                moved += 1;
                if (moved == moveSpeed)
                {
                    moved = 0;
                    performActions = false;
                }
            }
        }
    }

    public void Pathfinding(HexagonTile targetHex)
    {
        //placeholder
        //movement.Clear();
        movement.Add(targetHex);
    }
}
