using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class MyNameplate : MonoBehaviourPun
{
    [SerializeField] private TextMeshProUGUI name_txt;

    [SerializeField] private GameObject tagPanel;
    // Start is called before the first frame update
    void Start()
    {
        if (photonView.IsMine) return;
        SetPanelVis();
        SetName();
        
    }

    private void SetPanelVis()
    {
        tagPanel.SetActive(true);
    }

    private void SetName() => name_txt.text = photonView.Owner.NickName;

}
