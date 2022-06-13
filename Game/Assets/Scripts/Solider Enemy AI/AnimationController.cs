using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AnimationController : MonoBehaviour
{
    public float maxTime;
    public float maxDistance;

    NavMeshAgent agent;
    Animator animator;
    public Transform target;

    float timer;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        //animator = GetComponent<Animator>();
    }

    void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            float sqDistance = (target.position - agent.destination).sqrMagnitude;
            
            if (sqDistance > maxDistance * maxDistance)
            {
                agent.SetDestination(target.position);
            }

            timer = maxTime;
        }

        //animator.SetFloat("Speed", agent.velocity.magnitude);
    }
}
