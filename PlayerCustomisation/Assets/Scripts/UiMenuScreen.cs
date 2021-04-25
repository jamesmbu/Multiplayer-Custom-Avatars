using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class UiMenuScreen : MonoBehaviourPunCallbacks
{
	public static UiMenuScreen instance;
	public string MenuScreenName;
	public bool open;

    private void Start()
    {
		instance = this;
    }
    public void Open()
	{
		open = true;
		gameObject.SetActive(true);
	}

	public void Close()
	{
		open = false;
		gameObject.SetActive(false);
	}
}
