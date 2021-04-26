using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyNameButtonInfo : MonoBehaviour
{
	[SerializeField] private Text text;

	public RoomInfo info;

	public void SetUp(RoomInfo ThisRoominfo)
	{
		info = ThisRoominfo;
		text.text = ThisRoominfo.Name;
	}

	public void OnClick()
	{
		LobbyManager.Instance.EventJoinARoom(info);
	}
}
