using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodenCrate : Breakable
{
    [SerializeField] GameObject loot;

    void Break()
    {
        GameObject fractured = Instantiate(fracturedVersion, transform.position, transform.rotation);
        Instantiate(loot, transform.position, transform.rotation);

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
            Break();
        }
    }
}
