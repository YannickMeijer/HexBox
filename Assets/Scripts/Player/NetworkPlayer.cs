using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkPlayer : MonoBehaviour
{
    protected SocketManager socketManager;
    protected Socket socket;

    protected virtual void Start()
    {
        socketManager = GameObject.Find("Network").GetComponent<SocketManager>();
    }

    public void Send(string text)
    {
        socket.Send(new TextNetworkData(text));
    }
}
