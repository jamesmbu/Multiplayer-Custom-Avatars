using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class InputHandler : MonoBehaviourPun
{
    public Vector2 InputVector { get; private set; }
    public Vector3 MousePosition { get; private set; }

    // Update is called once per frame
    void Update()
    {
        if (!photonView.IsMine)
        {
            return;
        }
        var h = Input.GetAxis("Horizontal");
        var v = Input.GetAxis("Vertical");
        InputVector = new Vector2(h, v);
        MousePosition = Input.mousePosition;
    }
}
