using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.XR;

public class FirstPersonController : MonoBehaviourPun
{
    [SerializeField] private Transform FirstPersonCamera = null;

    [SerializeField] private float mouseSensitivity = 3.5f;
    [SerializeField] private float walkSpeed = 8f;
    [SerializeField] private float gravity = -13f;
    [SerializeField] [Range(0f, 0.5f)] private float moveSmoothTime = 0.2f;
    [SerializeField] [Range(0f, 0.5f)] private float mouseSmoothTime = 0.1f;

    private float cameraPitch = 0.0f;
    private float velocityY = 0.0f;
    private CharacterController controller = null;

    [SerializeField] private bool lockCursor = true;

    private Vector2 currentDir = Vector2.zero;
    private Vector2 currentDirVelocity = Vector2.zero;

    private Vector2 currentMouseDelta = Vector2.zero;
    private Vector2 currentMouseDeltaVelocity = Vector2.zero;

    private Rigidbody rigidbody = null;
    private CapsuleCollider capsuleCollider = null;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent <CharacterController>();
        rigidbody = GetComponent<Rigidbody>();
        capsuleCollider = GetComponent<CapsuleCollider>();
    }

    void OnEnable()
    {
        if (!photonView.IsMine) return;
        if (lockCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        if (!capsuleCollider)
        {
            capsuleCollider = GetComponent<CapsuleCollider>();
        }

        if (!rigidbody)
        {
            rigidbody = GetComponent<Rigidbody>();
        }
        //capsuleCollider.enabled = false;
        //rigidbody.isKinematic = true;
       // rigidbody.detectCollisions = false;
    }

    void OnDisable()
    {
        if (!photonView.IsMine) return;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        //capsuleCollider.enabled = true;
        //rigidbody.isKinematic = false;
        //rigidbody.detectCollisions = true;
    }
    // Update is called once per frame
    void Update()
    {
        if (!photonView.IsMine)
        {
            return;
        }
        UpdateMouseLook();
        UpdateMovement();
    }

    void UpdateMovement()
    {
        Vector2 targetDir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        targetDir.Normalize();

        currentDir = Vector2.SmoothDamp(currentDir, targetDir, ref currentDirVelocity, moveSmoothTime);

        if (controller.isGrounded)
        {
            velocityY = 0.0f;
        }

        velocityY += gravity * Time.deltaTime;

        Vector3 velocity = (transform.forward * currentDir.y + transform.right * currentDir.x) * walkSpeed + Vector3.up * velocityY;

        controller.Move(velocity * Time.deltaTime);
    }

    void UpdateMouseLook()
    {
        Vector2 targetMouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        
        currentMouseDelta = Vector2.SmoothDamp(currentMouseDelta, targetMouseDelta, ref currentMouseDeltaVelocity,
            mouseSmoothTime);

        cameraPitch -= currentMouseDelta.y * mouseSensitivity;

        cameraPitch = Mathf.Clamp(cameraPitch, -90f, 90f);

        FirstPersonCamera.localEulerAngles = Vector3.right * cameraPitch;

        transform.Rotate(Vector3.up * currentMouseDelta.x * mouseSensitivity);

    }
}
