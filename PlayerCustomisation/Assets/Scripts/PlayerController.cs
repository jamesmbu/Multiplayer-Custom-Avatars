using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
public class PlayerController : MonoBehaviourPun
{
    PlayerMaster Master;
    PhotonView View;
    private void Awake()
    {
        Master = GetComponentInParent<PlayerMaster>();
        View = GetComponent<PhotonView>();
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        if (View.IsMine)
        {
            StartCoroutine("countdown");
        }
        else 
            Debug.Log("View not mine");
    }

    IEnumerator countdown()
    {
        yield return new WaitForSeconds(2);
        Master.CallEventModifyHealth(10);
    }



}
