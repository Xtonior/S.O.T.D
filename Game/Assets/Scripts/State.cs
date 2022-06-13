using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace StatePattern
{
    public abstract class State : ScriptableObject
    {
        public bool isFinished { get; protected set; }
        [HideInInspector] public BasicSoliderAI enemy;
        [HideInInspector] public NavMeshAgent agent;
        [HideInInspector] public AiSensor sensor;

        public virtual void Init() {}
        public abstract void Run();

        public void RotateToTarget(Vector3 target, float rotationSpeed)
        {   
            Vector3 dir = target - enemy.transform.position;
            dir.y = 0;

            if(dir == Vector3.zero) return;

            enemy.transform.rotation = Quaternion.RotateTowards
            (
                enemy.transform.rotation,
                Quaternion.LookRotation(dir, Vector3.up),
                rotationSpeed * Time.deltaTime
            );
        }
    }
}
