using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
public class PlayerController : MonoBehaviourPun
{
    public float turnSpeed = 180;
    public float tiltSpeed = 180;
    public float walkSpeed = 1;


   float maxHealth , Health;

    [SerializeField]
    private Transform fpcam;    // first person camera

    private Rigidbody body;
    [SerializeField]
    private PhotonView View;

    private void Awake()
    {
        View = GetComponent<PhotonView>();
    }
    // Start is called before the first frame update
    void Start()
    {
        maxHealth = 100;
        Health = maxHealth;
      
        // Cursor.lockState = CursorLockMode.Confined;
    }

    // Update is called once per frame
    void Update()
    {
        onDamaged(25);
        if (View.IsMine)
        {
            
            float forward = Input.GetAxis("Vertical");
            float turn = Input.GetAxis("Horizontal") + Input.GetAxis("Mouse X");
            float tilt = Input.GetAxis("Mouse Y");
            transform.Translate(new Vector3(0, 0, forward * walkSpeed * Time.deltaTime));
            transform.Rotate(new Vector3(0, turn * turnSpeed * Time.deltaTime, 0));
            if (fpcam != null)
                fpcam.Rotate(new Vector3(-tilt * tiltSpeed * Time.deltaTime, 0));

            onDamaged(25);

        }
    }

    public void onDamaged(float delta)
    {
        View.RPC("onServerDamage",RpcTarget.All,delta);
    }

    [PunRPC]
    void onServerDamage(float delta)
    {
        if (!View.IsMine)
            return;
        ModifyHealth(-delta);
        Debug.Log("Health: " + ModifyHealth(-delta));
        if(isDead())
        {
            Die();
        }
    }

    float ModifyHealth(float delta)
    {
       float oldHealth = Health;

        Health = Mathf.Clamp(Health + delta, 0.0f, maxHealth);

        return Health - oldHealth;
    }
    bool isDead()
    {
        return (Health <= 0) ? true : false;
    }

    void Die()
    {
        Debug.Log("Dead!");
        NetworkManager.instance.Die();
    }
}
