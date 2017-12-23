using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class HostSocket
{
    public delegate void NetworkConnectedEventHandler(int connectionId);
    public event NetworkConnectedEventHandler OnIncomingConnection;

    public delegate void NetworkEventHandler();
    public event NetworkEventHandler OnConnected;
    public event NetworkEventHandler OnConnectionFailed;
    public event NetworkEventHandler OnDisconnected;

    protected readonly int channelId;
    protected readonly int socketId; // Aka hostId.
    protected int connectionId; // Set when the connection is initiated.
    protected bool connected;

    // Network data event handlers.
    private Dictionary<Type, List<Action<NetworkData>>> dataHandlers = new Dictionary<Type, List<Action<NetworkData>>>();

    public HostSocket(ConnectionConfig config, QosType qosType)
    {
        // TODO: temporary code to be able to test 2 instances on 1 machine.
        // Need to figure out something so this works both local and remote.
        int hostPort = 25565;
        if (Debug.isDebugBuild)
        {
            Debug.Log("Debug build, switching port numbers.");
            hostPort = 25564;
        }

        // Add the channel, open the socket.
        channelId = config.AddChannel(qosType);
        Debug.Log("Created " + qosType + " channel, id: " + channelId);
        socketId = NetworkTransport.AddHost(new HostTopology(config, 1), hostPort);
        Debug.Log("Opened socket on port " + hostPort + ", id: " + socketId);

        // Hook debug messages into the network events.
        OnConnected += () => Debug.Log("Connected.");
        OnConnectionFailed += () => Debug.Log("Could not connect.");
        OnData<TextNetworkData>(Debug.Log);

        // Hook into the incoming connection event.
        OnIncomingConnection += connectionId =>
        {
            Debug.Log("Incoming connection.");
            this.connectionId = connectionId;
            connected = true;
        };
    }

    /// <summary>
    /// Disconnect the socket.
    /// </summary>
    public void Disconnect()
    {
        byte errorByte;
        NetworkTransport.Disconnect(socketId, connectionId, out errorByte);
        NetworkController.LogNetworkError(errorByte);
        FireEvent(OnDisconnected);
        connected = false;
    }

    /// <summary>
    /// Handle incoming data.
    /// </summary>
    /// <param name="data">A buffer containing the data.</param>
    /// <param name="size">The size of the data.</param>
    public void HandleData(byte[] data, int size)
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

    /// <summary>
    /// Send an object (JSON encoded) over this socket.
    /// </summary>
    /// <param name="data">The object to send.</param>
    public void Send(NetworkData data)
    {
        Send(JsonUtility.ToJson(data));
    }

    /// <summary>
    /// Send data over this socket.
    /// </summary>
    /// <param name="data">The data to send.</param>
    private void Send(string data)
    {
        byte[] bytes = Encoding.UTF8.GetBytes(data);
        byte errorByte;
        NetworkTransport.Send(socketId, connectionId, channelId, bytes, bytes.Length, out errorByte);
        NetworkController.LogNetworkError(errorByte);
    }

    public void FireOnConnected()
    {
        FireEvent(OnConnected);
    }

    public void FireOnIncomingConnection(int connectionId)
    {
        // OnIncomingConnection is never null because it's used to receive the connection id.
        OnIncomingConnection(connectionId);
        // Also fire the Connected event.
        FireEvent(OnConnected);
    }

    public void FireOnConnectionFailed()
    {
        FireEvent(OnConnectionFailed);
    }

    public void FireOnDisconnected()
    {
        FireEvent(OnDisconnected);
    }

    private void FireEvent(NetworkEventHandler networkEvent)
    {
        if (networkEvent != null)
            networkEvent();
    }

    public int ConnectionId
    {
        get { return connectionId; }
    }

    public bool IsConnected
    {
        get { return connected; }
    }
}
