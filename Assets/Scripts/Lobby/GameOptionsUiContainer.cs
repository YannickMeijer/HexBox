using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOptionsUiContainer : MonoBehaviour
{
    public Slider PlayerCountSlider;

    private void Start()
    {
        // Find the network player.
        GameObject.Find("Network").GetComponent<NetworkPlayer>().InitializeLobbyGameOptions(this);
    }

    /// <summary>
    /// Apply game options to the lobby, setting the UI elements accordingly.
    /// </summary>
    /// <param name="options">The new game options.</param>
    public void ApplyOptions(GameOptions options)
    {
        PlayerCountSlider.value = options.PlayerCount;
    }
}
