using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class Explosion : MonoBehaviour
{
    [SerializeField] float radius;
    [SerializeField] float explosionForce;
    [SerializeField] int damage;
    [SerializeField] ParticleSystem explosionEffect;

    public void Explode()
    {
        Collider[] overlapColliders = Physics.OverlapSphere(transform.position, radius);

        for (int i = 0; i < overlapColliders.Length; i++)
        {
            Rigidbody rb = overlapColliders[i].attachedRigidbody;
            PlayerStats playerStats = overlapColliders[i].GetComponentInParent<PlayerStats>();
            FlameableBarrel flameableBarrel = overlapColliders[i].GetComponent<FlameableBarrel>();

            if(playerStats)
            {
                playerStats.TakeDamage(damage);
            }

            if(flameableBarrel)
            {
                flameableBarrel.TakeDamage(damage);
            }

            if(rb)
            {
                rb.AddExplosionForce(explosionForce, transform.position, radius);
                Instantiate(explosionEffect, transform.position, Quaternion.identity);
                explosionEffect.Play();
            }
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(transform.position, radius);
    }
}