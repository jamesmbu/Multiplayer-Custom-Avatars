using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopDownCharacterMovement : MonoBehaviour
{
    private InputHandler _input;
    [SerializeField] private float moveSpeed;
    void Awake()
    {
        _input = GetComponent<InputHandler>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
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

        var targetPosition = transform.position + targetVector * speed;

        transform.position = targetPosition;
    }
}
