using Unity.Netcode;
using UnityEngine.Networking;
using UnityEngine;

public class NetworkPlayer : NetworkBehaviour
{

    public NetworkVariable<string> playerName = new NetworkVariable<string>("No Name");

    public override void OnNetworkSpawn()
    {
        if(IsOwner)
        {

        }
    }
}
