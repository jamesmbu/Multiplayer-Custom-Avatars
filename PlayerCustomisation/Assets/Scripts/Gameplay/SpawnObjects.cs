using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class SpawnObjects : MonoBehaviourPun
{
    [SerializeField] private GameObject ObjectToSpawn;
    public int spawnedCounter;
    private float xPos;

    private float zPos;

    private float X_Radius = 0;
    private float Y_Radius = 0;
    private float Z_Radius = 0;
    [Header("Spawn Interval Settings")]
    [Tooltip("Shortest interval time for spawning")]
    [SerializeField] private float IntervalFloor = 2f;
    [Tooltip("Longest interval time for spawning")]
    [SerializeField] private float IntervalCeiling = 2f;


    private BoxCollider SpawnBounds;
    
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
        StopAllCoroutines();
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator SpawnEvent()
    {
        if (photonView.IsMine)
        {
            while (true)
            {
                xPos = transform.position.x + Random.Range(X_Radius*-1, X_Radius);
                zPos = transform.position.z + Random.Range(Z_Radius*-1, Z_Radius);
                
                photonView.RPC("InstantiateObjectCall", RpcTarget.AllBuffered, 
                    xPos, zPos
                    );

                yield return new WaitForSeconds(Random.Range(IntervalFloor,IntervalCeiling));
                spawnedCounter += 1;
            }
        }
    }

    [PunRPC] void InstantiateObjectCall(float x, float z)
    {
        GameObject newObject = Instantiate(ObjectToSpawn, new Vector3(x, 1, z),
            Quaternion.identity,
            transform);
    }
}
