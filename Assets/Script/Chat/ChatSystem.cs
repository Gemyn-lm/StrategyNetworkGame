using UnityEngine;
using Unity.Netcode;
using TMPro;
using WebSocketSharp;
using Unity.Collections;

public class ChatSystem : NetworkBehaviour
{
    #region Singleton
    public static ChatSystem Singleton { get; private set; }

    private void Awake()
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

    [SerializeField] private TMP_InputField chatInput;
    [SerializeField] private TMP_Text chatLog;

    public NetworkPlayer localPlayer;

    

    public override void OnNetworkSpawn()
    {
        chatInput = GameObject.Find("Chat Input").GetComponent<TMP_InputField>();
        chatLog = GameObject.Find("Chat Log").GetComponent<TMP_Text>();

        chatInput.onEndEdit.AddListener(OnChatMessageSend);

        NetworkManager.Singleton.OnConnectionEvent
            += (NetworkManager networkManager, ConnectionEventData data) =>
            {
                localPlayer = networkManager.LocalClient.PlayerObject.GetComponent<NetworkPlayer>();
            };
    }

    private void OnChatMessageSend(string message)
    {
        if (Input.GetKey(KeyCode.Return))
        {
            SendChatMessage_ServerRPC(localPlayer.networkPlayerName.Value.ToString(), message);
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void SendChatMessage_ServerRPC(string playerName, string message)
    {
        // Control message content
        if (message.IsNullOrEmpty())
            return;

        DistributeChatMessage_ClientRPC(playerName, message);
    }

    [ClientRpc]
    private void DistributeChatMessage_ClientRPC(string playerName, string message)
    {
        chatLog.text += "[" + playerName + "]" + " : " + message + "\n";
    }
}
