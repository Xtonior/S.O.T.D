using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractions : MonoBehaviour
{
    [SerializeField] private Transform playerCamera;
    [SerializeField] private Transform orientation;
    [SerializeField] private float rayDistance; 

    [Header("PickUp")]
    [SerializeField] private Transform holder;
    [SerializeField] private float throwForce;
    [SerializeField] private float minimalSpeed;
    [SerializeField] private float force;

    private GameObject hObject;
    private Rigidbody hObjectRigidbody;
    private float tmpDrag;
    public PlayerStats playerStats;

    RaycastHit hit;

    private Vector3 targetPos;
    private float delta;
    private float newDelta;




    [Header("Debugging")]
    [SerializeField] GameObject box;
    [SerializeField] GameObject barrel;



    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            Use();
        }

        if(hObject != null && Input.GetMouseButton(0))
        {
            ThrowObject();
        }

        if(hObject != null)
        {
            MoveObject();
        }






        if(Input.GetKeyDown(KeyCode.Keypad1))
        {
            RaycastHit h;

            if(Physics.Raycast(transform.position, holder.forward, out h, 200f))
            {
                var i = Instantiate(box);
                i.transform.position = h.point;
                i.transform.position.Set(i.transform.position.x, i.transform.position.y + 5f, i.transform.position.z);
            }
        }
        if(Input.GetKeyDown(KeyCode.Keypad2))
        {
            RaycastHit h;

            if(Physics.Raycast(transform.position, holder.forward, out h, 200f))
            {
                var i = Instantiate(barrel);
                i.transform.position = h.point;
                i.transform.position.Set(i.transform.position.x, i.transform.position.y + 5f, i.transform.position.z);
            }
        }
    }

    private void Use()
    {
        if(Physics.Raycast(playerCamera.position, playerCamera.forward, out hit, rayDistance))
        {
            if(hit.transform.gameObject.GetComponent<MedKitProp>())
            {
               playerStats.Heal(hit.transform.gameObject.GetComponent<MedKitProp>().restoreAmount(playerStats.health, playerStats.maxHealth)); 
               playerStats.TakeDamage(0);
            }

            if(hObject == null)
            {
                if(Physics.Raycast(playerCamera.position, playerCamera.forward, out hit, rayDistance))
                {
                    PickUpObject(hit.transform.gameObject);
                }
            }
            else
            {
                DropObject();
            }           
        }
    }

    void MoveObject()
    {
        if(Vector3.Distance(hObject.transform.position, holder.position) > 0.1f)
        {
            targetPos = playerCamera.transform.position + playerCamera.transform.forward * 2.5f;
            hObjectRigidbody.velocity = (targetPos - hObject.transform.position) * force;

            if(Vector3.Angle(orientation.forward, hObject.transform.forward) < delta)
            {
                newDelta = delta - Vector3.Angle(orientation.forward, hObject.transform.forward);
                hObject.transform.rotation = Quaternion.Euler(hObject.transform.eulerAngles.x, hObject.transform.eulerAngles.y + newDelta, hObject.transform.eulerAngles.z);
            }

            if(Vector3.Angle(orientation.forward, hObject.transform.forward) > delta)
            {
                newDelta = Vector3.Angle(orientation.forward, hObject.transform.forward) - delta;
                hObject.transform.rotation = Quaternion.Euler(hObject.transform.eulerAngles.x, hObject.transform.eulerAngles.y - newDelta, hObject.transform.eulerAngles.z);
            }
        }

        if(Vector3.Distance(hObject.transform.position, holder.position) > rayDistance)
        {
            DropObject();
        }
    }

    void PickUpObject(GameObject pickObject)
    {
        if(pickObject.GetComponent<Rigidbody>())
        {
            hObjectRigidbody = pickObject.GetComponent<Rigidbody>();
            hObjectRigidbody.useGravity = false;
            tmpDrag = hObjectRigidbody.drag;
            hObjectRigidbody.drag = 10;
            hObjectRigidbody.angularVelocity = Vector3.zero;
            hObjectRigidbody.constraints = RigidbodyConstraints.FreezeRotation;

            //hObjectRigidbody.transform.parent = holder;
            hObject = pickObject;

            delta = Vector3.Angle(orientation.forward, hObject.transform.forward);
        }
    }

    void DropObject()
    {
        hObjectRigidbody.useGravity = true;
        hObjectRigidbody.drag = tmpDrag;
        hObjectRigidbody.constraints = RigidbodyConstraints.None;

        hObjectRigidbody.velocity.Set(minimalSpeed, minimalSpeed, minimalSpeed);

        //hObjectRigidbody.transform.parent = null;
        hObject = null;
    }

    void ThrowObject()
    {
        DropObject();
        hObjectRigidbody.AddForce(holder.forward * throwForce);
    }
}
