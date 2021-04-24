using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawnpoints : MonoBehaviour
{
    public bool isSpawnable;
    void Start()
    {
        isSpawnable = true;
    }

    // to prevent multiple player spawning in the same spot
   public void onSpawn()
    {
        isSpawnable = false;
        StartCoroutine("ResetSpawnPoint");
    }

    private IEnumerator ResetSpawnPoint()
    {
        yield return new WaitForSeconds(5);
        isSpawnable = true;
    }
}
