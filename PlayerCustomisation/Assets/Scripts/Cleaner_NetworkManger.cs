using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Cleaner_NetworkManger : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    public static Cleaner_NetworkManger instance;

    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        /* Connects to the master servers */
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        /*Print that it's connected to the master server and the region */
        Debug.Log("OnConnectedToMaster() was called by PUN. connected to the " + PhotonNetwork.CloudRegion + " Server!");
        PhotonNetwork.JoinLobby();
        ScreenUIManager.instance.OpenScreen("Loading");
    }
}
