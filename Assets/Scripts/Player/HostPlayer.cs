using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class HostPlayer : NetworkPlayer
{
    // TODO: support multiple clients by having a dedicated socket for assigning port numbers.

    protected override void Start()
    {
        base.Start();
        socketManager.CreateHostSocket(QosType.ReliableSequenced, newSocket => socket = newSocket);
    }
}
