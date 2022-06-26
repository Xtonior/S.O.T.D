using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeEntity : MonoBehaviour
{
    [SerializeField] float throwForce;
    private Rigidbody rb;

    public void Throw(Vector3 dir)
    {
        rb = GetComponent<Rigidbody>();
        rb.AddForce(dir * throwForce);
    }
}
