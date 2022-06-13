using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace StatePattern
{
    [CreateAssetMenu(fileName = "Chase", menuName = "AI States/Solider/Chase")]
    public class SoliderChaseState : State
    {
        public Vector3 target;
        public float rotationSpeed;
 
        public override void Run()
        {
            Chase();
        }

        void Chase()
        {
            if(sensor.canSee)
            {
                target = sensor.player.transform.position;
                agent.SetDestination(target);

                RotateToTarget(target, rotationSpeed);
            }
            else if(!sensor.canSee)
            {
                isFinished = true;
            }
        }
    }
}
