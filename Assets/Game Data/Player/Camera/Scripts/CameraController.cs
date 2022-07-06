using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform cameraTranform;
    public Transform playerTransform;

    public float normalSpeed;
    public float fastSpeed;
    public float movementSpeed;
    public float movementTime;
    public float rotationAmount;
    public Vector3 zoomAmount;

    public Vector3 newPosition;
    public Quaternion newRotation;
    public Vector3 newZoom;

    public Vector2 cameraInput
    {
        private get;
        set;
    }
    public bool fastCamera;
    public float cameraRotation;
    public float cameraZoom;
    public bool lockCamera;

    private void Start()
    {
        newPosition = transform.position;
        newRotation = transform.rotation;
        newZoom = cameraTranform.localPosition;
    }

    private void Update()
    {
        HandleMovementInput();
        HandleRotation();
        HandleZoom();
    }

    void HandleMovementInput()
    {
        if(lockCamera)
        {
            newPosition = playerTransform.position;
        }

        //Use fast speed if the shift key is down
        if (fastCamera)
            movementSpeed = fastSpeed;
        else
            movementSpeed = normalSpeed;

        //Right
        if(cameraInput.x >= 0.5f)
        {
            newPosition += (transform.right * movementSpeed);
        }

        //Left
        else if (cameraInput.x <= -0.5f)
        {
            newPosition += (transform.right * -movementSpeed);
        }

        //Forward
        if (cameraInput.y >= 0.5f)
        {
            newPosition += (transform.forward * movementSpeed);
        }

        //Backward
        else if (cameraInput.y <= -0.5f)
        {
            newPosition += (transform.forward * -movementSpeed);
        }

        transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime * movementTime);
    }

    void HandleRotation()
    {
        //Rotate Clockwise
        if (cameraRotation >= 0.5f)
        {
            newRotation *= Quaternion.Euler(Vector3.up * rotationAmount);
        }
        //Rotate anti-Clockwise
        else if (cameraRotation <= -0.5f)
        {
            newRotation *= Quaternion.Euler(Vector3.up * -rotationAmount);
        }
        transform.rotation = Quaternion.Lerp(transform.rotation, newRotation, Time.deltaTime * movementTime);
    }

    void HandleZoom()
    {
        if (cameraZoom >= 1f)
        {
            newZoom -= zoomAmount;
        }
        else if (cameraZoom <= -1f)
        {
            newZoom += zoomAmount;
        }
        cameraTranform.localPosition = Vector3.Lerp(cameraTranform.localPosition, newZoom, Time.deltaTime * movementTime);
    }
    
}
