using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollController : MonoBehaviour
{
    [SerializeField] Collider[] ragdollColliders;
    [SerializeField] Rigidbody[] ragdollRigidbodies;
    [SerializeField] Collider mainCollider;

    void Start()
    {
        DiasableRagdoll();
    }

    public void EnableRagdoll()
    {
        foreach(Collider collider in ragdollColliders)
        {
            collider.enabled = true;
        }

        foreach(Rigidbody rb in ragdollRigidbodies)
        {
            rb.isKinematic = false;
        }

        mainCollider.enabled = false;
    }

    public void DiasableRagdoll()
    {
        foreach(Collider collider in ragdollColliders)
        {
            collider.enabled = false;
        }

        foreach(Rigidbody rb in ragdollRigidbodies)
        {
            rb.isKinematic = true;
        }

        mainCollider.enabled = true;
    }

    public void GetHit(Vector3 direction, float force, Rigidbody rb)
    {
        rb.AddForce(direction * force);
    }
}
