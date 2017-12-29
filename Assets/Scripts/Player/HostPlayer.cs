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
            int newId = Interlocked.Increment(ref currentPlayerId);
            socket.Send(new PlayerId(newId));

            data.Id = newId;
            players.Add(data);
        });
    }

    /// <summary>
    /// Set the game options and send them to the other players.
    /// </summary>
    /// <param name="options">The new game options.</param>
    public void SetGameOptions(GameOptions options)
    {
        gameOptions = options;
        if (socket != null)
            socket.Send(options);
    }
}
