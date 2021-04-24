using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class PlayerSpawnpoints : MonoBehaviourPunCallbacks
{
    [SerializeField]private PlayerSpawnManager psm;
    public bool isSpawnable;
    void Start()
    {
        isSpawnable = true;
     
    }
    public override void OnEnable()
    {
        base.OnEnable();
        psm.PlayerSpawnedEvent += onSpawn;
    }

    public override void OnDisable()
    {
        base.OnDisable();
        psm.PlayerSpawnedEvent -= onSpawn;
    }

    // to prevent multiple player spawning in the same spot
    [PunRPC]
    public void onSpawn()
    {
      //  Debug.Log("Delegate called onSpawn");
        isSpawnable = false;
        StartCoroutine("ResetSpawnPoint");
    }

    private IEnumerator ResetSpawnPoint()
    {
        yield return new WaitForSeconds(5);
       // Debug.Log(" Time for respawnPoint " + Time.time);
        isSpawnable = true;
   
    }
}
