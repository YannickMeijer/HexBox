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
        id = 0;

        socketManager.CreateHostSocket(QosType.ReliableSequenced, PlayerConnected);
    }

    private void PlayerConnected(Socket socket)
    {
        this.socket = socket;

        // Initialize the player.
        socket.OnConnected += () =>
        {
            socket.Send(new PlayerInitData(Interlocked.Increment(ref currentPlayerId)));
        };
    }
}
