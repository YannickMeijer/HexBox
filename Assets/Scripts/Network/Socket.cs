using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class Socket
{
    private readonly int channelId;
    private readonly int socketId; // Aka hostId.
    private readonly int connectionId;

    public Socket(HostTopology topology, ConnectionConfig config, QosType qosType, string address, int port)
    {
        // TODO: temporary code to be able to test 2 instances on 1 machine.
        // Need to figure out something so this works both local and remote.
        port = 8888;
        int connectPort = 8889;
        if (Debug.isDebugBuild)
        {
            Debug.Log("Debug build, switching port numbers.");
            port = 8889;
            connectPort = 8888;
        }

        // Open the socket.
        channelId = config.AddChannel(qosType);
        Debug.Log("Created " + qosType + " channel, id: " + channelId);
        socketId = NetworkTransport.AddHost(topology, port);
        Debug.Log("Opened socket on port " + port + ", id: " + socketId);

        // Connect.
        byte errorByte;
        connectionId = NetworkTransport.Connect(socketId, address, connectPort, 0, out errorByte);
        Debug.Log("Connecting to " + address + ":" + connectPort + ", id: " + connectionId);
        NetworkController.LogNetworkError(errorByte);
    }

    /// <summary>
    /// Send data over this socket.
    /// </summary>
    /// <param name="data">The data to send.</param>
    public void Send(string data)
    {
        byte[] bytes = Encoding.UTF8.GetBytes(data);
        byte errorByte;
        NetworkTransport.Send(socketId, connectionId, channelId, bytes, bytes.Length, out errorByte);
        NetworkController.LogNetworkError(errorByte);
    }

    public int ConnectionId
    {
        get { return connectionId; }
    }
}
