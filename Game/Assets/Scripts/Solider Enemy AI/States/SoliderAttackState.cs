using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StatePattern
{
    [CreateAssetMenu(fileName = "ShootAttack", menuName = "AI States/Solider/Shoot Attack")]
    public class SoliderAttackState : State
    {
        public SoliderShooting soliderShooting;
        public SoliderWeaponIK weaponIK;
        public float rotationSpeed;
        Vector3 target;
        Transform targetTransform;

        public override void Init()
        {
            soliderShooting = enemy.GetComponent<SoliderShooting>();
            weaponIK = enemy.GetComponent<SoliderWeaponIK>();
        }

        public override void Run()
        {
            if(sensor.canSee)
            {
                target = sensor.player.transform.position;
                targetTransform = sensor.player.transform;
                weaponIK.SetTarget(targetTransform);

                if(Vector3.Distance(enemy.transform.position, target) <= 1)
                {
                    agent.SetDestination(enemy.transform.position);
                }
                else
                {
                    agent.SetDestination(target);
                }

                if(weaponIK.aimed)
                {
                    soliderShooting.Shoot();
                }
            }
            else
            {
                isFinished = true;
            }
        }
    }
}
