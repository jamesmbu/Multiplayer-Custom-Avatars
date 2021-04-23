using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObjects : MonoBehaviour
{
    [SerializeField] private GameObject ObjectToSpawn;

    private float xPos;

    private float zPos;

    private float X_Radius = 0;
    private float Y_Radius = 0;
    private float Z_Radius = 0;

    private BoxCollider SpawnBounds;
    public int spawnedCounter;
    // Start is called before the first frame update
    void Start()
    {
        SpawnBounds = GetComponent<BoxCollider>();
        X_Radius = SpawnBounds.size.x / 2;
        Y_Radius = SpawnBounds.size.y / 2;
        Z_Radius = SpawnBounds.size.z / 2;
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
            xPos = transform.position.x + Random.Range(X_Radius*-1, X_Radius);
            zPos = transform.position.z + Random.Range(Z_Radius*-1, Z_Radius);

            GameObject newObject = Instantiate(ObjectToSpawn, new Vector3(xPos, 1, zPos),
                Quaternion.identity);

            yield return new WaitForSeconds(2f);
            spawnedCounter += 1;
        }
    }
}
