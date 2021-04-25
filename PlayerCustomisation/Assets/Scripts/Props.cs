using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Props : MonoBehaviour
{
    public PropsInfo propsInfo;
    public GameObject propsGameObject;
    public abstract void onUse();
}
