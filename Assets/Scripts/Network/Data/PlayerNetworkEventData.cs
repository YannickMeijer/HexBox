using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerNetworkEventData : NetworkData
{
    [SerializeField]
    private PlayerData playerData;
    [SerializeField]
    private PlayerNetworkEvent eventType;

    public PlayerNetworkEventData(PlayerData playerData, PlayerNetworkEvent eventType)
    {
        this.playerData = playerData;
        this.eventType = eventType;
    }

    public PlayerData PlayerData
    {
        get { return playerData; }
    }

    public PlayerNetworkEvent EventType
    {
        get { return eventType; }
    }
}

public enum PlayerNetworkEvent
{
    CONNECTED = 0,
    DISCONNECTED = 1
}
