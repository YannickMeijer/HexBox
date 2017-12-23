using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ClientPlayer : MonoBehaviour
{
    private Socket socket;

    private void Start()
    {
        socket = GameObject.Find("Network").GetComponent<SocketManager>().CreateClientSocket(QosType.ReliableSequenced, "127.0.0.1");
    }
}
