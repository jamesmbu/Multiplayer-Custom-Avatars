using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using System;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    public static LobbyManager Instance;
    [Range(1,16)]
    [SerializeField] private int RoomSize;
    [SerializeField] private InputField RoomNameInput;
    [SerializeField] private InputField RoomSizeInput;
    [SerializeField] private InputField PlayerNameInput;

    [SerializeField] private string LoadingScreen;
    [SerializeField] private string SetPlayerNameScreenName;
    [SerializeField] private string MenuScreenName;
    [SerializeField] private string CreateARoomScreenName;
    [SerializeField] private string FindARoomScreenName;
    [SerializeField] private string LobbyScreenName;

    private void Start()
    {
        Instance = this;
    }

    public int GetRoomSize()
    {
        return RoomSize;
    }
    public override void OnConnectedToMaster()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
    }
    public override void OnJoinedLobby()
    {
        Debug.Log("YEP we are in a lobby!");
        ScreenUIManager.instance.OpenScreen(SetPlayerNameScreenName);
    }
    public void SetPlayerName()
    {
        if (string.IsNullOrEmpty(PlayerNameInput.text))
        {
            return;
        }
        PhotonNetwork.NickName = PlayerNameInput.text;
       // goes to the meny screen
        ScreenUIManager.instance.OpenScreen(MenuScreenName);
    }

    public void EventOpenCreateARoomScreen()
    {
        ScreenUIManager.instance.OpenScreen(CreateARoomScreenName);
    }
    public void EventCreateRoom()
    {
        if (string.IsNullOrEmpty(RoomNameInput.text))
            return;
        if (string.IsNullOrEmpty(RoomSizeInput.text))
            return;
        int.TryParse(RoomSizeInput.text, out RoomSize);
        RoomOptions roomOptions = new RoomOptions() 
        { IsVisible = true, IsOpen = true, MaxPlayers = (byte)RoomSize };
        PhotonNetwork.CreateRoom(RoomNameInput.text, roomOptions);
        //Loading screen
        ScreenUIManager.instance.OpenScreen(LoadingScreen);
    }
    public void createARandomRoom()
    {
        Debug.Log("Creating a Random room");
        int RandomNum = UnityEngine.Random.Range(0, 10000);
        RoomOptions roomOptions = new RoomOptions() { IsVisible = true, IsOpen = true, MaxPlayers = (byte)RoomSize };
        PhotonNetwork.CreateRoom("Room" + RandomNum, roomOptions);
        Debug.Log(RandomNum);
        ScreenUIManager.instance.OpenScreen(LoadingScreen);
    }

    public void EventJoinARoom(RoomInfo _Info)
    {
        PhotonNetwork.JoinRoom(_Info.Name);
        ScreenUIManager.instance.OpenScreen(LoadingScreen);
    }
    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
        ScreenUIManager.instance.OpenScreen(LoadingScreen);
    }
    public void EventLeftTheRoom()
    {
        ScreenUIManager.instance.OpenScreen(MenuScreenName);

    }    
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        createARandomRoom();
    }
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        createARandomRoom();
    }
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        createARandomRoom();
    }

}
