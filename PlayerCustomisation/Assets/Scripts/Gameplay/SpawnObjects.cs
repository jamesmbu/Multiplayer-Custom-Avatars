using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObjects : MonoBehaviour
{
    [SerializeField] private GameObject ObjectToSpawn;

    public int xPos;

    public int zPos;

    public int spawnedCounter;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void OnEnable()
    {
        StartCoroutine(SpawnEvent());
    }

    void OnDisable()
    {
        StopCoroutine(SpawnEvent());
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator SpawnEvent()
    {
        while (true)
        {
            xPos = Random.Range(-15, 15);
            zPos = Random.Range(15, 35);
            Instantiate(ObjectToSpawn, new Vector3(xPos, 1, zPos), Quaternion.identity);
            yield return new WaitForSeconds(2f);
            spawnedCounter += 1;
        }
    }
}
