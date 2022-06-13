using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodSplash : MonoBehaviour
{
    [SerializeField] GameObject splashDecal;
    [SerializeField] float lifeTime;
    
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void CreateSplash(Vector3 dir, GameObject t)
    {
        RaycastHit hit;

        if(Physics.Raycast(t.transform.position, dir, out hit, 1000f))
        {
            GameObject g = Instantiate(splashDecal, hit.point, Quaternion.LookRotation(hit.normal));
        }
    }
}
