using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon.StructWrapping;
using UnityEngine;

public class TopDownCharacterMovement : MonoBehaviour
{
    private InputHandler _input;
    [SerializeField] private float moveSpeed;
    [SerializeField] private Camera TopDownCamera;
    [SerializeField] private float rotateSpeed;

    void Awake()
    {
        _input = GetComponent<InputHandler>();
    }
    // Start is called before the first frame update
    void Start()
    {
        if (TopDownCamera == null)
        {
            TopDownCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        var targetVector = new Vector3(_input.InputVector.x, 0, _input.InputVector.y);

        // Move in direction of aim
        var movementVector = MoveToTarget(targetVector);
        // Rotate in direction of movement
        RotateToMovement(movementVector);
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
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, rotateSpeed);
        
    }

    private Vector3 MoveToTarget(Vector3 targetVector)
    {
        var speed = moveSpeed * Time.deltaTime;

        targetVector = Quaternion.Euler(0, 
            TopDownCamera.gameObject.transform.eulerAngles.y,
            0) * targetVector; // match top-down camera rotation

        var targetPosition = transform.position + targetVector * speed;

        transform.position = targetPosition;
        return targetVector;
    }
}
