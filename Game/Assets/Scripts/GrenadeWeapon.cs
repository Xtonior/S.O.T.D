using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeWeapon : MonoBehaviour
{
    [SerializeField] float damage;
    [SerializeField] float damageRadius;
    [SerializeField] float impactForce;
    [SerializeField] GameObject entityGrenade;
    [SerializeField] float coolDownTime;
    [SerializeField] int grenadesInReserve;
    [SerializeField] Transform throwPoint;

    bool canThrow;

    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Throw()
    {
        animator.SetTrigger("Throw");
        grenadesInReserve--;
        GameObject tmp_grenade = Instantiate(entityGrenade, throwPoint);
        tmp_grenade.GetComponent<GrenadeEntity>().Throw(throwPoint.forward);
    }

    void Update()
    {
        if(Input.GetButton("Fire 1"))
        {
            StartCoroutine(IEThrow());
        }
    }

    IEnumerator IEThrow()
    {
        yield return new WaitForSecondsRealtime(coolDownTime);
        Throw();
    }
}
