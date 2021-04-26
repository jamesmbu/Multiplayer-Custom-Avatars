using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager_Delegates : MonoBehaviour
{
    public delegate void GameEvent();
    public event GameEvent EventGameOver;
    public event GameEvent EventGameStart;
    
    public void CallEventOnStart()
    {
        if(EventGameStart != null)
        {
            EventGameStart();
        }
    }
}
