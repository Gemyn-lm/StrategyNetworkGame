using NUnit.Framework;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using System.Collections.Generic;

public class PlayerLobbyDisplay : MonoBehaviour
{
    [SerializeField] private GameObject playerLobbyDisplayPrefab;
    public List<int> clientIDList = new List<int>();

    private void Start()
    {
        NetworkManager.Singleton.OnConnectionEvent += OnConnection;
    }

    private void OnConnection(NetworkManager networkManager, ConnectionEventData data)
    {
        clientIDList.Clear();
        foreach (ulong id in networkManager.ConnectedClientsIds)
        {
            clientIDList.Add((int)id);
        }
    }
}
