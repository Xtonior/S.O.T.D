using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crowbar : MonoBehaviour
{
    [SerializeField] private float damage;
    [SerializeField] private float rayDistance;
    [SerializeField] private Transform playerCamera;
    [SerializeField] private RecoilSystem recoilSystem;
    [SerializeField] private float cooldown;
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip attackSound;

    private bool canAttack;
    private Animator animator;
    RaycastHit hit;

    void Start()
    {
        animator = GetComponent<Animator>();
        canAttack = true;
    }

    void Attack()
    {
        animator.SetTrigger("Attack");
        recoilSystem.RecoilFire();
        
        canAttack = false;

        StartCoroutine(ResetCooldown());
        audioSource.PlayOneShot(attackSound);

        if(Physics.Raycast(playerCamera.position, playerCamera.forward, out hit, rayDistance))
        {
            if(hit.transform.gameObject.GetComponent<Breakable>())
            {
                hit.transform.gameObject.GetComponent<Breakable>().GetDamage(damage);
            }
        }
    }

    IEnumerator ResetCooldown()
    {
        yield return new WaitForSeconds(cooldown);
        canAttack = true;
    }

    void Update()
    {
        if(Input.GetKey(KeyCode.H) && canAttack)
        {
            Attack();
        }
    }
}
