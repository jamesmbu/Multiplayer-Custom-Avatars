using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
public class HealthManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private PhotonView View;
    private float Health;
    private const float MaxHealth = 100;
    // Start is called before the first frame update
    void Start()
    {
        View = GetComponent<PhotonView>();
        Health = MaxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
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
        View.RPC("onServerDamage", RpcTarget.All, delta);
    }

    [PunRPC]
    void onServerDamage(float delta)
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
        NetworkManager.instance.Die();
    }
}
