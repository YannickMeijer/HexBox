using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ClientSocket : HostSocket
{
    public ClientSocket(ConnectionConfig config, QosType qosType, string address) : base(config, qosType)
    {
        // TODO: temporary code to be able to test 2 instances on 1 machine.
        // Need to figure out something so this works both local and remote.
        int connectPort = 25564;
        if (Debug.isDebugBuild)
        {
            Debug.Log("Debug build, switching port numbers.");
            connectPort = 25565;
        }

        // Connect.
        byte errorByte;
        connectionId = NetworkTransport.Connect(socketId, address, connectPort, 0, out errorByte);
        Debug.Log("Connecting to " + address + ":" + connectPort + ", id: " + connectionId);
        NetworkController.LogNetworkError(errorByte);
    }
}
