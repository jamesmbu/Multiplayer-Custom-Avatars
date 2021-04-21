using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PerspectiveChanger : MonoBehaviour
{
    public enum CameraSetting
    {
        Menu,
        CharacterPortrait,
        GameTop,
        GameFirstPerson,
    }
    [System.Serializable]
    public struct CameraRef
    {
        public CameraSetting Type;
        public Camera Camera;
    }

    // Make an array of the struct for the editor
    public CameraRef[] CameraRefs;

    [SerializeField]
    private Canvas NetworkCanvas;

    // Start is called before the first frame update
    void Start()
    {
        SetCameraPerspective(CameraSetting.Menu);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetCameraPerspective(CameraSetting cameraSetting)
    {
        foreach (CameraRef camera in CameraRefs) // go through each camera type programmed into the editor
        {
            if (camera.Type == cameraSetting) // if the type matches with the desired perspective to be set...
            {
                camera.Camera.gameObject.SetActive(true); // turn on the appropriate camera
                NetworkCanvas.GetComponent<Canvas>().worldCamera = camera.Camera; // get the network GUI, and associate the new camera setting with it
                NetworkCanvas.GetComponent<Canvas>().planeDistance = 1; // set the plane distance- to keep the UI in front of everything
            }
            else
            {
                if (cameraSetting == CameraSetting.Menu && camera.Type == CameraSetting.CharacterPortrait) continue; // Menu and Preview of customised player activate together
                camera.Camera.gameObject.SetActive(false);
            }
        }

        
    }
}
