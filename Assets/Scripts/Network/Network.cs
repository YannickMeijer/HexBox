using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Network : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
    }

    public void HostLobby()
    {
        gameObject.AddComponent<HostPlayer>();
        SceneManager.LoadScene("Scenes/Game");
    }

    public void Connect()
    {
        gameObject.AddComponent<ClientPlayer>();
        SceneManager.LoadScene("Scenes/Game");
    }
}
