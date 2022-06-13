using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace StatePattern
{
    public class BasicSoliderAI : MonoBehaviour
{
    private NavMeshAgent agent;
    public AiSensor aiSensor;
    public LayerMask playerLayer;
    public float health;
    public RagdollController ragdollController;
    bool alive;

    [Header("States")]
    public State currentState;
    [SerializeField] State startState;
    [SerializeField] State patrolState;
    [SerializeField] State chaseState;
    [SerializeField] State attackState;

    //States
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;

    void Start()
    {
        alive = true;
        SetState(startState);
    }

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if(alive)
        {
            playerInSightRange = aiSensor.canSee;
            playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, playerLayer);

            if(playerInSightRange && !playerInAttackRange) SetState(chaseState);

            if(!playerInSightRange && !playerInAttackRange) SetState(patrolState);  

            if(playerInSightRange && playerInAttackRange) SetState(attackState);

            if(!currentState.isFinished)
            {
                currentState.Run();
            }
        }
        else
        {
            return;
        }
    }

    void SetState(State state)
    {
        currentState = Instantiate(state);
        currentState.enemy = this;
        currentState.agent = agent;
        currentState.sensor = aiSensor;
        currentState.Init();
    }

    public void TakeDamage(float damage)
    {
        health -= damage;

        if(health <= 0)
        {
            Invoke(nameof(Die), 0.2f);
        }
    }

    private void Die()
    {
        ragdollController.EnableRagdoll();
        alive = false;
        aiSensor.enabled = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
}
