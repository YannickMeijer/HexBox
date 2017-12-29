﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Networking;

public class HostPlayer : NetworkPlayer
{
    // TODO: support multiple clients by having a dedicated socket for assigning port numbers.

    private int currentPlayerId = 0;

    protected override void Start()
    {
        base.Start();

        // For the host the id is always 0.
        playerData = new PlayerData(0, "Host Player");

        // Create the socket.
        socketManager.CreateHostSocket(QosType.ReliableSequenced, PlayerConnected);
    }

    private void PlayerConnected(Socket socket)
    {
        this.socket = socket;

        // When a new player sends their data, return their id.
        socket.OnData<PlayerData>(data =>
        {
            // Get the id for the player.
            int newId = Interlocked.Increment(ref currentPlayerId);
            socket.Send(new PlayerIdNetworkData(newId));
            data.Id = newId;

            // Send all players' data to the new player, add the player to the list of players.
            socket.Send(new PlayerNetworkEventData(playerData, PlayerNetworkEvent.CONNECTED));
            players.ForEach(player => socket.Send(new PlayerNetworkEventData(player, PlayerNetworkEvent.CONNECTED)));
            players.Add(data);

            // Fire the player connected event.
            FireOnPlayerConnected(data);

            // Hook into the disconnect event.
            socket.OnDisconnected += () => FireOnPlayerDisconnected(data);
        });
    }

    public override void Send(NetworkData data)
    {
        socket.Send(data);
    }

    /// <summary>
    /// Send the game options to the other players.
    /// </summary>
    private void UpdateGameOptions()
    {
        if (socket != null)
            Send(gameOptions);
    }

    public override void InitializeLobbyGameOptions(GameOptionsUiContainer optionsContainer)
    {
        // Hook into all events.
        optionsContainer.PlayerCountSlider.onValueChanged.AddListener(value =>
        {
            gameOptions.PlayerCount = (int)value;
            UpdateGameOptions();
        });
    }
}
