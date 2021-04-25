using Photon.Pun;
using UnityEngine;

public class Billboard : MonoBehaviourPun
{
    private Transform mainCameraTransform;
    private PerspectiveChanger perspectiveChanger;
    // Start is called before the first frame update
    void Start()
    {
        perspectiveChanger = GameObject.FindGameObjectWithTag("PerspectiveManager").GetComponent<PerspectiveChanger>();
        mainCameraTransform = perspectiveChanger.GetCameraRef(PerspectiveChanger.CameraSetting.GameTop).transform;
    }

    public void SetCameraTransform(Transform cameraTransform)
    {
        mainCameraTransform = cameraTransform;

    }
    private void LateUpdate()
    {
        transform.LookAt(transform.position + mainCameraTransform.rotation * Vector3.forward,
            mainCameraTransform.rotation * Vector3.up);
    }
}
