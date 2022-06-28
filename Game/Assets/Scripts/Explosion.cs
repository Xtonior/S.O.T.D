using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class Explosion : MonoBehaviour
{
    [SerializeField] float radius;
    [SerializeField] float explosionForce;
    [SerializeField] ParticleSystem explosionEffect;

    public void Explode()
    {
        Collider[] overlapColliders = Physics.OverlapSphere(transform.position, radius);

        for (int i = 0; i < overlapColliders.Length; i++)
        {
            Rigidbody rb = overlapColliders[i].attachedRigidbody;
            if(rb)
            {
                rb.AddExplosionForce(explosionForce, transform.position, radius);
                Instantiate(explosionEffect, transform.position, Quaternion.identity);
                explosionEffect.Play();
            }
        }
    }
}