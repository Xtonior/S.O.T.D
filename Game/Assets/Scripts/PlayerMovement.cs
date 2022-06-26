using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerMovement : MonoBehaviour
{
    float playerHeight = 2f;

    [SerializeField] Transform orientation;

    [Header("Movement")]
    [SerializeField] float moveSpeed = 6f;
    [SerializeField] float airMultiplier = 0.4f;
    float movementMultiplier = 10f;

    [Header("Crouching")]
    [SerializeField] float crouchHeight;
    [SerializeField] float toCrouchSpeed;
    [SerializeField] CapsuleCollider playerCollider;
    [SerializeField] Transform playerCamera;
    [SerializeField] float crouchSpeed;
    [SerializeField] Vector3 offset;

    [SerializeField] float startHeight;

    [Header("Sprinting")]
    [SerializeField] float walkSpeed = 4f;
    [field: SerializeField] public float sprintSpeed { get; private set; } = 6f;
    [field: SerializeField] public float acceleration { get; private set; } = 10f;
    [field: SerializeField] public float maxAcceleration { get; private set; } = 10f;

    [Header("Jumping")]
    public float jumpForce = 5f;

    [Header("Drag")]
    [SerializeField] float groundDrag = 6f;
    [SerializeField] float airDrag = 2f;

    float horizontalMovement;
    float verticalMovement;

    [Header("Ground Detection")]
    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask groundMask;
    [SerializeField] float groundDistance = 0.2f;
    [SerializeField] float jumpDistance = 0.3f;
    [SerializeField] float maxSlopeAngle;
    public bool isGrounded { get; private set; }
    public bool canJump { get; private set; }

    Vector3 moveDirection;
    Vector3 slopeMoveDirection;

    [SerializeField] Rigidbody rigidBody;
    private RaycastHit slopeHit;

    public bool isRunning{ get; private set; }
    public bool isWalking{ get; private set; }

    bool isCrouching;
    float currentHeight;

    public bool OnSlope()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight / 2 + 0.5f))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle < maxSlopeAngle && angle != 0;
        }
        return false;
    }

    public Vector3 GetSlopeDirection(Vector3 dir)
    {
        return Vector3.ProjectOnPlane(dir, slopeHit.normal).normalized;
    }

    private void Start()
    {
        startHeight = playerCollider.height;
    }

    private void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        canJump = Physics.CheckSphere(groundCheck.position, jumpDistance, groundMask);

        SetupInput();
        ControlDrag();
        ControlSpeed();

        if(Input.GetButton("Jump") && canJump)
        {
            Jump();
        }

        if(Input.GetKeyDown(KeyCode.LeftControl))
        {
            CrouchStart();
        }
        if(Input.GetKeyUp(KeyCode.LeftControl))
        {
            CrouchEnd();
        }
        

        slopeMoveDirection = Vector3.ProjectOnPlane(moveDirection, slopeHit.normal).normalized;
    }

    void SetupInput()
    {
        horizontalMovement = Input.GetAxisRaw("Horizontal");
        verticalMovement = Input.GetAxisRaw("Vertical");

        moveDirection = orientation.forward * verticalMovement + orientation.right * horizontalMovement;
    }

    void CrouchStart()
    {
        playerCollider.height = crouchHeight;
        isCrouching = true;
    }

    void CrouchEnd()
    {
        playerCollider.height = startHeight;
        
        if(isGrounded)
        {
            transform.position += offset;
        }

        isCrouching = false;
    }

    void Jump()
    {
        if(canJump)
        {
            rigidBody.velocity = new Vector3(rigidBody.velocity.x, 0, rigidBody.velocity.z);
            rigidBody.AddForce(transform.up * jumpForce, ForceMode.Impulse);
        }
    }


    void ControlSpeed()
    {
        if(Input.GetButton("Sprint") && isGrounded)
        {
            moveSpeed = Mathf.Lerp(moveSpeed, sprintSpeed, acceleration * Time.deltaTime);
            isRunning = true;
        }
        else if(isCrouching)
        {
            moveSpeed = Mathf.Lerp(moveSpeed, crouchSpeed, acceleration * Time.deltaTime);
            isRunning = false;
        }
        else
        {
            moveSpeed = Mathf.Lerp(moveSpeed, walkSpeed, acceleration * Time.deltaTime);
            isRunning = false;
        }
    }

    void ControlDrag()
    {
        if(isGrounded)
        {
            rigidBody.drag = groundDrag;
        }
        else
        {
            rigidBody.drag = airDrag;
        }
    }

    private void FixedUpdate()
    {
        if(rigidBody.useGravity)
        {
            MovePlayer();
        }
    }

    void MovePlayer()
    {
        if(isGrounded && !OnSlope())
        {
            rigidBody.AddForce(moveDirection.normalized * moveSpeed * movementMultiplier, ForceMode.Acceleration);
        }
        else if(isGrounded && OnSlope())
        {
            rigidBody.AddForce(slopeMoveDirection.normalized * moveSpeed * movementMultiplier, ForceMode.Acceleration);
        }
        else if(!isGrounded)
        {
            rigidBody.AddForce(moveDirection.normalized * moveSpeed * movementMultiplier * airMultiplier, ForceMode.Acceleration);
        }
    }
}