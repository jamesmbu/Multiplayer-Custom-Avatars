using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.IO;
public class PlayerMaster : MonoBehaviourPunCallbacks
{
    public static PlayerMaster instance;
    public delegate void PlayerMasterEvent();
    public event PlayerMasterEvent EventSpawnPlayer;
    public event PlayerMasterEvent EventChangeToFPP;
    public event PlayerMasterEvent EventChangeToTopDownview;
    public event PlayerMasterEvent EventOnPlayerDeath;

    public delegate void HealthModifyer(float delta);
    public event HealthModifyer EventModifyHealth;

    [SerializeField] private PlayerSpawnManager playerSpawnManager;
    [SerializeField] private GameObject playerPreview; 
   
    PhotonView View;
    GameObject Thisplayer;

    private void Awake()
    {
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

    private void OnEnable()
    {
            EventSpawnPlayer += CreateThisPlayer;
    }
    private void OnDisable()
    {
            EventSpawnPlayer -= CreateThisPlayer;
    }
    void CreateThisPlayer()
    {
        Transform spawnpoint = PlayerSpawnManager.instance.GetTransform();
        Debug.Log("spawn pos: " + spawnpoint.position+ "Spawn rot :" + spawnpoint.rotation);
       
        // Instantiate new player
       Thisplayer = PhotonNetwork.Instantiate(NetworkManager.instance.player.name, spawnpoint.position, spawnpoint.rotation, 0, new object[] { View.ViewID });
       Thisplayer.transform.SetParent(transform);

        // Customise the newly spawned player - send the saved settings from the preview to the new player
        playerPreview = NetworkManager.instance.playerPreview;

        CharacterCustomisation newPlayerCustomiser = Thisplayer.GetComponent<CharacterCustomisation>();
        newPlayerCustomiser.ApplySavedAppearance(playerPreview.GetComponent<CharacterCustomisation>().Save_Model);
        // Hide the preview version of the player
        playerSpawnManager.CallEventPLayerSpawn();
    }

    public void Die()
    {
        PhotonNetwork.Destroy(Thisplayer);
        StartCoroutine("RespawnPlayer");
    }

    private IEnumerator RespawnPlayer()
    {
        yield return new WaitForSeconds(5);
        CreateThisPlayer();
    }
}
