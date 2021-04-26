using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class GameplayHandle : MonoBehaviourPun
{
    [SerializeField] private int score = 0;

    [SerializeField] private Text scoreTextRef;

    //private NetworkManager netManager;

    // Start is called before the first frame update
    void Start()
    {
       // if (!netManager) netManager = FindObjectOfType<NetworkManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        // If the collision entry was a collectable item
        if (other.CompareTag("Collectable"))
        {
            if (photonView.IsMine)
            {
                IncrementScore();
            }
            
            Destroy(other.gameObject);
        }
    }

    void IncrementScore()
    {
        score++;
        // netManager.OnPlayerScore(score);
        GameManager.instance.OnPlayerScore(score);
    }
}
