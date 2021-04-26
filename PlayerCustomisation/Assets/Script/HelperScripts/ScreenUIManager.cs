using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class ScreenUIManager : MonoBehaviourPunCallbacks
{
	public static ScreenUIManager instance;
	[SerializeField] UiMenuScreen[] Screens;

    private void Start()
    {
		instance = this;
    }
    public void OpenScreen(string ScreenName)
	{
		for (int i = 0; i < Screens.Length; i++)
		{
			if (Screens[i].MenuScreenName == ScreenName)
			{
				Screens[i].Open();
			}
			else if (Screens[i].open)
			{
				CloseScreen(Screens[i]);
			}
		}
	}

	public void OpenScreen(UiMenuScreen Screen)
	{
		for (int i = 0; i < Screens.Length; i++)
		{
			if (Screens[i].open)
			{
				CloseScreen(Screens[i]);
			}
		}
		Screen.Open();
	}

	public void CloseScreen(UiMenuScreen Screen)
	{
		Screen.Close();
	}
}
