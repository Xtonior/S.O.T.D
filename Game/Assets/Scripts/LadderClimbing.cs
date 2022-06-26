using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LadderClimbing : MonoBehaviour
{
    [SerializeField] float climbingSpeed;
    [SerializeField] Transform playerCamera;

    private float verticalInput;
    [field: SerializeField] private bool onLadder;
    [field: SerializeField] private bool isUsing;
    private Rigidbody rigidBody;

    private void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
    }

    private void OnTriggerEnter()
    {
        if(!onLadder) Climb();
        
        if(isUsing)
        {
            rigidBody.useGravity = false;
            rigidBody.velocity.Set(0, 0, 0);
        }
    }

    private void OnTriggerExit()
    {
        onLadder = false;
        isUsing = false;
        rigidBody.useGravity = true;
        rigidBody.AddForce(playerCamera.forward * 50f, ForceMode.Impulse);
    }

    private void OnTriggerStay()
    {
        if(isUsing)
        {
            verticalInput = Input.GetAxisRaw("Vertical");
            transform.position += new Vector3(0, verticalInput, 0) / climbingSpeed;

            if(Input.GetKey(KeyCode.E))
            {
                isUsing = false;
            }
        }

        if(!isUsing) Push();
    }

    void Climb()
    {
        onLadder = true;
        isUsing = true;
    }

    void Push()
    {
        if(onLadder)
        {
            rigidBody.useGravity = true;
            rigidBody.velocity.Set(0, 0, 0);
        }
    }
}
