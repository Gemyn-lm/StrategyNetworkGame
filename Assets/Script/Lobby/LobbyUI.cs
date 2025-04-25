using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using WebSocketSharp;
using System.Collections.Generic;

public class LobbyUI : MonoBehaviour
{
    #region Singleton

    public static LobbyUI Singleton {  get; private set; }

    private void SetUpSingleton()
    {
        if (Singleton != null && Singleton != this)
        {
            Destroy(this);
        }
        else
        {
            Singleton = this;
        }
    }

    #endregion

    [SerializeField] private Button hostButton;
    [SerializeField] private Button joinButton;
    [SerializeField] private Button readyButton;
    [SerializeField] private Button startButton;
    [SerializeField] private TMP_InputField playerInput;
    [SerializeField] private GameObject playerLobbyDisplayPrefab;
    [SerializeField] private Transform playerInfoLobbyParent;

    public NetworkPlayer localPlayer;

    public List<int> clientIDList = new List<int>();

    public List<NetworkPlayer> playerList = new List<NetworkPlayer>();

    public List<PlayerLobbyInfoDisplay> playerInfoList = new List<PlayerLobbyInfoDisplay>();


    private void Awake()
    {
        SetUpSingleton();

        hostButton.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.StartHost();
        });
        joinButton.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.StartClient();
        });


    }

    private void Start()
    {
        NetworkManager.Singleton.OnConnectionEvent
            += (NetworkManager networkManager, ConnectionEventData data) =>
            {
                localPlayer = networkManager.LocalClient.PlayerObject.GetComponent<NetworkPlayer>();
                startButton.gameObject.SetActive(localPlayer.IsHost);
                readyButton.gameObject.SetActive(true);
            };

        NetworkManager.Singleton.OnConnectionEvent += OnConnection;
        readyButton.onClick.AddListener(SetLocalPlayerReady);
        startButton.onClick.AddListener(OnStartGame);
    }

    private void OnConnection(NetworkManager networkManager, ConnectionEventData data)
    {
        if(localPlayer.IsHost && data.EventType == ConnectionEvent.ClientConnected)
        {
            NetworkPlayer newPlayer = networkManager.ConnectedClients[data.ClientId].PlayerObject.GetComponent<NetworkPlayer>();
            playerList.Add(newPlayer);
            newPlayer.isReady.OnValueChanged += OnPlayerReadyIsChanged;
            OnPlayerReadyIsChanged(false, false);
        }

        clientIDList.Clear();
        foreach (ulong id in networkManager.ConnectedClientsIds)
        {
            clientIDList.Add((int)id);
        }
        PopulatePlayerList();
    }

    public void SetLocalPlayerReady()
    {
        localPlayer.isReady.Value = !localPlayer.isReady.Value;
    }

    private void PopulatePlayerList()
    {
        foreach (PlayerLobbyInfoDisplay child in playerInfoList)
        {
            Destroy(child.gameObject);
        }
        playerInfoList.Clear();

        foreach (ulong id in clientIDList)
        {
            PlayerLobbyInfoDisplay pnd = Instantiate(playerLobbyDisplayPrefab, playerInfoLobbyParent.transform).GetComponent<PlayerLobbyInfoDisplay>();
            pnd.affiliatedPlayer = NetworkManager.Singleton.ConnectedClients[id].PlayerObject.GetComponent<NetworkPlayer>();
            playerInfoList.Add(pnd);
        }

    }

    private void OnPlayerReadyIsChanged(bool oldValue, bool newValue)
    {
        bool everyoneReady = true;
        foreach (NetworkPlayer player in playerList)
        {
            if(!player.isReady.Value)
            {
                everyoneReady = false;
                break;
            }
        }
        
        startButton.interactable = everyoneReady;
    }

    public string GetPlayerNameInput()
    {
        return playerInput.text;
    }

    private void OnStartGame()
    {
        
    }

}
