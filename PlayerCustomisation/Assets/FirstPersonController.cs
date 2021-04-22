using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonController : MonoBehaviour
{
    [SerializeField] private Transform FirstPersonCamera = null;

    [SerializeField] private float mouseSensitivity = 3.5f;

    private float cameraPitch = 0.0f;
    [SerializeField] private bool lockCursor = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void OnEnable()
    {
        if (lockCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    void OnDisable()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    // Update is called once per frame
    void Update()
    {
        UpdateMouseLook();
    }

    void UpdateMouseLook()
    {
        Vector2 mouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        cameraPitch -= mouseDelta.y * mouseSensitivity;

        cameraPitch = Mathf.Clamp(cameraPitch, -90f, 90f);

        FirstPersonCamera.localEulerAngles = Vector3.right * cameraPitch;

        transform.Rotate(Vector3.up * mouseDelta.x * mouseSensitivity);
    }
}
