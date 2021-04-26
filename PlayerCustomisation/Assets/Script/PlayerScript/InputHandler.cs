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
    private Billboard nametag_billboard;

    private PlayerMaster player;


    private void OnEnable()
    {
        PlayerMaster.instance.EventOnPlayerDeath += Die;
        PlayerMaster.instance.EventSpawnPlayer += Respawn;
    }
    private void OnDisable()
    {
        PlayerMaster.instance.EventOnPlayerDeath -= Die;
        PlayerMaster.instance.EventSpawnPlayer -= Respawn;
    }


    void Start()
    {
       // player = GetComponent<PlayerMaster>();
        perspectiveChanger = GameObject.FindGameObjectWithTag("PerspectiveManager").GetComponent<PerspectiveChanger>();
        nametag_billboard = GetComponentInChildren<Billboard>();

    }
    // Update is called once per frame
    void Update()
    {
        if (!photonView.IsMine)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                if (!InFirstPerson) // Toggle into first-person
                {
                    InFirstPerson = true;

                    nametag_billboard.SetCameraTransform(perspectiveChanger
                        .GetCameraRef(PerspectiveChanger.CameraSetting.GameFirstPerson).transform);

                }
                else // Toggle out of first-person
                {
                    InFirstPerson = false;

                    nametag_billboard.SetCameraTransform(perspectiveChanger
                        .GetCameraRef(PerspectiveChanger.CameraSetting.GameTop).transform);

                }
            }
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

    void Die()
    {
        InFirstPerson = !InFirstPerson;
        perspectiveChanger.SetCameraPerspective(PerspectiveChanger.CameraSetting.GameTop);
        GetComponent<FirstPersonController>().enabled = false;
        Debug.Log("Called from the Input Handler");

    }

    void Respawn()
    {
        InFirstPerson = !InFirstPerson;
        perspectiveChanger.SetCameraValue(PerspectiveChanger.CameraSetting.GameFirstPerson,
    transform.Find("FirstPersonCamera").GetComponent<Camera>());
        perspectiveChanger.SetCameraPerspective(PerspectiveChanger.CameraSetting.GameFirstPerson);
        // Change movement handler
        GetComponent<TopDownCharacterMovement>().enabled = false;
        //GetComponent<CharacterController>().enabled = true;
        GetComponent<FirstPersonController>().enabled = true;
    }
}
