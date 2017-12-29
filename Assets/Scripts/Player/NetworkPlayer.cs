using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class NetworkPlayer : MonoBehaviour
{
    public delegate void PlayerDataEvent(PlayerData player);
    public event PlayerDataEvent OnPlayerConnected;
    public event PlayerDataEvent OnPlayerDisconnected;

    protected SocketManager socketManager;
    protected Socket socket;

    protected PlayerData playerData;
    protected List<PlayerData> players = new List<PlayerData>();
    protected GameOptions gameOptions = new GameOptions();

    protected virtual void Start()
    {
        socketManager = GameObject.Find("Network").GetComponent<SocketManager>();
    }

    public void SendText(string text)
    {
        Send(new TextNetworkData("From player " + playerData.Id + ":\n" + text));
    }

    /// <summary>
    /// Send data to all connected players.
    /// </summary>
    /// <param name="data">The data to send.</param>
    public abstract void Send(NetworkData data);

    /// <summary>
    /// Initialize the lobby game options.
    /// </summary>
    /// <param name="optionsContainer">The container for the ui elements.</param>
    public abstract void InitializeLobbyGameOptions(GameOptionsUiContainer optionsContainer);

    protected void FireOnPlayerConnected(PlayerData player)
    {
        if (OnPlayerConnected != null)
            OnPlayerConnected(player);
    }

    protected void FireOnPlayerDisconnected(PlayerData player)
    {
        if (OnPlayerDisconnected != null)
            OnPlayerDisconnected(player);
    }

    public PlayerData PlayerData
    {
        get { return playerData; }
    }
}
