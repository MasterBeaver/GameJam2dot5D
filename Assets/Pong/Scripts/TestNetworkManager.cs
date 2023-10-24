using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System.Linq;
using Steamworks;

public class TestNetworkManager : NetworkManager
{
    [SerializeField] private string notificationMessage = string.Empty;

    private static List<Transform> spawnPoint = new List<Transform>();
   
    public Transform leftRacketSpawn;
    public Transform rightRacketSpawn;
    public override void OnStartServer()
    {
        // ServerChangeScene("TestSteam");
    }

    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        // base.OnServerAddPlayer(conn);
        Transform start = numPlayers == 0 ? leftRacketSpawn : rightRacketSpawn;
        GameObject player = Instantiate(playerPrefab, start.position, start.rotation);
        NetworkServer.AddPlayerForConnection(conn, player);
        PlayerManeger playerManeger = player.GetComponent<PlayerManeger>();

        Debug.Log("AddedPlayer");

        CSteamID steamID = SteamMatchmaking.GetLobbyMemberByIndex(
            LobbySteam.LobbyID, numPlayers - 1);

        var playerInfoDisplay = conn.identity.GetComponent<PlayerInfoDisplay>();

        playerInfoDisplay.SetSteamId(steamID.m_SteamID);

        if (numPlayers == 2)
        {
            playerManeger.CmdDealCards();
        }
    }
    

    [ContextMenu("Send Notification")]
    private void SendNotificatiob()
    {
        NetworkServer.SendToAll(new Notification { content = ""});
    }

    
}
