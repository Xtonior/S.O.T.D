using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponKickbackSystem : MonoBehaviour
{
    [SerializeField] private Weapon weapon;

    [SerializeField] private float recoilX;
    [SerializeField] private float recoilY;
    [SerializeField] private float recoilZ;

    [SerializeField] private float snappiness;
    [SerializeField] private float returnSpeed;

    private Vector3 currentPosition;
    private Vector3 targetPosition;
    
    private void Start()
    {
        currentPosition = transform.localPosition;
    }

    private void OnEnable()
    {
        weapon.OnAttack += RecoilFire;
    }

    private void OnDisable()
    {
        weapon.OnAttack -= RecoilFire;
    }

    private void Update()
    {
        targetPosition = Vector3.Lerp(targetPosition, Vector3.zero, returnSpeed * Time.deltaTime);
        currentPosition = Vector3.Lerp(currentPosition, targetPosition, snappiness * Time.deltaTime);
        transform.localPosition = currentPosition;
    }

    public void RecoilFire()
    {
        targetPosition += new Vector3(UnityEngine.Random.Range(-recoilX, recoilX), recoilY, recoilZ);
    }
}
