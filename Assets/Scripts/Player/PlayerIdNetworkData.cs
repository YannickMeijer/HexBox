using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerIdNetworkData : NetworkData
{
    [SerializeField]
    private int playerId;

    public PlayerIdNetworkData(int playerId)
    {
        this.playerId = playerId;
    }

    public int PlayerId
    {
        get { return playerId; }
    }
}
