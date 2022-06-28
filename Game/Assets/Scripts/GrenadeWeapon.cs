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
    [SerializeField] float forceMultiplier;
    [SerializeField] float maxForce;

    bool canThrow;
    bool throwStarted;

    private Animator animator;
    [field:SerializeField] private float currentForce;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void ThrowStart()
    {
        ThrowEnd();
    }

    void ThrowEnd()
    {
        if(throwStarted)
        {
            animator.SetTrigger("ThrowEnd");
            throwStarted = false;
            grenadesInReserve--;
            GameObject tmp_grenade = Instantiate(entityGrenade, throwPoint.position, throwPoint.rotation);
            tmp_grenade.GetComponent<GrenadeEntity>().Throw(throwPoint.forward, Mathf.Min(currentForce, maxForce));
            currentForce = 0f;
        }        
    }

    void MultiplyForce()
    {
        if(throwStarted) currentForce += forceMultiplier * Time.deltaTime;
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.H))
        {
            if(!throwStarted)
            {
                animator.SetTrigger("ThrowStart");
                throwStarted = true;
            }
        }

        if(Input.GetKeyUp(KeyCode.H))
        {
            StartCoroutine(IEThrow());
        }

        MultiplyForce();
    }

    IEnumerator IEThrow()
    {
        ThrowStart();
        yield return new WaitForSecondsRealtime(coolDownTime);
    }
}
