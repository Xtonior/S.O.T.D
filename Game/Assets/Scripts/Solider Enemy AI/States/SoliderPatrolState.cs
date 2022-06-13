using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace StatePattern
{
    [CreateAssetMenu(fileName = "Patrol", menuName = "AI States/Solider/Patrol")]
    public class SoliderPatrolState : State
    {
        private Vector3 walkPoint;
        public float walkRange;
        bool walkPointSet;
        public float restTime;
        public float targetMemoryTime;
        public float rotSpeed;
        float mTimer;
        Vector3 lastTargetPoint;
        public LayerMask groundLayer;

        public override void Run()
        {
            Patrol();
        }

        void Patrol()
    {
        float timer = restTime;

        if(mTimer > 0f)
        {
            mTimer -= Time.deltaTime;
            walkPoint = lastTargetPoint;
            walkPointSet = true;

            RotateToTarget(walkPoint, rotSpeed);
        }

        if(!walkPointSet && mTimer <= 0f)
        {
            isFinished = true;
        }

        if(walkPointSet)
        {
            agent.SetDestination(walkPoint);
            RotateToTarget(walkPoint, rotSpeed);
        }

        Vector3 distanceToWalkPoint = enemy.transform.position - walkPoint;

        if(distanceToWalkPoint.magnitude < 1f)
        {
            SearchWalkPoint();      
        }
    }

    void GenerateWalkPoint()
    {
        float randomZ = Random.Range(-walkRange, walkRange);
        float randomX = Random.Range(-walkRange, walkRange);

        walkPoint = new Vector3(enemy.transform.position.x + randomX, enemy.transform.position.y, enemy.transform.position.z + randomZ);
    }

    void SearchWalkPoint()
    {
        GenerateWalkPoint();

        if(Physics.Raycast(walkPoint, - enemy.transform.up, 2f, groundLayer))
        {
            walkPointSet = true;
        }
    }
    }
}