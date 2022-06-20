using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractions : MonoBehaviour
{
    [SerializeField] private Transform playerCamera;
    [SerializeField] private float rayDistance; 

    [Header("PickUp")]
    [SerializeField] private Transform holder;
    [SerializeField] private float throwForce;
    [SerializeField] private float minimalSpeed;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float pickDelay;
    [SerializeField] private float smooth;

    private GameObject hObject;
    private Rigidbody hObjectRigidbody;
    private float tmpDrag;
    private float pickUpTime;
    private float currentDistance;
    public PlayerStats playerStats;

    RaycastHit hit;

    public Action onUse;

    private Vector3 targetPos;



    [Header("Debugging")]
    [SerializeField] GameObject box;
    [SerializeField] GameObject raggdoll;



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
            }
        }
        if(Input.GetKeyDown(KeyCode.Keypad2))
        {
            RaycastHit h;

            if(Physics.Raycast(transform.position, holder.forward, out h, 200f))
            {
                var i = Instantiate(raggdoll);
                i.transform.position = h.point;
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
            hObjectRigidbody.velocity = (targetPos - (hObjectRigidbody.transform.position + hObjectRigidbody.centerOfMass)) * 20f;

            Quaternion quaternion = hObject.transform.rotation;
            quaternion *= holder.rotation;
            hObject.transform.rotation = quaternion;
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
