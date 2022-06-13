using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public abstract class Weapon : MonoBehaviour
{
    public Action OnAttack;
    public Action OnEmptyClip;

    public float damage;
    public float maxDistance;
    public float fireRate;
    public float impactForce;
    public float reloadingTime;
    public float shootCost;
    public float maxAmmo;
    public float availableAmmo;
    public float maxClipAmmo;
    public Vector3 bulletSpreadVariance = new Vector3(0.1f, 0.1f, 0.1f);
    public float currentAmmo;
    public Transform shootPoint;
    public Transform muzzle;
    public GameObject recoil;
    public GameObject kickback;
    public PlayerMovement player;
    public RecoilSystem recoilSystem;
    public WeaponKickbackSystem kickbackSystem;
    public abstract void Shoot();
    public abstract void Reload();
    public abstract void CreateBulletHole(GameObject decal, RaycastHit hit);
    public abstract void CreateBulletImpact(GameObject effect, RaycastHit hit);
}
