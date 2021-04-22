using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon.StructWrapping;
using UnityEngine;

public class TopDownCharacterMovement : MonoBehaviour
{
    private InputHandler _input;
    [SerializeField] private float moveSpeed;
    [SerializeField] private Camera TopDownCamera;
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
        MoveToTarget(targetVector);
        // Rotate in direction of movement
    }

    private void MoveToTarget(Vector3 targetVector)
    {
        var speed = moveSpeed * Time.deltaTime;

        targetVector = Quaternion.Euler(0, 
            TopDownCamera.gameObject.transform.eulerAngles.y,
            0) * targetVector; // match top-down camera rotation

        var targetPosition = transform.position + targetVector * speed;

        transform.position = targetPosition;
    }
}
