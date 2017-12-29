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
        socket.OnData<PlayerId>(data =>
        {
            playerData.Id = data.Id;
            Debug.Log("Initialized player, id: " + playerData.Id);
        });
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
}
