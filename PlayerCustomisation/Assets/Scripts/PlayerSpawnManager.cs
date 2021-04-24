using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawnManager : MonoBehaviour
{
    public static PlayerSpawnManager instance;
    //public static PlayerSpawnManager instance;
    PlayerSpawnpoints[] points;
   public delegate void SpawnDelegate();
   public event SpawnDelegate PlayerSpawnedEvent;


    public void CallEventPLayerSpawn()
    {
        if(PlayerSpawnedEvent != null)
        {
            PlayerSpawnedEvent();
        }
    }
    private void Awake()
    {
        instance = this;
        points = GetComponentsInChildren<PlayerSpawnpoints>();
    }

    public Transform GetTransform()
    {
        PlayerSpawnpoints thisPoint = points[Random.Range(0, points.Length)];
        if (thisPoint.isSpawnable)
            return thisPoint.transform;
        else
            return transform;
        
    }


}
