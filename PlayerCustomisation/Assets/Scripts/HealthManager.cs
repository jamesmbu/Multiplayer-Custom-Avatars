using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
public class HealthManager : MonoBehaviourPun
{
   [SerializeField] PlayerMaster playerMaster;
    [SerializeField]PhotonView View;
    private float Health;
    private const float MaxHealth = 100;
    // Start is called before the first frame update
    private void Awake()
    {

    }
    void Start()
    {
        setUpRef();
        Health = MaxHealth;
    }
   private void OnEnable()
    {
            Debug.Log("master in player Health");
            PlayerMaster.instance.EventModifyHealth += onDamaged;       
    }

    private void OnDisable()
    {
       
            PlayerMaster.instance.EventModifyHealth -= onDamaged;
    }

    void setUpRef()
    {
        playerMaster = GetComponentInParent<PlayerMaster>();
        View = GetComponent<PhotonView>();
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
        Debug.Log("OnDamage: "+ delta);
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
    bool isDead() { return (Health <= 0) ? true : false; }
    

    void Die()
    {
       // Debug.Log("Dead!");
        playerMaster.CallEventOnPlayerDeath();
    }
}
