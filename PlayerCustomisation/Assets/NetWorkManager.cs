using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;



public class NetworkManager : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private GameObject Player;

    [SerializeField]
    private Text nickname, status, room, players;

    [SerializeField]
    private Button buttonPlay, buttonLeave;
    [SerializeField]
    private InputField playerName;
    [SerializeField]
    private byte maxPlayersPerRoom = 4;


    // Start is called before the first frame update
    void Start()
    {
        status.text = "Connecting...";
        buttonPlay.gameObject.SetActive(false);
        buttonLeave.gameObject.SetActive(false);
        playerName.gameObject.SetActive(false);

        if (!PhotonNetwork.IsConnected)
            PhotonNetwork.ConnectUsingSettings();
    }

    public void Play()
    {
        PhotonNetwork.JoinRandomRoom();
        PlayerPrefs.SetString("PlayerName", playerName.text);
        PhotonNetwork.NickName = playerName.text;
    }

    // Update is called once per frame
    void Update()
    {

    }
}