using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class InputHandler : MonoBehaviourPun
{
    [SerializeField]private PerspectiveChanger perspectiveChanger;
    public Vector2 InputVector { get; private set; }
    public Vector3 MousePosition { get; private set; }

    private bool InFirstPerson = false;

    void Start()
    {
        perspectiveChanger = GameObject.FindGameObjectWithTag("PerspectiveManager").GetComponent<PerspectiveChanger>();
    }
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
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (!InFirstPerson) // Toggle into first-person
            {
                InFirstPerson = true;
                perspectiveChanger.SetCameraValue(PerspectiveChanger.CameraSetting.GameFirstPerson,
                    transform.Find("FirstPersonCamera").GetComponent<Camera>());
                perspectiveChanger.SetCameraPerspective(PerspectiveChanger.CameraSetting.GameFirstPerson);
                // Change movement handler
                GetComponent<TopDownCharacterMovement>().enabled = false;
                //GetComponent<CharacterController>().enabled = true;
                GetComponent<FirstPersonController>().enabled = true;

            }
            else // Toggle out of first-person
            {
                InFirstPerson = false;
                perspectiveChanger.SetCameraPerspective(PerspectiveChanger.CameraSetting.GameTop);
                GetComponent<TopDownCharacterMovement>().enabled = true;
                //GetComponent<CharacterController>().enabled = false;
                GetComponent<FirstPersonController>().enabled = false;
            }
            
        }
    }
}
