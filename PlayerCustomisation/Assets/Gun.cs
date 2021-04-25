using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : AWeapon
{
	public static Gun instance;
	[SerializeField] PlayerMaster playerMaster;
	[SerializeField] Camera cam;
	[SerializeField] float damage;
	[SerializeField] private float rateofFire = 0.25f;
	float Next_fire;
	public GameObject defaultHitEffect;
	public GameObject enemyHitEffect;
	Transform myTranform;
	PhotonView View;
	
	void Awake()
	{
		instance = this;
		View = GetComponent<PhotonView>();
	}

    public override void onUse()
	{
		Shoot();
	}

	void Shoot()
	{
		Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f));
		Vector3 StartPos = ray.origin = cam.transform.position;
		if (Physics.Raycast(myTranform.TransformPoint(StartPos),myTranform.forward, out RaycastHit hit))
		{
			
			View.RPC("RPC_Shoot", RpcTarget.All, hit.point, hit.transform);
		}
	}

	[PunRPC]
	void RPC_Shoot(RaycastHit hitPosition, Transform hitTransform)
	{
		if(hitTransform.GetComponent<PlayerController>() != null)
        {
			PlayerMaster.instance.CallEventShotEnemy(hitPosition, hitTransform);
        }
        else
        {
			PlayerMaster.instance.CallEventShotDefault(hitPosition, hitTransform);
        }
	}

	public void SpawnEnemyHitEffect(RaycastHit hitPosition, Transform hitTransform)
    {
		hitPosition.collider.gameObject.GetComponent<Damage>()?.TakeDamage((damage));
		if(enemyHitEffect != null)
        {
			Quaternion QuatAngle = Quaternion.LookRotation(hitPosition.normal);
			Instantiate(enemyHitEffect, hitPosition.point, QuatAngle);
		}
	}

	public void SpawnDefaultHitEffect(RaycastHit hitPosition, Transform hitTransform)
    {
		if (defaultHitEffect != null)
		{
			Quaternion quatAngle = Quaternion.LookRotation(hitPosition.normal);
			Instantiate(defaultHitEffect, hitPosition.point, quatAngle);
		}
	}
}
