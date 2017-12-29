using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class TurnTimer : MonoBehaviour
{
    //If genesisPeriod is true, the game will not start normal turn progression until all players have ended their genesis turn.
    public bool genesisPeriod = true;

    //genesisDuration determines how long the gracefulStart lasts.
    public int turnDuration, unitMoveDuration, genesisDuration, attackDuration;
    
    public List<bool> playersReady;
    bool genesisActive = true, playerTime, unitTime, attackTime;

    //The values used internally for counting down. 
    public float internalTimer;
    

    public delegate void MoveBegin();
    public event MoveBegin moveBegin;

    public delegate void AttackBegin();
    public event MoveBegin attackBegin;

    public delegate void TurnBegin();
    public event MoveBegin turnBegin;

    void Start()
    {
        if (genesisPeriod)
            internalTimer = genesisDuration;
        else
            internalTimer = turnDuration;
        playerTime = true;
        unitTime = false;
    }


    void Update()
    {
        internalTimer -= Time.deltaTime;
        if (genesisPeriod && genesisActive)
        {
            if (playersReady.All(players => players == true) || internalTimer < 0)
            {
                genesisActive = false;
                internalTimer = unitMoveDuration;
            }
        }
        else
        {
            if (playerTime && internalTimer <= 0)
            {
                if(moveBegin != null)
                    moveBegin();
                internalTimer = unitMoveDuration;
                playerTime = false;
                unitTime = true;
            }
            else if (unitTime && internalTimer <= 0)
            {
                if(attackBegin != null)
                    attackBegin();
                internalTimer = attackDuration;
                unitTime = false;
                attackTime = true;
                
            }
            else if(attackTime && internalTimer <= 0)
            {
                if(turnBegin != null)
                    turnBegin();
                attackTime = false;
                internalTimer = turnDuration;
                playerTime = true;
            }
        }

    }
}