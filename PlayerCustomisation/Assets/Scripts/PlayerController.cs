using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
public class PlayerController : MonoBehaviourPun,Damage
{
    PlayerMaster Master;
    PhotonView View;
    [SerializeField ]Props gun;
    private void Awake()
    {
        Master = GetComponentInParent<PlayerMaster>();
        View = GetComponent<PhotonView>();
    }
    // Start is called before the first frame update
    void Start()
    {
        Master = GetComponentInParent<PlayerMaster>();
    }
    public void OnEnable()
    {
        Debug.Log("master in player controller");
        PlayerMaster.instance.EventOnPlayerDeath += onDeath;      
    }
    public  void OnDisable()
    {
        PlayerMaster.instance.EventOnPlayerDeath -= onDeath;
    }
    // Update is called once per frame
    void Update()
    {
       
        if (View.IsMine)
        {

            if (Input.GetMouseButtonDown(0))
            {
                gun.onUse();
            }
        }
    }

    IEnumerator countdown()
    {
        yield return new WaitForSeconds(5);
        Debug.Log("Start countdown");
      
    }

    void onDeath()
    {
        PhotonNetwork.Destroy(this.gameObject);
    }

    public void TakeDamage(float damage)
    {
        Master.CallEventModifyHealth(damage);
    }
}
