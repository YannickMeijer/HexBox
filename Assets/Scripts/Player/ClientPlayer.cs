﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ClientPlayer : NetworkPlayer
{
    protected override void Start()
    {
        base.Start();
        socket = socketManager.CreateClientSocket(QosType.ReliableSequenced, "127.0.0.1");

        // Hook into the initialization data.
        socket.OnData<PlayerInitData>(data =>
        {
            id = data.Id;
            Debug.Log("Initialized player, id: " + id);
        });
    }
}
