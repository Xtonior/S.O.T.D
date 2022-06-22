using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hands : MonoBehaviour
{
    Animator animator;

    public float distance;
    RaycastHit hit;

    void Update()
    {
        if(Physics.Raycast(transform.position, transform.forward, out hit,distance))
        {
            if(hit.transform.gameObject.GetComponent<Rigidbody>())
            {
                SetPushPose();
            }
        }
        else
        {
            SetDefaultPose();
        }
    }

    public void SetDefaultPose()
    {
        animator = GetComponent<Animator>();

        animator.SetBool("IsDefaultState", true);
    }

    public void SetPushPose()
    {
        animator = GetComponent<Animator>();

        animator.SetBool("IsDefaultState", false);
    }

    void OnEnable()
    {
        SetDefaultPose();
    }

    void OnDisable()
    {

    }
}
