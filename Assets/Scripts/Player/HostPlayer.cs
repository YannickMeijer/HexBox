using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class HostPlayer : MonoBehaviour
{
    // TODO: support multiple clients by having a dedicated socket for assigning port numbers.
    private Socket socket;

    private void Start()
    {
        GameObject.Find("GameController").GetComponent<SocketManager>().CreateHostSocket(QosType.ReliableSequenced, newSocket => socket = newSocket);
    }
}
