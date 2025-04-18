using Unity.Netcode;
using UnityEngine.Networking;
using UnityEngine;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;

public class NetworkPlayer : NetworkBehaviour
{

    public NetworkVariable<FixedString64Bytes> networkPlayerName
        = new NetworkVariable<FixedString64Bytes>("No Name", NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    public NetworkVariable<bool> isReady 
        = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    public override void OnNetworkSpawn()
    {
        if(IsOwner)
        {
            Debug.Log("wiriting on playerName");
            networkPlayerName.Value = LobbyUI.Singleton.GetPlayerNameInput();
            //ChatSystem.Singleton.localPlayer = this;
        }
        
        gameObject.name = "Player [" + networkPlayerName.Value + "]";
        networkPlayerName.OnValueChanged += NetworkPlayerName_OnValueChanged;
    }

    private void NetworkPlayerName_OnValueChanged(FixedString64Bytes oldValue, FixedString64Bytes newValue)
    {
        gameObject.name = "Player [" + newValue.ToString() + "]";
    }

}
