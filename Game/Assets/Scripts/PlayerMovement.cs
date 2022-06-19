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
    [SerializeField] float crouchYscale;
    [SerializeField] float crouchCameraYposition;
    [SerializeField] Transform player;
    [SerializeField] Transform playerCamera;
    [SerializeField] float crouchSpeed;

    private float startYscale;
    private float startCameraYposition;

    [Header("Sprinting")]
    [SerializeField] float walkSpeed = 4f;
    [SerializeField] public float sprintSpeed { get; private set; } = 6f;
    [SerializeField] public float acceleration { get; private set; } = 10f;
    [SerializeField] public float maxAcceleration { get; private set; } = 10f;

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

    [Header("Debug")]
    [SerializeField] TMP_Text velocityText;

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
        startYscale = player.localScale.y;
        startCameraYposition = playerCamera.localPosition.y;
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



        velocityText.text = rigidBody.velocity.magnitude.ToString();
    }

    void SetupInput()
    {
        horizontalMovement = Input.GetAxisRaw("Horizontal");
        verticalMovement = Input.GetAxisRaw("Vertical");

        moveDirection = orientation.forward * verticalMovement + orientation.right * horizontalMovement;
    }

    void CrouchStart()
    {
        player.localScale = new Vector3(player.localScale.x, crouchYscale, player.localScale.z);
        playerCamera.localPosition = new Vector3(playerCamera.localPosition.x, crouchCameraYposition, playerCamera.localPosition.z);
        
        if(isGrounded)
            moveSpeed = crouchSpeed;
        
        isCrouching = true;
    }

    void CrouchEnd()
    {
        player.localScale = new Vector3(player.localScale.x, startYscale, player.localScale.z);
        playerCamera.localPosition = new Vector3(playerCamera.localPosition.x, startCameraYposition, playerCamera.localPosition.z);
        moveSpeed = walkSpeed;

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

    Vector3 AcceleratePlayer(Vector3 vel, Vector3 wishdir)
    {
        float currentSpeed = Vector3.Dot(vel, wishdir);
        float addspeed = sprintSpeed - walkSpeed;
        addspeed = Mathf.Min(addspeed, maxAcceleration * Time.deltaTime);
        return vel + wishdir * addspeed;
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
        MovePlayer();
    }

    void MovePlayer()
    {
        AcceleratePlayer(rigidBody.velocity, playerCamera.forward);

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

    void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawLine(transform.position, rigidBody.velocity.normalized * 1000f);
        Gizmos.color = Color.green;
    }
}