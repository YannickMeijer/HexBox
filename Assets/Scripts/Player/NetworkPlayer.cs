using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkPlayer : MonoBehaviour
{
    protected SocketManager socketManager;
    protected Socket socket;

    protected PlayerData playerData;
    protected List<PlayerData> players = new List<PlayerData>();

    protected virtual void Start()
    {
        socketManager = GameObject.Find("Network").GetComponent<SocketManager>();
    }

    public void Send(string text)
    {
        socket.Send(new TextNetworkData("From player " + playerData.Id + ":\n" + text));
    }

    public PlayerData PlayerData
    {
        get { return playerData; }
    }
}
