using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public float timeValueSeconds = 60;

    public float timeValueSeconds_Tracked = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void OnEnable()
    {
        timeValueSeconds_Tracked = timeValueSeconds;
    }

    void OnDisable()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (timeValueSeconds_Tracked > 0)
        {
            timeValueSeconds_Tracked -= Time.deltaTime;
        }
        else
        {
            timeValueSeconds_Tracked = 0;
        }
        
    }
}
