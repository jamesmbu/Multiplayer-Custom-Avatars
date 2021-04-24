using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
public class HealthManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private PlayerMaster playerMaster;
    [SerializeField] private PhotonView View;
    private float Health;
    private const float MaxHealth = 100;
    // Start is called before the first frame update
    void Start()
    {
        if (playerMaster != null)
        {
            playerMaster = GetComponentInParent<PlayerMaster>();
        }
        View = GetComponent<PhotonView>();
        Health = MaxHealth;
    }

    private void OnEnable()
    {
        if (playerMaster != null)
        {
            playerMaster.EventModifyHealth += onDamaged;
        }
    }

    private void OnDisable()
    {
        if (playerMaster != null)
        {
            playerMaster.EventModifyHealth -= onDamaged;
        }
    }
    void Testing()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            onDamaged(25);
        }
    }
    public void onDamaged(float delta)
    {
        View.RPC("OnServerDamage", RpcTarget.All, delta);
    }

    [PunRPC]
    void OnServerDamage(float delta)
    {
        if (!View.IsMine)
            return;
        ModifyHealth(-delta);
        Debug.Log("Health: " + Health);
        if (isDead())
        {
            Die();
        }
    }

    float ModifyHealth(float delta)
    {
        float oldHealth = Health;

        Health = Mathf.Clamp(Health + delta, 0.0f, MaxHealth);

        return Health - oldHealth;
    }
    bool isDead()
    {
        return (Health <= 0) ? true : false;
    }

    void Die()
    {
        Debug.Log("Dead!");
        playerMaster.Die();
    }
}
