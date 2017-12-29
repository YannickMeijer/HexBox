using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ClientPlayer : NetworkPlayer
{
    protected override void Start()
    {
        base.Start();

        playerData = new PlayerData(-1, "Client Player");
        socket = socketManager.CreateClientSocket(QosType.ReliableSequenced, "127.0.0.1");

        // On connected send our player data.
        socket.OnConnected += () => socket.Send(playerData);

        // Hook into the initialization data.
        socket.OnData<PlayerIdNetworkData>(data =>
        {
            playerData.Id = data.PlayerId;
            Debug.Log("Initialized player, id: " + playerData.Id);
        });

        socket.OnData<PlayerNetworkEventData>(HandlerPlayerNetworkEvent);
    }

    public override void Send(NetworkData data)
    {
        socket.Send(data);
    }

    public override void InitializeLobbyGameOptions(GameOptionsUiContainer optionsContainer)
    {
        // Disable all interactive elements.
        optionsContainer.PlayerCountSlider.interactable = false;

        // Listen for new game options.
        socket.OnData<GameOptions>(newOptions =>
        {
            gameOptions = newOptions;
            optionsContainer.ApplyOptions(newOptions);
        });
    }

    private void HandlerPlayerNetworkEvent(PlayerNetworkEventData data)
    {
        // Fire the right event based on the event type.
        switch (data.EventType)
        {
            case PlayerNetworkEvent.CONNECTED:
                FireOnPlayerConnected(data.PlayerData);
                break;
            case PlayerNetworkEvent.DISCONNECTED:
                FireOnPlayerDisconnected(data.PlayerData);
                break;
            default:
                Debug.Log("Unknown player network event type: " + data.EventType);
                break;
        }
    }
}
