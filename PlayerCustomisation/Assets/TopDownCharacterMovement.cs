using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon.StructWrapping;
using Photon.Pun;
using UnityEngine;

public class TopDownCharacterMovement : MonoBehaviourPun
{
    private CharacterController controller = null;

    private InputHandler _input;
    [SerializeField] private float moveSpeed;
    [SerializeField] private Camera TopDownCamera;
    [SerializeField] private float rotateSpeed;

    [SerializeField] private float gravity = -13f;
    private float velocityY = 0.0f;
    private Vector2 currentDir = Vector2.zero;
    private Vector2 currentDirVelocity = Vector2.zero;
    [SerializeField] [Range(0f, 0.5f)] private float moveSmoothTime = 0.2f;

    void Awake()
    {
        _input = GetComponent<InputHandler>();
    }
    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        if (TopDownCamera == null)
        {
            TopDownCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        }
    }

    void OnEnable()
    {
        if (!photonView.IsMine) return;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    // Update is called once per frame
    void Update()
    {
        if (!photonView.IsMine)
        {
            return;
        }
        var targetVector = new Vector3(_input.InputVector.x, 0, _input.InputVector.y);

        // Move in direction of aim
        var movementVector = MoveToTarget(targetVector);
        // Rotate in direction of movement
        //RotateToMovement(movementVector);
    }

    private void RotateToMovement(Vector3 movementVector)
    {
        if (movementVector.magnitude == 0) { return; }

        /*gameObj.transform.eulerAngles = new Vector3(
            gameObj.transform.eulerAngles.x,
            gameObj.transform.eulerAngles.y + 180,
            gameObj.transform.eulerAngles.z
        );*/

        Quaternion rotation = Quaternion.LookRotation(movementVector);
        //Debug.Log(transform.rotation);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, rotateSpeed);
        
    }

    private Vector3 MoveToTarget(Vector3 targetVector)
    {
        var speed = moveSpeed * Time.deltaTime;

        Vector2 targetDir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        targetDir.Normalize();

        currentDir = Vector2.SmoothDamp(currentDir, targetDir, ref currentDirVelocity, moveSmoothTime);
        
        if (controller.isGrounded)
        {
            velocityY = 0.0f;
        }
        velocityY += gravity * Time.deltaTime;
        Vector3 velocity = (transform.forward * currentDir.y + transform.right * currentDir.x) * 8f + Vector3.up * velocityY;
        controller.Move(velocity * Time.deltaTime);
        //controller.Move(new Vector3(0, velocityY, 0));
        
        /*velocityY -= gravity * Time.deltaTime;
        controller.Move(new Vector3(0, velocityY, 0));
        if (controller.isGrounded) velocityY = 0;*/

        targetVector = Quaternion.Euler(0, 
            TopDownCamera.gameObject.transform.eulerAngles.y,
            0) * targetVector; // match top-down camera rotation

        var targetPosition = transform.position + targetVector * speed;
        //Debug.Log(targetVector);
        //transform.position = targetPosition;
        transform.Rotate(0, Input.GetAxis("Horizontal") * 450f * Time.deltaTime, 0);
        return targetVector;
    }
}
