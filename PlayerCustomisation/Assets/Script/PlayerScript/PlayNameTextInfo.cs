using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using Photon.Realtime;

public class PlayNameTextInfo : MonoBehaviourPunCallbacks
{

    [SerializeField] private Text text;
    Player Player;
    public void OnCreate(Player M_Player)
    {
        Player = M_Player;
        text.text = Player.NickName;
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);
        if (Player == otherPlayer)
        {
            Destroy(this);
        }
    }

    public override void OnLeftRoom()
    {
        base.OnLeftRoom();
        Destroy(this);
    }
}
