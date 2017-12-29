using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOptionsWatcher : MonoBehaviour
{
    public Slider PlayerCountSlider;

    private GameOptions options = new GameOptions();
    private HostPlayer host;

    private void Start()
    {
        // Set up host- and client-specific events.
        GameObject network = GameObject.Find("Network");
        host = network.GetComponent<HostPlayer>();
        ClientPlayer client = network.GetComponent<ClientPlayer>();
        if (client != null)
            client.GameOptionsChanged += ApplyOptions;

        // Hook into all events.
        PlayerCountSlider.onValueChanged.AddListener(value =>
        {
            options.PlayerCount = (int)value;
            SetOptionsOnHost();
        });
    }

    private void ApplyOptions(GameOptions options)
    {
        this.options = options;
        PlayerCountSlider.value = options.PlayerCount;
    }

    private void SetOptionsOnHost()
    {
        if (host != null)
            host.SetGameOptions(options);
    }
}
