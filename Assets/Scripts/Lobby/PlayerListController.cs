using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerListController : MonoBehaviour
{
    public GameObject PlayerListItemPrefab;

    // Player id to list item.
    private Dictionary<int, GameObject> listItems = new Dictionary<int, GameObject>();

    private void Start()
    {
        // Hook into events from the network player.
        NetworkPlayer player = GameObject.Find("Network").GetComponent<NetworkPlayer>();
        player.OnPlayerConnected += AddPlayer;
        player.OnPlayerDisconnected += RemovePlayer;

        // Add the local player to the list.
        AddPlayer(player.PlayerData);
    }

    private void AddPlayer(PlayerData player)
    {
        // Create the list object, apply the player data to it.
        GameObject newListItem = Instantiate(PlayerListItemPrefab, transform);
        newListItem.name = player.Id + ' ' + player.Name;
        newListItem.GetComponentInChildren<Text>().text = player.Name;

        listItems.Add(player.Id, newListItem);
    }

    private void RemovePlayer(PlayerData player)
    {
        Destroy(listItems[player.Id]);
        listItems.Remove(player.Id);
    }
}
