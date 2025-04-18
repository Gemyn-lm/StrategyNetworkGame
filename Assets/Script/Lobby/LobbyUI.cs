using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using WebSocketSharp;

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
    [SerializeField] private TMP_InputField playerInput;
    [SerializeField] private Button readyButton;

    [SerializeField] private NetworkPlayer localPlayer;


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
            };
    }

    public string GetPlayerNameInput()
    {
        return playerInput.text;
    }

}
