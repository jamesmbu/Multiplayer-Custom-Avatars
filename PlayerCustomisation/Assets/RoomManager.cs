using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using System.Linq;

public class RoomManager : MonoBehaviourPunCallbacks
{
    public static RoomManager Instance;
   public GameObject StartButton;
    public Text RoomName;
    public GameObject PlayNameTextInfo;
    public GameObject LobbyNameButtonInfo;
    public Transform PlayerNameContent;
    public Transform LobbyNameButtonContent;
    public string LobbyRoomScreen;

    private void Start()
    {
        Instance = this;
    }
    public void EventOnClickStartButton()
    {
        StartButton.SetActive(false);
        StartGame();
        Debug.Log("Starting Game...");
    }
    void StartGame()
    {
        if(PhotonNetwork.IsMasterClient && PhotonNetwork.CountOfPlayersInRooms == LobbyManager.Instance.GetRoomSize())
        {
            PhotonNetwork.LoadLevel(1);
        }
    }

    public override void OnEnable()
    {
        base.OnEnable();
        PhotonNetwork.AddCallbackTarget(this);
    }
    public override void OnDisable()
    {
        base.OnDisable();
        PhotonNetwork.RemoveCallbackTarget(this);
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        base.OnRoomListUpdate(roomList);
        foreach(Transform Names in LobbyNameButtonContent)
        {
            Destroy(Names.gameObject);
        }

        for(int i=0; i < roomList.Count; i++)
        {
            if (roomList[i].RemovedFromList)
                continue;
            Instantiate(LobbyNameButtonInfo, LobbyNameButtonContent).GetComponent<LobbyNameButtonInfo>().SetUp(roomList[i]);
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);
        Instantiate(PlayNameTextInfo, PlayerNameContent).GetComponent<PlayNameTextInfo>().OnCreate(newPlayer);
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        ScreenUIManager.instance.OpenScreen(LobbyRoomScreen);
      
        RoomName.text = PhotonNetwork.CurrentRoom.Name;
        Player[] players = PhotonNetwork.PlayerList;

        // clear the list before creating a new list
        foreach (Transform child in PlayerNameContent)
        {
            Destroy(child.gameObject);
        }
        // create a new list
        for (int i = 0; i < players.Count(); i++)
        {
            Instantiate(PlayNameTextInfo, PlayerNameContent).GetComponent<PlayNameTextInfo>().OnCreate(players[i]);
        }
        // so only the Host/ master client can click this and can only be clicked when Roomsize it met
        StartButton.SetActive(PhotonNetwork.IsMasterClient && PhotonNetwork.CountOfPlayersInRooms == LobbyManager.Instance.GetRoomSize());
    }
}
