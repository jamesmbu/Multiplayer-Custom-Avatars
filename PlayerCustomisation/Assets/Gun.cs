using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : AWeapon
{
	public static Gun instance;
	[SerializeField] PlayerMaster playerMaster;
	[SerializeField] Camera cam;
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
		ray.origin = cam.transform.position;
	

		if(Physics.Raycast(ray, out RaycastHit hit,400))
		{
			if (hit.transform.GetComponent<Damage>() != null)
			{
				Debug.Log("player hit: " + hit.collider.gameObject.name);
				hit.collider.gameObject.GetComponent<Damage>()?.TakeDamage(((AWeaponinfo)propsInfo).Damage);
				View.RPC(nameof(RPC_EnemyShoot), RpcTarget.All, hit.point, hit.normal);
			}
            else
            {
				Debug.Log("Default hit: "+hit.collider.gameObject.name);
				View.RPC(nameof(RPC_DefaultShoot), RpcTarget.All, hit.point, hit.normal);
            }
		}
	}

	[PunRPC]
	void RPC_EnemyShoot(Vector3 hitPosition, Vector3 hitTransform)
	{
		Collider[] colliders = Physics.OverlapSphere(hitPosition, 0.3f);
		if (colliders.Length != 0)
		{
			
			GameObject bulletImpactObj = Instantiate(EnemyHitEffectPrefab, hitPosition + hitTransform * 0.001f, Quaternion.LookRotation(hitTransform, Vector3.up) * EnemyHitEffectPrefab.transform.rotation);
			Destroy(bulletImpactObj, 10f);
			bulletImpactObj.transform.SetParent(colliders[0].transform);
		}

	}

	[PunRPC]void RPC_DefaultShoot(Vector3 hitPosition, Vector3 hitTransform)
	{
		Collider[] colliders = Physics.OverlapSphere(hitPosition, 0.3f);
		if (colliders.Length != 0)
		{

			GameObject bulletImpactObj = Instantiate(EnemyHitEffectPrefab, hitPosition + hitTransform * 0.001f, Quaternion.LookRotation(hitTransform, Vector3.up) * EnemyHitEffectPrefab.transform.rotation);
			Destroy(bulletImpactObj, 10f);
			bulletImpactObj.transform.SetParent(colliders[0].transform);
		}

	}

}
