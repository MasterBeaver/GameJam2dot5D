using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steamworks;
using Mirror;
using TMPro;

public class LobbySteam : MonoBehaviour
{
  [SerializeField] private GameObject button;

  [SerializeField] private GameObject bg;

  [SerializeField] private TMP_Text text;

  private const string HostAdressKey = "HostAdress";

  protected Callback<LobbyCreated_t> lobbyCreated;
  protected Callback<GameLobbyJoinRequested_t> gameLobbyJoinRequested;  
  protected Callback<LobbyEnter_t> lobbyEnter;  
    private NetworkManager networkManager;
    public static CSteamID LobbyID { get; private set;}
    private void Start() 
    {
        networkManager = GetComponent<NetworkManager>();

        if (!SteamManager.Initialized) { return; }

        lobbyCreated = Callback<LobbyCreated_t>.Create(OnLobbyCreated);
        gameLobbyJoinRequested = Callback<GameLobbyJoinRequested_t>.Create(OnGameLobbyJoinRequested);
        lobbyEnter = Callback<LobbyEnter_t>.Create(OnLobbyEnter);
        text.text = "Started";
    }
    public void HostLobby()
    {
        bg.SetActive(false);
        button.SetActive(false);

        SteamMatchmaking.CreateLobby(ELobbyType.k_ELobbyTypeFriendsOnly, 10);
        text.text = "HostLobby";
    }

    private void OnLobbyCreated(LobbyCreated_t callback)
    {
        if(callback.m_eResult != EResult.k_EResultOK)
        {
            bg.SetActive(false);
            button.SetActive(true);
            return;
        }

        LobbyID = new CSteamID(callback.m_ulSteamIDLobby);

        networkManager.StartHost();

        SteamMatchmaking.SetLobbyData(LobbyID, HostAdressKey, SteamUser.GetSteamID().ToString());

        text.text = "LobbyCreated";
    
    }   

    private void OnGameLobbyJoinRequested(GameLobbyJoinRequested_t callback)
    {
        SteamMatchmaking.JoinLobby(callback.m_steamIDLobby);

        text.text = "JoinRequested";
    } 

    private void OnLobbyEnter(LobbyEnter_t callback)
    {
        if(NetworkServer.active) { return; }

        string hostAdress = SteamMatchmaking.GetLobbyData(
            new CSteamID(callback.m_ulSteamIDLobby), HostAdressKey
        );

        networkManager.networkAddress = hostAdress;
        networkManager.StartClient();

        text.text = "LobbyEnter";
        bg.SetActive(false);
        button.SetActive(false);
        
    }   
}
