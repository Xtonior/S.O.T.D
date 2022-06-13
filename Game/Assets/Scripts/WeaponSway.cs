using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSway : MonoBehaviour
{
    [SerializeField] private float amount;
    [SerializeField] private float maxAmount;
    [SerializeField] private float smooth;

    [SerializeField] private float r_maxAmount;
    [SerializeField] private float r_smooth;

    [SerializeField] private float damp;
    [SerializeField] private float returnSpeed;
    [SerializeField] private float minWalkSpeed;
    [SerializeField] private float z;

    private Vector3 targetPosition;

    [SerializeField] private PlayerMovement player;

    private Vector3 startPosition;
    private Vector3 currentPosition; 
    private Vector3 intermediatePosition;
    private Quaternion startRotation;

    private void Start()
    {
        startPosition = transform.localPosition;
        startRotation = transform.localRotation;
    }

    private void Update()
    {
        float mouseX = -Input.GetAxisRaw("Mouse X") * amount;
        float mouseY = -Input.GetAxisRaw("Mouse Y") * amount;

        mouseX = Mathf.Clamp(mouseX, -maxAmount, maxAmount);
        mouseY = Mathf.Clamp(mouseY, -maxAmount, maxAmount);


        float r_mouseX = -Input.GetAxisRaw("Mouse X") * amount;
        float r_mouseY = -Input.GetAxisRaw("Mouse Y") * amount;

        r_mouseX = Mathf.Clamp(mouseX, -r_maxAmount, r_maxAmount);
        r_mouseY = Mathf.Clamp(mouseY, -r_maxAmount, r_maxAmount);


        Vector3 finalPosition = new Vector3(mouseX, mouseY, 0);
        
        Quaternion xRot = Quaternion.AngleAxis(-mouseY, Vector3.right);
        Quaternion yRot = Quaternion.AngleAxis(mouseX,  Vector3.up);

        Quaternion targetRot = xRot * yRot;

        transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRot, r_smooth);
        transform.localPosition = Vector3.Lerp(transform.localPosition, finalPosition + startPosition, smooth * Time.deltaTime);
    }
}
