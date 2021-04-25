using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AWeapon : Props
{
    public abstract override void onUse();
    public GameObject defaultHitEffectPrefab;
    public GameObject EnemyHitEffectPrefab;
}
