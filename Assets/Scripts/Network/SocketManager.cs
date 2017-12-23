using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class SocketManager : MonoBehaviour
{
    private const int BUFFER_SIZE = 1024;

    private readonly ConnectionConfig config = new ConnectionConfig();

    // SocketId to socket instance.
    private Dictionary<int, Socket> sockets = new Dictionary<int, Socket>();
    private Socket newSocket; // The next connecting socket.
    private Action<Socket> newSocketCreated; // The callback for when the new socket is connected.
    private Dictionary<Func<Socket>, Action<Socket>> socketCreationList = new Dictionary<Func<Socket>, Action<Socket>>(); // The sockets to be created.

    public static void LogNetworkError(byte errorByte)
    {
        NetworkError error = (NetworkError)errorByte;
        // Check if the error is an ok, or message too long (which is handled).
        if (error != NetworkError.Ok && error != NetworkError.MessageToLong)
            Debug.LogError("A network error occurred: " + error);
    }

    private void Start()
    {
        NetworkTransport.Init();
    }

    public void CreateHostSocket(QosType qosType, Action<Socket> callback)
    {
        socketCreationList.Add(() => new Socket(config, qosType), callback);
    }

    public ClientSocket CreateClientSocket(QosType qosType, string address)
    {
        ClientSocket socket = new ClientSocket(config, qosType, address);
        sockets.Add(socket.ConnectionId, socket);
        return socket;
    }

    private void Update()
    {
        CreateNewSocket();
        ReceiveNetworkEvent();
    }

    private void CreateNewSocket()
    {
        // Check if the previous socket has connected and if there are more sockets to connect.
        if (newSocket == null && socketCreationList.Count > 0)
        {
            // Get an item from the collection, resolve it.
            KeyValuePair<Func<Socket>, Action<Socket>> entry = socketCreationList.First();
            newSocket = entry.Key();
            newSocketCreated = entry.Value;
            socketCreationList.Remove(entry.Key);
        }
    }

    private void ReceiveNetworkEvent()
    {
        int hostId;
        int connectionId;
        int channelId;
        byte[] buffer = new byte[BUFFER_SIZE];
        int receivedSize;
        byte errorByte;

        NetworkEventType networkEvent = NetworkTransport.Receive(out hostId, out connectionId, out channelId, buffer, BUFFER_SIZE, out receivedSize, out errorByte);
        LogNetworkError(errorByte);

        // Check if the connectionId corresponds with a client.
        Socket connectedSocket;
        sockets.TryGetValue(connectionId, out connectedSocket);

        switch (networkEvent)
        {
            case NetworkEventType.ConnectEvent:
                // New connection, check if it is incoming or outgoing.
                if (connectedSocket != null)
                    connectedSocket.FireOnConnected();
                else
                {
                    FireSocketCreated();
                    newSocket.FireOnIncomingConnection(connectionId);
                    sockets.Add(newSocket.ConnectionId, newSocket);
                    newSocket = null;
                }
                break;
            case NetworkEventType.DisconnectEvent:
                // Disconnected.
                if (connectedSocket != null)
                {
                    connectedSocket.FireOnConnectionFailed();
                    sockets.Remove(connectionId);
                }
                else
                {
                    FireSocketCreated();
                    newSocket.FireOnDisconnected();
                    newSocket = null;
                }
                break;
            case NetworkEventType.DataEvent:
                // Check if the buffer was big enough, otherwise repeat the call.
                if (((NetworkError)errorByte) == NetworkError.MessageToLong)
                {
                    buffer = new byte[receivedSize];
                    NetworkTransport.Receive(out hostId, out connectionId, out channelId, buffer, receivedSize, out receivedSize, out errorByte);
                    LogNetworkError(errorByte);
                }

                // Data!
                connectedSocket.HandleData(buffer, receivedSize);
                break;
            case NetworkEventType.BroadcastEvent:
            case NetworkEventType.Nothing:
                // Do nothing.
                break;
            default:
                Debug.LogError("Unknown message format received: " + networkEvent);
                break;
        }
    }

    private void FireSocketCreated()
    {
        if (newSocketCreated != null)
            newSocketCreated(newSocket);
    }
}
