using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
public class OnDeathCountDown : MonoBehaviourPun
{
    PlayerMaster master;
    [SerializeField] private int CountDownTime = 5;
    [SerializeField] private Text text;
    [SerializeField] private GameObject CountdownTimer;
    [SerializeField] private bool TakingAway = false;
    bool Died;

    void Start()
    {
        master = GetComponent<PlayerMaster>();
    }
    private void OnEnable()
    {
       PlayerMaster.instance.EventOnPlayerDeath += onDeath;
        PlayerMaster.instance.EventSpawnPlayer += onRespawn;
    }


    private void OnDisable()
    {
        PlayerMaster.instance.EventOnPlayerDeath -= onDeath;
        PlayerMaster.instance.EventSpawnPlayer -= onRespawn;

    }

    void Update()
    {
        
        if (photonView.IsMine)
        {
            if (Died == false)
                return;

            if (TakingAway == false && CountDownTime > 0)
            {
                DeathTimer();
            }
        }
    }

    void onDeath()
    {
        CountdownTimer.SetActive(true);
        text.text = "Respawn in: " + CountDownTime.ToString();
        Died = true;
    }
    void onRespawn()
    {
        Died = false;
        CountdownTimer.SetActive(false);
        CountDownTime = 5;
    }
    IEnumerator DeathTimer()
    {
        TakingAway = true;
        yield return new WaitForSeconds(1);
        CountDownTime -= 1;
        text.text = "Respawn in: " + CountDownTime.ToString();
        TakingAway = false;
    }

}
