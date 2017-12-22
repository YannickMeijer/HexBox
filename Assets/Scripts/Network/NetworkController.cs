using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkController : MonoBehaviour
{
    private const int BUFFER_SIZE = 1024;

    public delegate void NetworkEventHandler();
    public event NetworkEventHandler OnConnected;
    public event NetworkEventHandler OnIncomingConnection;
    public event NetworkEventHandler OnConnectionFailed;
    public event NetworkEventHandler OnConnectionClosed;

    private Dictionary<Type, List<Action<NetworkData>>> dataHandlers = new Dictionary<Type, List<Action<NetworkData>>>();

    private Socket reliableSocket;

    public static void LogNetworkError(byte errorByte)
    {
        NetworkError error = (NetworkError)errorByte;
        // Check if the error is an ok, or message too long (which is handled).
        if (error != NetworkError.Ok && error != NetworkError.MessageToLong)
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

        OnData<TextNetworkData>(Debug.Log);
    }

    private void Update()
    {
        int hostId;
        int connectionId;
        int channelId;
        byte[] buffer = new byte[BUFFER_SIZE];
        int receivedSize;
        byte errorByte;

        NetworkEventType networkEvent = NetworkTransport.Receive(out hostId, out connectionId, out channelId, buffer, BUFFER_SIZE, out receivedSize, out errorByte);
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
                // Check if the buffer was big enough, otherwise repeat the call.
                if (((NetworkError)errorByte) == NetworkError.MessageToLong)
                {
                    buffer = new byte[receivedSize];
                    NetworkTransport.Receive(out hostId, out connectionId, out channelId, buffer, receivedSize, out receivedSize, out errorByte);
                    LogNetworkError(errorByte);
                }

                // Data!
                HandleData(buffer, receivedSize);
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

    private void HandleData(byte[] data, int size)
    {
        string json = Encoding.UTF8.GetString(data, 0, size);
        string typeName = JsonUtility.FromJson<NetworkData>(json).Type;

        // Try to get the type of the object.
        Type type = Type.GetType(typeName);
        if (type == null)
        {
            Debug.Log("Invalid NetworkData object received:\n" + data);
            return;
        }

        // Check if there are listeners for this type.
        List<Action<NetworkData>> handlers;
        if (dataHandlers.TryGetValue(type, out handlers))
        {
            // Decode the data, call the handlers.
            NetworkData result = JsonUtility.FromJson(json, type) as NetworkData;
            handlers.ForEach(handler => handler.Invoke(result));
        }
    }

    /// <summary>
    /// Hook into a network data event.
    /// </summary>
    /// <typeparam name="T">The type of network data to listen for.</typeparam>
    /// <param name="handler">The event handler.</param>
    public void OnData<T>(Action<T> handler) where T : NetworkData
    {
        Type type = typeof(T);

        // Check if the type is already registered.
        if (!dataHandlers.ContainsKey(type))
            dataHandlers.Add(type, new List<Action<NetworkData>>());

        // This cast is safe because it is checked when firing the events.
        dataHandlers[type].Add(data => handler(data as T));
    }

    public Socket ReliableSocket
    {
        get { return reliableSocket; }
    }
}
