using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawnManager : MonoBehaviour
{
    public static PlayerSpawnManager instance;
    PlayerSpawnpoints[] points;

    private void Awake()
    {
        instance = this;
        points = GetComponentsInChildren<PlayerSpawnpoints>();
    }

    public Transform GetTransform()
    {
        return points[Random.Range(0, points.Length)].transform;
    }


}
