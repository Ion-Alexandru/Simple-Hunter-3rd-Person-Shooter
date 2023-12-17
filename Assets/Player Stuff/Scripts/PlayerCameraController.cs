using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraController : MonoBehaviour
{
    PlayerController playerController;
    public Cinemachine.AxisState xAxis, yAxis;
    [SerializeField] Transform cameraFollowPosition;
    public float mouseSense = 10f;
    public float rotationSpeed = 10f; // Adjust the rotation speed to your liking
    public float damping = 5f; // Adjust the damping factor to your liking

    private float verticalRotation = 0f;

    public Transform characterSpine;

    private void Update()
    {
        float mouseX = Input.GetAxisRaw("Mouse X") * mouseSense;
        float mouseY = -Input.GetAxisRaw("Mouse Y") * mouseSense; // Inverted for more intuitive controls

        xAxis.Value += mouseX;
        yAxis.Value += mouseY;
        yAxis.Value = Mathf.Clamp(yAxis.Value, -90, 90);
    }

    private void LateUpdate()
    {
        // Smoothly interpolate the rotations using Lerp
        cameraFollowPosition.localRotation = Quaternion.Lerp(cameraFollowPosition.localRotation, Quaternion.Euler(yAxis.Value, cameraFollowPosition.localEulerAngles.y, cameraFollowPosition.localEulerAngles.z), Time.deltaTime * rotationSpeed);
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(transform.eulerAngles.x, xAxis.Value, transform.eulerAngles.z), Time.deltaTime * rotationSpeed);


        float mouseY =  Input.GetAxis("Mouse Y");

        // Calculate the new vertical rotation for the camera
        verticalRotation -= mouseY * 5.5f;
        verticalRotation = Mathf.Clamp(verticalRotation, -90, 90);
        characterSpine.transform.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);
    }
}
