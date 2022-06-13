using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadBob : MonoBehaviour
{
    [SerializeField, Range(0, 0.1f)] private float walkAmplitude;
    [SerializeField, Range(0, 0.1f)] private float runAmplitude;
    [SerializeField, Range(0, 30f)] private float walkFrequency;
    [SerializeField, Range(0, 30f)] private float runFrequency;
    [SerializeField] private float returnSpeed;

    [SerializeField] private Transform playerCamera = null;
    [SerializeField] private Transform cameraHolder = null;

    private Vector3 startPosition;
    private float toggleSpeed = 3f;
    private float amplitude;
    private float frequency;
    private PlayerMovement controller;

    private void Awake()
    {
        controller = GetComponent<PlayerMovement>();
        startPosition = playerCamera.localPosition;
    }

    private void Update()
    {
        CheckMotion();
        ResetPosition();
        //playerCamera.LookAt(FocusTarget());
    }

    private void CheckMotion()
    {
        float speed = new Vector3(controller.GetComponent<Rigidbody>().velocity.x, 0, controller.GetComponent<Rigidbody>().velocity.z).magnitude;

        if(speed < toggleSpeed) return;
        if(!controller.isGrounded) return;

        if(controller.isRunning) 
        {
            frequency = runFrequency;
            amplitude = runAmplitude;
        }
        else 
        {
            frequency = walkFrequency;
            amplitude = walkAmplitude;
        }

        playMotion(FootStepMotion());
    }

    private void ResetPosition()
    {
        if(playerCamera.localPosition == startPosition) return;
        playerCamera.localPosition = Vector3.Lerp(playerCamera.localPosition, startPosition, Time.deltaTime * returnSpeed);
    }

    private void playMotion(Vector3 motion)
    {
        playerCamera.localPosition += motion;
    }

    private Vector3 FootStepMotion()
    {
        Vector3 pos = Vector3.zero;
        pos.x += Mathf.Cos(Time.time * frequency / 2) * amplitude * 2;
        pos.y += Mathf.Sin(Time.time * frequency) * amplitude;
        return pos;
    }

    private Vector3 FocusTarget()
    {
        Vector3 pos = new Vector3(transform.position.x, transform.position.y + cameraHolder.localPosition.y, transform.position.z);
        pos += cameraHolder.forward * 15f;
        return pos;
    }

}
