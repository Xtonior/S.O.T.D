using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodenCrate : Breakable
{
    void afterBreak()
    {
        base.Break();

        foreach(Rigidbody rigidBody in fractured.GetComponentsInChildren<Rigidbody>())
        {
            Vector3 force = (rigidBody.transform.position - transform.position).normalized * breakForce;
            rigidBody.AddForce(force);
        }

        Destroy(gameObject);
    }

    void Update()
    {
        if(Health <= 0)
        {
            afterBreak();
        }
    }
}
