using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.IO;
using System;
using System.Reflection;

public class PlayerMaster : MonoBehaviourPunCallbacks
{
    public static PlayerMaster instance;
    public delegate void PlayerMasterEvent();
    public event PlayerMasterEvent EventSpawnPlayer;
    public event PlayerMasterEvent EventChangeToFPP;
    public event PlayerMasterEvent EventChangeToTopDownview;
    public event PlayerMasterEvent EventOnPlayerDeath;

    public delegate void Modifyer(float delta);
    public event Modifyer EventModifyHealth;
    public event Modifyer EventDamageTaken;

    public delegate void GunFire(RaycastHit hitPosition, Transform hitTransform);
    public event GunFire EventShotDefault;
    public event GunFire EventShotEnemy;

    [SerializeField] private PlayerSpawnManager playerSpawnManager;
    [SerializeField] private GameObject playerPreview;

    GameManager gameManager;
      PhotonView View;
    GameObject Thisplayer;
    GameObject testing;
    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>().GetComponent<GameManager>();
        instance = this;
        View = GetComponent<PhotonView>();
        playerSpawnManager = FindObjectOfType<PlayerSpawnManager>().GetComponent<PlayerSpawnManager>();
    }
    public void CallEventplayerspawn()
    {
        if(EventSpawnPlayer != null)
        {
            EventSpawnPlayer();
        }
    }
    public void CallEventDamageTake(float delta)
    {
        if(EventDamageTaken!= null)
        {
            EventDamageTaken(delta);
        }
    }

    public void CallEventOnPlayerDeath()
    {
        if (EventOnPlayerDeath != null)
            EventOnPlayerDeath();
    }
    public void CallEventChangeToFPP()
    {
        if (EventChangeToFPP != null)
            EventChangeToFPP();
    }

    public void CallEventChangedToTopDownView()
    {
        if (EventChangeToTopDownview != null)
            EventChangeToTopDownview();
    }

    public void CallEventModifyHealth(float delta)
    {
        if (EventModifyHealth != null)
            EventModifyHealth(delta);
    }
    public void CallEventShotDefault(RaycastHit hPos, Transform hTransform)
    {
        if (EventShotDefault != null)
            EventShotDefault(hPos,hTransform);
    }
    public void CallEventShotEnemy(RaycastHit hPos, Transform hTransform)
    {
        
        if (EventShotEnemy != null)
            EventShotEnemy(hPos,hTransform);
    }



    private void OnEnable()
    {
       
        EventSpawnPlayer += OnInitialized;
        EventOnPlayerDeath += Die;
    }
    private void OnDisable()
    {
       EventSpawnPlayer -= OnInitialized;
        EventOnPlayerDeath -= Die;
    }

    private void OnInitialized()
    {
        CreateThisPlayer();
        //photonView.RPC(nameof(CreateThisPlayer), RpcTarget.All);
    }

  //  [PunRPC]
    void CreateThisPlayer()
    {

        Transform spawnpoint = PlayerSpawnManager.instance.GetTransform();
        //  Debug.Log("spawn pos: " + spawnpoint.position+ "Spawn rot :" + spawnpoint.rotation);
        Thisplayer = null;
        // Instantiate new player
        Thisplayer = PhotonNetwork.Instantiate(Path.GetFileName("PlayerController"), spawnpoint.position, spawnpoint.rotation, 0, new object[] { View.ViewID });
        Thisplayer.transform.SetParent(transform);
       
        CharacterCustomisation PlayerCustomiser = Thisplayer.GetComponent<CharacterCustomisation>();
     
        // Customise the newly spawned player - send the saved settings from the preview to the new player
        playerPreview = gameManager.playerPreview;
        
        
        PlayerCustomiser.ApplySavedAppearance(playerPreview.GetComponent<CharacterCustomisation>().Save_Model);
        // Hide the preview version of the player
        playerSpawnManager.CallEventPLayerSpawn();
    }


    public void Die()=> StartCoroutine("RespawnPlayer");   
   

    private IEnumerator RespawnPlayer()
    {
        yield return new WaitForSeconds(5);
        CallEventplayerspawn();
    }

   
}
