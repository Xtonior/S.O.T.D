using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crowbar : MonoBehaviour
{
    [SerializeField] private float damage;
    [SerializeField] private float rayDistance;
    [SerializeField] private Transform playerCamera;
    [SerializeField] private RecoilSystem recoilSystem;
    [SerializeField] private float delay;
    [SerializeField] private float t;

    private float timer;
    private Animator animator;
    RaycastHit hit;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Attack()
    {
        animator.SetTrigger("Attack");
        recoilSystem.RecoilFire();
        timer = delay;

        if(Physics.Raycast(playerCamera.position, playerCamera.forward, out hit, rayDistance))
        {

        }
    }

    void Update()
    {
        if(Input.GetKey(KeyCode.H))
        {
            timer -= t * Time.deltaTime;

            if(timer <= 0)
            {
                Attack();
            }
        }
    }
}
