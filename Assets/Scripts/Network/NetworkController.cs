using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkController : MonoBehaviour
{
    private const int PORT = 8888;

    public delegate void NetworkEventHandler();
    public event NetworkEventHandler OnConnected;
    public event NetworkEventHandler OnIncomingConnection;
    public event NetworkEventHandler OnConnectionFailed;
    public event NetworkEventHandler OnConnectionClosed;

    public delegate void NetworkDataHandler(string data);
    public event NetworkDataHandler OnData;

    private Socket reliableSocket;

    public static void LogNetworkError(byte errorByte)
    {
        NetworkError error = (NetworkError)errorByte;
        if (error != NetworkError.Ok)
            Debug.LogError("A network error occurred: " + error);
    }

    private void Start()
    {
        // Initialize and configure the network.
        NetworkTransport.Init();
        ConnectionConfig config = new ConnectionConfig();

        reliableSocket = new Socket(config, QosType.Reliable, "127.0.0.1");

        // Hook debug messages into the network events.
        OnConnected += () => Debug.Log("Connected.");
        OnIncomingConnection += () => Debug.Log("Incoming connection.");
        OnConnectionFailed += () => Debug.Log("Could not connect.");
        OnConnectionClosed += () => Debug.Log("Connection closed.");

        OnData += data => Debug.Log("Incoming data:\n" + data);
    }

    private void Update()
    {
        int hostId;
        int connectionId;
        int channelId;
        int bufferSize = 1024;
        byte[] buffer = new byte[bufferSize];
        int receivedSize;
        byte errorByte;

        NetworkEventType networkEvent = NetworkTransport.Receive(out hostId, out connectionId, out channelId, buffer, bufferSize, out receivedSize, out errorByte);
        LogNetworkError(errorByte);

        switch (networkEvent)
        {
            case NetworkEventType.ConnectEvent:
                // New connection, either incoming or outgoing.
                if (reliableSocket.ConnectionId == connectionId)
                    OnConnected();
                else
                    OnIncomingConnection();
                break;
            case NetworkEventType.DisconnectEvent:
                // Disconnected.
                if (reliableSocket.ConnectionId == connectionId)
                    OnConnectionFailed();
                else
                    OnConnectionClosed();
                break;
            case NetworkEventType.DataEvent:
                // Data!
                string bufferString = Encoding.UTF8.GetString(buffer, 0, receivedSize);
                if (OnData != null)
                    OnData(bufferString);
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

    public Socket ReliableSocket
    {
        get { return reliableSocket; }
    }
}
