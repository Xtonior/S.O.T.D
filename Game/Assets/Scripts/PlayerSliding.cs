using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSliding : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Transform orientation;
    [SerializeField] Transform playerCamera;
    [SerializeField] Transform player;
    [SerializeField] Rigidbody rbody;
    [SerializeField] PlayerMovement playerMovement;

    [Header("Settings")]
    [SerializeField] float maxSlideTime;
    [SerializeField] float minSlideSpeed;
    [SerializeField] float slideForce;
    private float slideTimer;

    private float startYscale;
    private float startYposition;
    [SerializeField] float slideYscale;
    [SerializeField] float cameraYposition;

    private float inputX;
    private float inputY;

    public bool isSliding
    {
        get;

        private set;
    }

    private void Start()
    {
        startYscale = player.localScale.y;
        startYposition = playerCamera.localPosition.y;
    }

    private void Update()
    {
        inputX = Input.GetAxisRaw("Horizontal");
        inputY = Input.GetAxisRaw("Vertical");

        if(Input.GetKeyDown(KeyCode.Z) && rbody.velocity.magnitude >= minSlideSpeed && playerMovement.isGrounded)
        {
            StartSlide();
        }

        if(Input.GetKeyUp(KeyCode.Z) && isSliding)
        {
            EndSlide();
        }
    }

    private void FixedUpdate()
    {
        if(isSliding)
        {
            Sliding();
        }
    }

    private void StartSlide()
    {
        isSliding = true;

        player.localScale = new Vector3(player.localScale.x, slideYscale, player.localScale.z);
        playerCamera.localPosition = new Vector3(playerCamera.localPosition.x, cameraYposition, playerCamera.localPosition.z);
        rbody.AddForce(Vector3.down * 50f, ForceMode.Impulse);

        slideTimer = maxSlideTime;
    }

    private void Sliding()
    {
        Vector3 dir = orientation.forward * inputY + orientation.right * inputX;
        isSliding = true;
        rbody.AddForce(dir.normalized * slideForce, ForceMode.Force);

        if(!playerMovement.OnSlope() || rbody.velocity.y > -0.1f)
        {
            rbody.AddForce(dir.normalized * slideForce, ForceMode.Force);
            slideTimer -= Time.deltaTime;
        }
        else if(rbody.velocity.magnitude <= minSlideSpeed)
        {
            EndSlide();
            slideTimer = maxSlideTime;
        }
        else
        {
            rbody.AddForce(playerMovement.GetSlopeDirection(dir) * slideForce, ForceMode.Force);
        }

        if(slideTimer <= 0)
        {
            EndSlide();
        }
    }

    private void EndSlide()
    {
        isSliding = false;

        player.localScale = new Vector3(player.localScale.x, startYscale, player.localScale.z);
        playerCamera.localPosition = new Vector3(playerCamera.localPosition.x, startYposition, playerCamera.localPosition.z);
    }
}
