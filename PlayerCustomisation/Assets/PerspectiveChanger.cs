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
    // Start is called before the first frame update
    void Start()
    {
        
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
                camera.Camera.gameObject.SetActive(true);

            }
            else
            {
                camera.Camera.gameObject.SetActive(false);
            }
        }
    }
}
